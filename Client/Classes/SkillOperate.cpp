#include "SkillOperate.h"
#include "Unit.h"
#include "Cell.h"

mutex SkillOperate::mutexSkill;
CCell *SkillOperate::g_cellSkillTarget = NULL;
mutex SkillOperate::mutexDialog;
bool SkillOperate::g_bClickDialogButton = false;
bool SkillOperate::g_bDialogReturn = false;

void SkillOperate::NormalDamage(CUnit *unit, int damage)
{
	if (unit == NULL) return;
	unit->runAction(Sequence::create(CCell::BeAttacked(),
		CallFunc::create([unit, damage]()
			{
				unit->ChangeHP(-damage);
				if (unit->GetHP() == 0)
					unit->UnitDeath();
			}),
		NULL));
}

CUnit *SkillOperate::SelectUnit()
{
	CCell *cell = NULL;
	while (cell == NULL)
	{
		mutexSkill.lock();
		cell = g_cellSkillTarget;
		mutexSkill.unlock();
		Sleep(1);
	}
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
		clickDialogButton = g_bClickDialogButton;
		dialogReturn = g_bDialogReturn;
		mutexDialog.unlock();
		Sleep(1);
	}
	chessboard->SetDialogVisible(false);
	return dialogReturn;
}