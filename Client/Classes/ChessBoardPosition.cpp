#include "ChessboardPosition.h"
#include "Cell.h"

ChessboardPosition::ChessboardPosition(int _x, int _y) :
	x(_x),
	y(_y)
{
}
ChessboardPosition::ChessboardPosition() :
	x(-1),
	y(-1)
{
}

//ChessboardPosition &ChessboardPosition::operator = (ChessboardPosition &rhs)
//{
//	x = rhs.x;
//	y = rhs.y;
//	return *this;
//}

bool ChessboardPosition::operator==(ChessboardPosition &rhs)
{
	if (x == rhs.x &&
		y == rhs.y)
		return true;
	return false;
}

bool ChessboardPosition::operator!=(ChessboardPosition &rhs)
{
	return !(*this == rhs);
}

void ChessboardPosition::SetPosition(int _x, int _y)
{
	x = _x;
	y = _y;
}

Point ChessboardPosition::CastPoint()
{
	return CLayerChessboard::getInstance()->GetChessboardPosition(*this);
}

//CCell *ChessboardPosition::GetCell()
//{
//	return CLayerChessboard::getInstance()->GetCell(*this);
//}

bool ChessboardPosition::Adjacent(ChessboardPosition &position)
{
	return abs(position.x - x) + abs(position.y - y) == 1;

}

bool ChessboardPosition::InRange(ChessboardPosition &position, int range)
{
	return abs(position.x - x) + abs(position.y - y) <= range;
}

bool ChessboardPosition::OnChessboard()
{
	return 0 <= x && x < CHESSBOARD_MAX_X &&
		0 <= y && y < CHESSBOARD_MAX_Y;
}