#pragma once

#include "cocostudio/CocoStudio.h"
#include "ui/CocosGUI.h"
#include "Cell.h"

class CLayerChessBoard : public Layer
{
public:
	virtual bool init() override;
	CREATE_FUNC(CLayerChessBoard);
	//
	void SetUnit(int unitID, int x, int y);
	void ShowMapCell(Sprite *sprite, int x, int y);
	void ShowAtkRange(int x, int y, int range);
	void ShowMovableRange(int x, int y, int range);
	void ShowSelectCell(int x, int y);
	void DelSprite(int x, int y);
	Point GetChessBoardPosition(int x, int y);

	CCell *GetCell(int x, int y);
	void ChangeUnitPosition(int x1, int y1, int x2, int y2);
	void ShowAttribut(float dt);

	void MouseMove(float mouseX, float mouseY);
private:
	CCell *_currentCell;
	const float _spacingX = 75, _spacingY = 75;
	Point _chessBoardPosition;

	CCell *_cell[8][12];

	void _clearBackGround();
};