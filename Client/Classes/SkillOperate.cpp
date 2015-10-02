#include "SkillOperate.h"
#include "Unit.h"
#include "Cell.h"

bool SkillOperate::bWaitSelectCell = false;
mutex SkillOperate::mutexSkill;
CCell *SkillOperate::cellSkillTarget = NULL;
mutex SkillOperate::mutexDialog;
bool SkillOperate::bClickDialogButton = false;
bool SkillOperate::bDialogReturn = false;

void SkillOperate::NormalDamage(CUnit *unit, int damage)
{
	if (unit == NULL) return;
	unit->runAction(Sequence::create(CCell::BeAttacked(),
		CallFunc::create([unit, damage]()
			{
				unit->ChangeHP(-damage);
				if (unit->GetHP() == 0)
				{
					unit->UnitDeath();
					CLayerChessboard::getInstance()->GetCell(unit->GetChessboardPosition())->unit = NULL;
				}
			}),
		NULL));
}
       
CUnit *SkillOperate::SelectUnit()
{
	CCell *cell = NULL;
	bWaitSelectCell = true;
	cellSkillTarget = NULL;
	while (cell == NULL)
	{
		mutexSkill.lock();
		cell = cellSkillTarget;
		mutexSkill.unlock();
		Sleep(1);
	}
	bWaitSelectCell = false;
	return cell->unit;
}

bool SkillOperate::ChessboardDialog(string str)
{
	auto chessboard = CLayerChessboard::getInstance();
	chessboard->SetDialogString(str);
	chessboard->SetDialogVisible(true);
	bool clickDialogButton = false;
	bool dialogReturn;
	while (!clickDialogButton)
	{
		mutexDialog.lock();
		clickDialogButton = bClickDialogButton;
		dialogReturn = bDialogReturn;
		mutexDialog.unlock();
		Sleep(1);
	}
	chessboard->SetDialogVisible(false);
	return dialogReturn;
}

void SkillOperate::DispatchEventSkillEnd()
{
	Director::getInstance()->getEventDispatcher()->dispatchCustomEvent("SkillEnd", NULL);
}

void SkillOperate::AddEventSkillEndListener(const function<void(EventCustom *)> &callBack)
{
	Director::getInstance()->getEventDispatcher()->addCustomEventListener("SkillEnd", callBack);
}

