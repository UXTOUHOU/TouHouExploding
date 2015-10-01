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
	extern bool g_bRunningSkill;
	extern mutex mutexSkill;
	extern CCell *g_cellSkillTarget;
	extern mutex mutexDialog;
	extern bool g_bClickDialogButton;
	extern bool g_bDialogReturn;

	//vector<CSkill *> vecOnUnitDead;

	void NormalDamage(CUnit *unit, int damage);
	CUnit *SelectUnit();
	CCard *SelectCard();
	void AddDelaySkill();
	bool ChessboardDialog(string str);
}