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
	RoundCountdown(1000.F);
	for (int i = 0; i < 12; ++i)
	{
		Sprite *sprite = CreateUnitSprite(i + 1);
		chessBoard->ShowSprite(sprite, i, 0);
		if (i < 4)
			Effects::Moveable(sprite);
		else if (i < 8)
			Effects::Normal(sprite);
		else
			Effects::Moved(sprite);
	}

	connect->Login("123", "456");
	connect->AskRoomList();
	//
	return true;
}
