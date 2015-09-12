#pragma once

#include "cocostudio/CocoStudio.h"

char *GetUnitFileName(char *fileName, int unitID);

enum UnitCamp
{
	UC_YOURSELF,
	UC_ENEMY,
};

class CCell
{
public:
	Point cellPosition;

	Sprite *unit;
	void SetUnit(int unitID, Point postiton);
	void DelUnit();
	struct
	{
		Label *lblHP;
		DrawNode *camp;
		Label *lblAttribute;
	}unitState;
	//单位左上角的HP显示
	void SetHP(int HP);
	void SetHPVisable(bool visable);
	//单位右上角的阵营显示
	void SetCamp(UnitCamp campType);
	void SetCampVisable(bool visable);
	//鼠标悬停时的属性悬浮窗
	void SetAttribute(std::string attribute);
	void SetAttributeVisable(bool visable);
	//特效
	void Moveable();					//未移动的单位
	void Moved();						//已移动的单位
	void Normal();						//通常状态
	void Blink(Point position);			//闪烁至某格
	void MoveTo(Point position);

	Sprite *backGround;
	
	CCell();
	~CCell();
};
