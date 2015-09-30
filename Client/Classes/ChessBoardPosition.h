#pragma once
#include "cocos2d.h"
USING_NS_CC;
//#include "Cell.h"

//class CCell;
class ChessboardPosition;

class ChessboardPosition
{
public:
	int x;
	int y;
	void SetPosition(int _x, int _y);
	Point CastPoint();
	//CCell *GetCell();
	bool OnChessboard();
	bool Adjacent(ChessboardPosition &position);
	bool InRange(ChessboardPosition &position, int range);
	ChessboardPosition(int _x, int _y);
	ChessboardPosition();
	bool operator==(ChessboardPosition &rhs);
	bool operator!=(ChessboardPosition &rhs);
};
