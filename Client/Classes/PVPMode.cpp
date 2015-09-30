#include "PVPMode.h"

CPVPMode::CPVPMode()
{

}

CPVPMode::~CPVPMode()
{

}

bool CPVPMode::init()
{
	CSceneBattle::init();
	ChangeState(PS_Wait);
	CPVPConnect *connect = CPVPConnect::getInstance();
	//test//////////////////////////////////////////////////////////
	auto order1 = CCommunity();
	auto str = order1.ValueToJson(order1.MakeJsonValue());
	connect->_SendToServer(str);
	//
	//test
	AddHandCards(0);
	AddHandCards(1);
	AddSummonPool(1);
	RoundCountdown(1000.F);
	for (int i = 0; i < 12; ++i)
	{
		chessboard->SetUnit(i + 1, i, 0);
		//HPÏÔÊ¾²âÊÔ
		auto cell = chessboard->GetCell(ChessboardPosition(i, 0));
		auto unit = cell->unit;
		unit->SetHP(10);
		unit->SetHPVisible(true);
		chessboard->addChild(unit->GetLabelHP());
		if (i % 2)
			unit->SetGroup(UG_ENEMY);
		else
			unit->SetGroup(UG_YOURSELF);
		unit->SetGroupPosition(cell->chessboardPosition);
		chessboard->addChild(unit->GetSpriteGroup());
		
		//test
		//if (i < 4)
		//	cell->unit->runAction(CCell::Moveable());
		//else if (i < 8)
		//	;// cell->Normal();
		//else
		//	cell->unit->runAction(CCell::Moved());
	}
	//test
	//connect->Login("123", "456");
	//connect->AskRoomList();

	StartYourTurn();
	//
	return true;
}

