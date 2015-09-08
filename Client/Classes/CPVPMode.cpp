#include "CPVPMode.h"

CPVPMode::CPVPMode()
{

}

CPVPMode::~CPVPMode()
{

}

bool CPVPMode::init()
{
	CSceneBattle::init();
	CPVPConnect *connect = CPVPConnect::getInstance();
	//test
	AddHandCards(0);
	AddHandCards(1);
	AddSummonPool(1);
	RoundCountdown(5.F);
	for (int i = 0; i < 12; ++i)
	{
		Sprite *sprite = CreateUnitSprite(i + 1);
		chessBoard->ShowSprite(sprite, i, 0);
		if (i < 11)
			Effects::Moveable(sprite);
		else
			Effects::Moved(sprite);
	}

	connect->login("123", "456");
	connect->askRoomList();
	//
	return true;
}
