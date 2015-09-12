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
		chessBoard->SetUnit(i + 1, i, 0);
		//HPÏÔÊ¾²âÊÔ
		auto cell = chessBoard->GetCell(i, 0);
		cell->SetHP(10);
		chessBoard->addChild(cell->unitState.lblHP);
		//
		if (i < 4)
			cell->Moveable();
		else if (i < 8)
			cell->Normal();
		else
			cell->Moved();
	}

	connect->Login("123", "456");
	connect->AskRoomList();
	//
	return true;
}
