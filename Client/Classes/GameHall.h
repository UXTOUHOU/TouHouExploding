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
	virtual void OnMouseDown(EventMouse *eventMouse) override;
	virtual void OnMouseScroll(EventMouse *eventMouse) override;

	void OnButtonJoinGame();
	void EffectSelectRoom(ui::Layout *room);
	void AddRoomList(std::string ID, std::string roomName, std::string playerName);

	ui::ListView *listViewRoomList;
	ui::Layout *selectedRoom;

	CGameHall();
	~CGameHall();
};

