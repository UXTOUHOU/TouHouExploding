#pragma once
#include "ChessBoard.h"
#include "SkillOperate.h"
#include "ToolFunc.h"
using namespace SkillOperate;
extern std::mutex mutexSkill;
class CSkill
{
public:
	void SetUsable(bool usable);
	bool GetUsable() const;

	void RunSkill();
	virtual void OnSkillStart() = 0;
	virtual bool IsSelectable(ChessboardPosition unitPosition, ChessboardPosition position);
	virtual bool IsTarget(ChessboardPosition cursor, ChessboardPosition position);
private:
	bool _usable;
	CUnit *_unit;

	int _minSkillRange;
	int _maxSkillRange;
};

class CSkill_1 :public CSkill
{
public:
	void OnSkillStart() override;
};