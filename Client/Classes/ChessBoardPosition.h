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
	Point CastPoint() const;
	//CCell *GetCell();
	int Distance(ChessboardPosition position) const;
	bool OnChessboard() const;
	bool Adjacent(ChessboardPosition &position) const;
	bool InRange(ChessboardPosition &position, int range) const;
	ChessboardPosition(int _x, int _y);
	ChessboardPosition();
	bool operator==(ChessboardPosition &rhs) const;
	bool operator!=(ChessboardPosition &rhs) const;
};
