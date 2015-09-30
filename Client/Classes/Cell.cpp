#include "Cell.h"

void CCell::SetUnit(int unitID, ChessboardPosition position)
{
	Point point = CLayerChessboard::getInstance()->GetChessboardPosition(position);
	if (unit)
	{
		unit->removeFromParentAndCleanup(true);
		unit->setPosition(point);
	}
	else{
		unit = CUnit::create();
		unit->SetChessboardPosition(position);
		unit->initWithFile(GetUnitFileName(unitID));
		unit->InitCardAttribute(unitID);
		unit->setScale(1.5F);
		unit->setPosition(point);
		unit->SetHP(unit->hp);
		unit->SetHPPosition(position);
		unit->SetHPVisible(true);
	}
	CCAssert(unit, "单位图片不存在");
}

ChessboardPosition CCell::GetCellNum()
{
	return chessboardPosition;// chessboardPosition;
}

void CCell::DelUnit()
{
	if (unit)
		unit->removeFromParentAndCleanup(true);
	unit = NULL;
}

void CCell::SwapUnit(CCell *cell)
{
	auto tUnit = cell->unit;
	cell->unit = unit;
	unit = tUnit;
}

ActionInterval *CCell::Moveable()
{
	auto move1 = MoveBy::create(1, Point(0, 5));
	auto move2 = move1->reverse();
	return RepeatForever::create(Sequence::create(move1, move2, NULL));
}

ActionInterval *CCell::Moved()
{
	return TintTo::create(0.F, 120, 120, 120);
}

//Action *CCell::Normal()
//{
//	unit->setOpacity(255);
//}

ActionInterval *CCell::Blink(Point position)
{
	return MoveTo::create(0, position);
}

ActionInterval *CCell::MoveToPosition(ChessboardPosition position)
{
	//unit->stopAllActions();
	//unit->runAction(
	return MoveTo::create(1, CLayerChessboard::getInstance()->GetChessboardPosition(position));
}

ActionInterval *CCell::BeAttacked()
{
	return Spawn::create(Sequence::create(MoveBy::create(0.08F, Point(6, 0)),
		MoveBy::create(0.12F, Point(-6, 0)),
		NULL),
		Sequence::create(TintTo::create(0.F, Color3B(209, 130, 130)),
		DelayTime::create(0.2F),
		TintTo::create(0.F, Color3B::WHITE),
		NULL),
		//Sequence::create(TintTo::create(0.08F, Color3B(149, 70, 70)),
		//TintTo::create(0.12F, Color3B::WHITE),
		//NULL),
		NULL);
}

CCell::CCell() :
	unit(NULL),
	chessboardPosition(-1, -1)
{
}

CCell::~CCell()
{
}

void CCell::MoveWithPath(list<ChessboardPosition> &listMovePath)
{
	if (!listMovePath.empty())
	{
		ChangeState(PS_WaitMoveAnimateEnd);
		auto &selectedCell = CLayerChessboard::getInstance()->selectedCell;
		auto target = *listMovePath.begin();
		selectedCell->unit->SetHPPosition(target);
		selectedCell->unit->SetGroupPosition(target);
		unit->runAction(Sequence::create(MoveTo::create(0.7F, target.CastPoint()),
			CallFunc::create([&selectedCell, &listMovePath](){selectedCell->MoveWithPath(listMovePath); }),
			NULL));
		auto *nextCell = CLayerChessboard::getInstance()->GetCell(*listMovePath.begin());
		SwapUnit(nextCell);
		selectedCell = nextCell;
		listMovePath.pop_front();
	}
	else{
		//unitState.state = UnitState::StateType::ST_Moved;
		unit->SetMoveable(false);
		unit->runAction(Moved());
		ChangeState(PS_SelectUnitBehavior);
	}
}
