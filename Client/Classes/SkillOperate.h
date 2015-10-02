#pragma once
#include <vector>
#include "ChessBoardPosition.h"
using namespace std;
class CSkill;
class CUnit;
class CCard;
class CCell;
namespace SkillOperate
{
	extern bool bWaitSelectCell;
	extern bool bRunningSkill;
	extern mutex mutexSkill;
	extern CCell *cellSkillTarget;
	extern mutex mutexDialog;
	extern bool bClickDialogButton;
	extern bool bDialogReturn;
	//vector<CSkill *> vecOnUnitDead;

	void DispatchEventSkillEnd();
	void AddEventSkillEndListener(const function<void(EventCustom *)> &callBack);

	void NormalDamage(CUnit *unit, int damage);
	CUnit *SelectUnit();
	CCard *SelectCard();
	void AddDelaySkill();
	bool ChessboardDialog(string str);
}