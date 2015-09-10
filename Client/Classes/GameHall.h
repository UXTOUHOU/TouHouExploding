#pragma once
#include "LayerMenu.h"
class CGameHall : public CLayerMenu
{
public:
	static CGameHall *pGameHall;
	static CGameHall *getInstance()
	{
		if (!pGameHall)
			pGameHall = CGameHall::create();
		return pGameHall;
	}
	virtual bool init() override;
	CREATE_FUNC(CGameHall);

	virtual void Enter() override;
	virtual void OnButtonReturn() override;
	void OnButtonJoinGame();

	ui::ListView *roomList;

	CGameHall();
	~CGameHall();
};

