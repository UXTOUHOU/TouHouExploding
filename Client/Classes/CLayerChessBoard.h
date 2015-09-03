#pragma once

#include "cocostudio/CocoStudio.h"
#include "ui/CocosGUI.h"

class CLayerChessBoard : public Layer
{
public:
	virtual bool init() override;
	CREATE_FUNC(CLayerChessBoard);

	//
	void ShowSprite(Sprite *sprite, int x, int y);
	void ShowMapCell(Sprite *sprite, int x, int y);
	void ShowAtkRange(int x, int y, int range);
	void ShowMovableRange(int x, int y, int range);
	void ShowSelectCell(int x, int y);
	void DelSprite(int x, int y);
	Point GetChessBoardPosition(int x, int y);

	void MouseMove(float mouseX, float mouseY);
private:
	const float spacingX = 75, spacingY = 75;
	Point chessBoardPosition;
	Sprite *chessBoardBackGround[8][12];
	Sprite *unit[8][12];
};

Sprite *CreateUnitSprite(int unitID);
char *GetUnitFileName(char *fileName, int unitID);