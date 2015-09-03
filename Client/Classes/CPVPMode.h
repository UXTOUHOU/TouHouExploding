#pragma once
#include "CSceneBattle.h"
#include "Effects.h"

/*
	对战模式
*/

class CPVPMode : public CSceneBattle
{
public:
	virtual bool init() override;
	CREATE_FUNC(CPVPMode);
	CPVPMode();
	~CPVPMode();

	void StartYourTurn();
	void EndYourTurn();
	void SelectUnit();
	void SelectPosition();
};