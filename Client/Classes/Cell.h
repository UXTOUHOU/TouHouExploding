#pragma once

#include "cocostudio/CocoStudio.h"
#include "ChessboardPosition.h"
#include "SceneBattle.h"
#include "Chessboard.h"
#include "Unit.h"

//char *GetUnitFileName(char *fileName, int unitID);
class CCell;

class CCell
{
public:
	Point cellPosition;
	ChessboardPosition chessboardPosition;

	CUnit *unit;
	//struct UnitState
	//{
	//	//enum StateType{
	//	//	ST_Moveable,
	//	//	ST_Moved,
	//	//}state;
	//	UnitState() :
	//		lblHP(NULL),
	//		groupType(UG_YOURSELF),
	//		group(NULL),
	//		lblAttribute(NULL),
	//		attributeBackground(NULL),
	//		bMoveable(false),
	//		skillNum(1)
	//	{
	//	}
	//}unitState;

	ChessboardPosition GetCellNum();
	void SetUnit(int unitID, ChessboardPosition postiton);
	void SwapUnit(CCell *cell);
	//特效
	void MoveWithPath(std::list<ChessboardPosition> &listMovePath);

	static ActionInterval *Moveable();									//未移动的单位
	static ActionInterval *Moved();										//已移动的单位
	//static Action *Normal();											//通常状态
	static ActionInterval *Blink(Point position);						//闪烁至某格
	static ActionInterval *BeAttacked();								//
	static ActionInterval *MoveToPosition(ChessboardPosition position);

	Sprite *backGround;
	
	CCell();
	~CCell();
protected:
	const float _attributeBorderWidth = 5;
};
