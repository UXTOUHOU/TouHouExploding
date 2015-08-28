#include "CLayerChessBoard.h"
#include "CCard.h"
#define CHESS_BOARD_MAX_X 12
#define CHESS_BOARD_MAX_Y 8

bool CLayerChessBoard::init()
{
	Layer::init();
	memset(unit, NULL, sizeof(unit));
	chessBoardPosition = Point(446.40F, 241.11F);
	
	SpriteBatchNode *batchNode = SpriteBatchNode::create("temp.png");
	for (int y = 0; y < 8; ++y)
		for (int x = 0; x < 12; ++x)
		{
			Sprite *sprite = Sprite::createWithTexture(batchNode->getTexture());
			batchNode->addChild(sprite);
			sprite->setColor(Color3B::BLACK);
			ShowMapCell(sprite, x, y);
			chessBoardBackGround[y][x] = sprite;
		}
	addChild(batchNode);
	//
	DrawNode *drawNode = DrawNode::create();
	Point position = chessBoardPosition;
	for (int i = 0; i < 13; ++i)
	{
		drawNode->drawLine(position, position + Vec2(0, 600), Color4F::WHITE);
		position.x += spacingX;
	}
	position = chessBoardPosition;
	for (int i = 0; i < 9; ++i)
	{
		drawNode->drawLine(position, position + Vec2(900, 0), Color4F::WHITE);
		position.y += spacingY;
	}
	addChild(drawNode);
	//
	return true;
}

void CLayerChessBoard::ShowSprite(Sprite *sprite, int x, int y)
{
	sprite->setPosition(GetChessBoardPosition(x, y));
	addChild(sprite);
}

void CLayerChessBoard::ShowMapCell(Sprite *sprite, int x, int y)
{
	sprite->setPosition(GetChessBoardPosition(x, y));
}

void CLayerChessBoard::DelSprite(int x, int y)
{
	Sprite *sprite = unit[y - 1][x - 1];
	sprite->removeFromParentAndCleanup(true);
}

Point CLayerChessBoard::GetChessBoardPosition(int x, int y)
{
	Point position = chessBoardPosition;
	position.x += (x + 0.5F) * spacingX;
	position.y += (y + 0.5F) * spacingY;
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
	Point position = chessBoardPosition;
	float fx = (mouseX - position.x) / spacingX;
	float fy = (mouseY - position.y) / spacingY;
	int x = fx;
	int y = fy;
	if (fx >= 0 && fy >= 0 &&
		0 <= x && x < 12 &&
		0 <= y && y < 8)
	{
		//test
		ShowSelectCell(x, y);
		//
		log("ChessBoard %d %d", x, y);
	}
	else{
		for (int i = 0; i < 8; ++i)
			for (int j = 0; j < 12; ++j)
				chessBoardBackGround[i][j]->setColor(Color3B(0, 0, 0));
	}
}

Sprite *CreateUnitSprite(int unitID)
{
	char fileName[20];
	Sprite *sprite = Sprite::create(GetUnitFileName(fileName, unitID));
	return sprite;
}

char *GetUnitFileName(char *fileName, int unitID)
{
	//返回 Unit_ID.png 格式的图片名
	strcpy(fileName, "Unit_");
	char strID[10];
	_itoa(unitID, strID, 10);
	strcat(fileName, strID);
	strcat(fileName, ".png");
	return fileName;
}

void CLayerChessBoard::ShowMovableRange(int x, int y, int range)
{
	for (int i = 1; i <= 8; ++i)
		for (int j = 1; j <= 12; ++j)
			chessBoardBackGround[i - 1][j - 1]->setColor(Color3B(0, 0, 0));

	for (int i = MAX(1, y - range); i <= MIN(CHESS_BOARD_MAX_Y, y + range); ++i)
		for (int j = MAX(1, x - range); j <= MIN(CHESS_BOARD_MAX_X, x + range); ++j)
		{
		if (abs(y - i) + abs(x - j) <= range)
		{
			chessBoardBackGround[i - 1][j - 1]->setColor(Color3B(70, 112, 70));
		}
	}
}

void CLayerChessBoard::ShowAtkRange(int x, int y, int range)
{
	for (int i = 1; i <= 8; ++i)
		for (int j = 1; j <= 12; ++j)
			chessBoardBackGround[i - 1][j - 1]->setColor(Color3B(0, 0, 0));

	for (int i = MAX(1, y - range); i <= MIN(CHESS_BOARD_MAX_Y, y + range); ++i)
		for (int j = MAX(1, x - range); j <= MIN(CHESS_BOARD_MAX_X, x + range); ++j)
		{
			if (abs(y - i) + abs(x - j) <= range)
			{
				chessBoardBackGround[i - 1][j - 1]->setColor(Color3B(149, 70, 70));
			}
		}
}

void CLayerChessBoard::ShowSelectCell(int x, int y)
{
	for (int i = 0; i < 8; ++i)
		for (int j = 0; j < 12; ++j)
			chessBoardBackGround[i][j]->setColor(Color3B(0, 0, 0));

	chessBoardBackGround[y][x]->setColor(Color3B(Color3B(60, 81, 102)));
}