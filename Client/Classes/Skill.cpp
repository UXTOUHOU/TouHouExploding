#include "Skill.h"
extern std::mutex mutexSkill;

bool SkillOperate::bRunningSkill = false;

void CSkill::RunSkill()
{
	SkillOperate::mutexSkill.lock();
	if (!bRunningSkill)
	{
		//initState
		bClickDialogButton = false;
		//
		std::thread thrSkill(&CSkill::OnSkillStart, this);
		thrSkill.detach();
	}
	SkillOperate::mutexSkill.unlock();
}

bool CSkill::GetUsable() const
{
	return _usable;
}

void CSkill::SetUsable(bool usable)
{
	_usable = usable;
}

bool CSkill::IsSelectable(ChessboardPosition unitPosition, ChessboardPosition position)
{
	int distance = unitPosition.Distance(position);
	return _minSkillRange <= distance &&
		distance <= _maxSkillRange;
}

bool CSkill::IsTarget(ChessboardPosition cursor, ChessboardPosition position)
{
	return position == cursor;
}

void CSkill_1::OnSkillStart()
{
	SetUsable(false);
	//
	auto unit = SelectUnit();
	bool buttonYes = ChessboardDialog(WStrToUTF8(L"是否支付1B对该单位造成额外的2点伤害？"));
	if (buttonYes)
		NormalDamage(unit, 2);
	//
	bRunningSkill = false;
	DispatchEventSkillEnd();
}