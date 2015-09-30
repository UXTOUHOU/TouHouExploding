#pragma once
#include "SkillOperate.h"
#include "ToolFunc.h"
using namespace SkillOperate;
extern std::mutex mutexSkill;
class CSkill
{
public:
	bool RunSkill();
	virtual void OnSkillStart() = 0;
	//virtual void OnShowSelectableRange();
	//virtual void OnShowTargetRange();
private:
	bool _usable;
};

class CSkill_1 :public CSkill
{
public:
	void OnSkillStart() override;
};