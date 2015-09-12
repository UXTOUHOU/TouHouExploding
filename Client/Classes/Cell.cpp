#include "Cell.h"
#include "CSceneBattle.h"

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

void CCell::SetUnit(int unitID, Point position)
{
	if (unit)
		unit->removeFromParentAndCleanup(true);
	char fileName[20];
	unit = Sprite::create(GetUnitFileName(fileName, unitID));
	unit->setPosition(position);
	unit->setScale(1.5);
	CCAssert(unit, "单位图片不存在");
}

void CCell::DelUnit()
{
	if (unit)
		unit->removeFromParentAndCleanup(true);
	unit = NULL;
}

void CCell::SetHP(int HP)
{
	if (!unitState.lblHP)
	{
		unitState.lblHP = Label::create();
		unitState.lblHP->setSystemFontSize(16);
		unitState.lblHP->setPosition(unit->getPosition() + Point(-75.F / 2 + 3, 75.F / 2 - 3));
		unitState.lblHP->setAnchorPoint(Point(0, 1));
		unitState.lblHP->setColor(Color3B::RED);
	}
	std::wstring wHP;
	std::wstringstream wss;
	wss << HP;
	wss >> wHP;
	unitState.lblHP->setString(WStrToUTF8(wHP));
}

void CCell::SetHPVisable(bool visable)
{
	unitState.lblHP->setVisible(visable);
}

void CCell::SetAttribute(std::string attribute)
{
	if (!unitState.lblAttribute)
	{
		unitState.lblAttribute = Label::create();
		unitState.lblAttribute->setSystemFontSize(24);
		unitState.lblAttribute->setPosition(unit->getPosition() + Point(75.F / 2 - 3, 75.F / 2 - 3));
		unitState.lblAttribute->setAnchorPoint(Point(0, 0));
	}
	unitState.lblAttribute->setString(attribute);
}

void CCell::Moveable()
{
	unit->stopAllActions();
	auto move1 = MoveBy::create(1, Point(0, 5));
	auto move2 = move1->reverse();
	unit->runAction(RepeatForever::create(Sequence::create(move1, move2, NULL)));
}

void CCell::Moved()
{
	unit->stopAllActions();
	unit->setOpacity(100);
}

void CCell::Normal()
{
	unit->stopAllActions();
	unit->setOpacity(255);
}

void CCell::Blink(Point position)
{
	unit->setPosition(position);
}

void CCell::MoveTo(Point position)
{
	unit->stopAllActions();
	unit->runAction(MoveTo::create(1, position));
}

CCell::CCell()
{
	unit = NULL;
	unitState.lblHP = NULL;
	unitState.camp = NULL;
	unitState.lblAttribute = NULL;
}

CCell::~CCell()
{
}
