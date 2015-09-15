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
		unitState.lblHP->setSystemFontSize(12);
		unitState.lblHP->setScale(1.33F);
		unitState.lblHP->setPosition(unit->getPosition() + Point(-75.F / 2 + 3, 75.F / 2 - 1));
		unitState.lblHP->setAnchorPoint(Point(0, 1));
		//unitState.lblHP->setColor(Color3B::RED);
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
		unitState.lblAttribute->setAnchorPoint(Point(0, 0));
		//backGround->addChild(unitState.lblAttribute);
		//
		unitState.attributeBackground = DrawNode::create();
		unitState.attributeBackground->setAnchorPoint(Point(0, 0));
	}
	unitState.lblAttribute->setString(attribute);
	unitState.lblAttribute->setVisible(false);

	auto position = unitState.lblAttribute->getPosition();
	auto size = unitState.lblAttribute->getContentSize();
	Size border(_attributeBorderWidth, _attributeBorderWidth);
	position += border;
	
	Vec2 verts[4] = { position, position, position, position };
	verts[0] -= border;
	verts[1] += Point(-border.width, size.height + border.height);
	verts[2] += size + border;
	verts[3] += Point(size.width + border.width, -border.height);
	unitState.attributeBackground->drawPolygon(verts, 4, Color4F(0, 0, 0, 0.7F), 0, Color4F(0, 0, 0, 0));
	unitState.attributeBackground->setOpacity(50);
	unitState.attributeBackground->setVisible(false);
}

void CCell::SetAttributeVisable(bool visable)
{
	if (!unitState.lblAttribute)
		return;
	unitState.attributeBackground->setVisible(visable);
	unitState.lblAttribute->setVisible(visable);
}

void CCell::SetAttributePosition(Point position)
{
	if (!unitState.lblAttribute)
		return;
	unitState.attributeBackground->setPosition(position);
	position += Point(_attributeBorderWidth, _attributeBorderWidth);
	unitState.lblAttribute->setPosition(position);
}

void CCell::SetCamp(UnitCamp campType)
{
	if (!unitState.camp)
	{
		unitState.camp = Sprite::create();
		unitState.camp->setAnchorPoint(Point(1, 1));
		unitState.camp->setPosition(unit->getPosition() + Point(75.F / 2 - 1, 75.F / 2 - 1));
	}
	if (campType == UC_YOURSELF)
		unitState.camp->setTexture("CampBlue.png");
	else if (campType == UC_ENEMY)
		unitState.camp->setTexture("CampRed.png");
}

void CCell::SetCampVisable(bool visable)
{
	unitState.camp->setVisible(visable);
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
	unitState.attributeBackground = NULL;
}

CCell::~CCell()
{
}
