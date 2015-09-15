#include "CLayerChessBoard.h"
#include "CSceneBattle.h"
#include "CCard.h"
#define CHESS_BOARD_MAX_X 12
#define CHESS_BOARD_MAX_Y 8

bool CLayerChessBoard::init()
{
	Layer::init();
	memset(_cell, NULL, sizeof(_cell));
	_chessBoardPosition = Point(446.40F, 241.11F);
	_currentCell = NULL;
	//
	SpriteBatchNode *batchNode = SpriteBatchNode::create("temp.png");
	for (int y = 0; y < 8; ++y)
		for (int x = 0; x < 12; ++x)
		{
			_cell[y][x] = new CCell;
			auto &cell = _cell[y][x];
			//
			cell->backGround = Sprite::createWithTexture(batchNode->getTexture());
			batchNode->addChild(cell->backGround);
			cell->backGround->setColor(Color3B::BLACK);
			ShowMapCell(cell->backGround, x, y);
		}
	addChild(batchNode, -1);
	//
	DrawNode *drawNode = DrawNode::create();
	Point position = _chessBoardPosition;
	for (int i = 0; i < 13; ++i)
	{
		drawNode->drawLine(position, position + Vec2(0, 600), Color4F::WHITE);
		position.x += _spacingX;
	}
	position = _chessBoardPosition;
	for (int i = 0; i < 9; ++i)
	{
		drawNode->drawLine(position, position + Vec2(900, 0), Color4F::WHITE);
		position.y += _spacingY;
	}
	addChild(drawNode, 0);
	//
	for (int y = 0; y < 1; ++y)
		for (int x = 0; x < 12; ++x)
		{
			auto &cell = _cell[y][x];
			cell->SetAttribute(WStrToUTF8(L"名字: ...\n血量: 0\n机动: 0\n攻击: 0\n射程: 0\n技能: ..."));
			addChild(cell->unitState.attributeBackground, 1);
			addChild(cell->unitState.lblAttribute, 1);
		}
	return true;
}

void CLayerChessBoard::SetUnit(int unitID, int x, int y)
{
	_cell[y][x]->SetUnit(unitID, GetChessBoardPosition(x, y));
	addChild(_cell[y][x]->unit);
}

void CLayerChessBoard::ShowMapCell(Sprite *sprite, int x, int y)
{
	sprite->setPosition(GetChessBoardPosition(x, y));
}

void CLayerChessBoard::DelSprite(int x, int y)
{
	_cell[y][x]->DelUnit();
}

Point CLayerChessBoard::GetChessBoardPosition(int x, int y)
{
	Point position = _chessBoardPosition;
	position.x += (x + 0.5F) * _spacingX;
	position.y += (y + 0.5F) * _spacingY;
	return position;
}

void CLayerChessBoard::MouseMove(float mouseX, float mouseY)
{
	////test
	//static bool b = false;
	//static CCard *card = CCard::create(0);
	//if (!b)
	//	card->ShowCard(Rect(mouseX, mouseY, 400, 400), this);
	//b = true;
	//card->setPosition(mouseX, mouseY);
	////
	Point position = _chessBoardPosition;
	float fx = (mouseX - position.x) / _spacingX;
	float fy = (mouseY - position.y) / _spacingY;
	int x = fx;
	int y = fy;
	static int prevX = -1, prevY = -1;
	if (fx >= 0 && fy >= 0 &&	//防止负数转换时被近似成0
		0 <= x && x < 12 &&
		0 <= y && y < 8)
	{
		//在棋盘内的移动
		if (_currentCell)
		{
			_currentCell->SetAttributePosition(Point(mouseX, mouseY));
		}

		if (prevX != x ||
			prevY != y)
		{
			//改变了格子
			prevX = x;
			prevY = y;
			log("change cell");
			unschedule(schedule_selector(CLayerChessBoard::ShowAttribut));
			if (_currentCell)
				_currentCell->SetAttributeVisable(false);
			_currentCell = GetCell(x, y);
			schedule(schedule_selector(CLayerChessBoard::ShowAttribut), 1.F);
		}
		//test
		ShowSelectCell(x, y);
		//
		log("ChessBoard %d %d", x, y);
	}
	else{
		//移出了棋盘
		prevX = -1;
		prevY = -1;
		unschedule(schedule_selector(CLayerChessBoard::ShowAttribut));
		if (_currentCell)
			_currentCell->SetAttributeVisable(false);
		_clearBackGround();
		log("out chellboard");
	}
}

void CLayerChessBoard::ShowMovableRange(int x, int y, int range)
{
	_clearBackGround();

	for (int i = MAX(0, y - range); i < MIN(CHESS_BOARD_MAX_Y, y + range + 1); ++i)
		for (int j = MAX(0, x - range); j < MIN(CHESS_BOARD_MAX_X, x + range + 1); ++j)
		{
			if (abs(y - i) + abs(x - j) <= range)
			{
				_cell[i][j]->backGround->setColor(Color3B(70, 112, 70));
			}
		}
}

void CLayerChessBoard::ShowAtkRange(int x, int y, int range)
{
	_clearBackGround();

	for (int i = MAX(0, y - range); i < MIN(CHESS_BOARD_MAX_Y, y + range + 1); ++i)
		for (int j = MAX(0, x - range); j < MIN(CHESS_BOARD_MAX_X, x + range + 1); ++j)
		{
			if (abs(y - i) + abs(x - j) <= range)
			{
				_cell[i][j]->backGround->setColor(Color3B(149, 70, 70));
			}
		}
}

void CLayerChessBoard::ShowSelectCell(int x, int y)
{
	_clearBackGround();

	_cell[y][x]->backGround->setColor(Color3B(Color3B(60, 81, 102)));
}

CCell *CLayerChessBoard::GetCell(int x, int y)
{
	return _cell[y][x];
}

void CLayerChessBoard::_clearBackGround()
{
	for (int i = 0; i < 8; ++i)
		for (int j = 0; j < 12; ++j)
			_cell[i][j]->backGround->setColor(Color3B(0, 0, 0));
}

void CLayerChessBoard::ShowAttribut(float dt)
{
	if (_currentCell)
		_currentCell->SetAttributeVisable(true);
}