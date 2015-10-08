#include "Unit.h"

//char *GetUnitFileName(int unitID)
//{
//	static char buf[20];
//	//返回 Unit_ID.png 格式的图片名
//	strcpy(buf, "Unit_");
//	char strID[10];
//	_itoa(unitID, strID, 10);
//	strcat(buf, strID);
//	strcat(buf, ".png");
//	return buf;
//}
bool CUnit::init()
{
	return true;
}


CUnit::CUnit() :
	_lblHP(NULL),
	_groupType(UG_YOURSELF),
	_group(NULL),
	_lblAttribute(NULL),
	_attributeBackground(NULL),
	_moveable(false),
	_canAttack(false)
{
}

CUnit::~CUnit()
{

}

void CUnit::InitCardAttribute(int unitID)
{
	CCardAttribute::InitMapCardAttribute(unitID);
	auto it = mapCardAttribute.find(unitID);
	if (it == mapCardAttribute.end()) return;// assert(false);
	auto &cardAttributee = it->second;
	hp = cardAttributee->hp;
	atk = cardAttributee->atk;
	motility = cardAttributee->motility;
	minAtkRange = cardAttributee->minAtkRange;
	maxAtkRange = cardAttributee->maxAtkRange;

	vecSkillAttribute = cardAttributee->vecSkillAttribute;
}

void CUnit::UnitDeath()
{
	removeFromParentAndCleanup(true);
	if (_lblHP)
		_lblHP->removeFromParentAndCleanup(true);
	if (_group)
		_group->removeFromParentAndCleanup(true);
	if (_lblAttribute)
		_lblAttribute->removeFromParentAndCleanup(true);
	if (_attributeBackground)
		_attributeBackground->removeFromParentAndCleanup(true);
}

void CUnit::SetChessboardPosition(ChessboardPosition position)
{
	_chessboardPosition = position;
}

ChessboardPosition CUnit::GetChessboardPosition() const
{
	return _chessboardPosition;
}

void CUnit::SetMoveable(bool moveabel)
{
	_moveable = moveabel;
}

bool CUnit::GetMoveable() const
{
	return _moveable;
}

void CUnit::SetCanAttack(bool canAttack)
{
	_canAttack = canAttack;
}

bool CUnit::GetCanAttack() const
{
	return _canAttack;
}

UnitGroup CUnit::GetGroupType() const
{
	return _groupType;
}

Sprite *CUnit::GetSpriteGroup() const
{
	return _group;
}

DrawNode *CUnit::GetAttributeBackground() const
{
	return _attributeBackground;
}

Label *CUnit::GetLabelAttribute() const
{
	return _lblAttribute;
}

int CUnit::GetSkillNum() const
{
	return vecSkillAttribute.size();
}

Label *CUnit::GetLabelHP() const
{
	return _lblHP;
}

void CUnit::SetHP(int HP)
{
	_HP = HP;
	if (!_lblHP)
	{
		_lblHP = Label::create();
		_lblHP->setSystemFontSize(12);
		_lblHP->setScale(1.33F);
		_lblHP->setAnchorPoint(Point(0, 1));
	}
	std::wstring wHP;
	std::wstringstream wss;
	wss << _HP;
	wss >> wHP;
	_lblHP->setString(WStrToUTF8(wHP));
}

int CUnit::GetHP() const
{
	return _HP;
}

void CUnit::ChangeHP(int deltaHP)
{
	std::wstring wHP;
	std::wstringstream wss;
	_HP += deltaHP;
	//if (_HP <= 0)
	//{
	//	UnitDeath();
	//}
	wss << _HP;
	wss >> wHP;
	_lblHP->setString(WStrToUTF8(wHP));
}

void CUnit::SetHPVisible(bool visible)
{
	if (_lblHP)
		_lblHP->setVisible(visible);
}

void CUnit::SetHPPosition(ChessboardPosition position)
{
	if (_lblHP)
		_lblHP->setPosition(position.CastPoint() + Point(-75.F / 2 + 3, 75.F / 2 - 1));
}

void CUnit::SetAttribute(std::string attribute)
{
	if (!_lblAttribute)
	{
		_lblAttribute = Label::create();
		_lblAttribute->setSystemFontSize(24);
		_lblAttribute->setAnchorPoint(Point(0, 0));

		_attributeBackground = DrawNode::create();
		_attributeBackground->setAnchorPoint(Point(0, 0));
	}
	_lblAttribute->setString(attribute);
	_lblAttribute->setVisible(false);

	auto position = _lblAttribute->getPosition();
	auto size = _lblAttribute->getContentSize();
	Size border(_attributeBorderWidth, _attributeBorderWidth);
	position += border;

	Vec2 verts[4] = { position, position, position, position };
	verts[0] -= border;
	verts[1] += Point(-border.width, size.height + border.height);
	verts[2] += size + border;
	verts[3] += Point(size.width + border.width, -border.height);
	_attributeBackground->drawPolygon(verts, 4, Color4F(0, 0, 0, 0.7F), 0, Color4F(0, 0, 0, 0));
	_attributeBackground->setOpacity(50);
	_attributeBackground->setVisible(false);
}

void CUnit::SetAttributeVisible(bool visible)
{
	if (!_lblAttribute)
		return;
	_attributeBackground->setVisible(visible);
	_lblAttribute->setVisible(visible);
}

void CUnit::SetAttributePosition(Point position)
{
	if (!_lblAttribute)
		return;
	_attributeBackground->setPosition(position);
	position += Point(_attributeBorderWidth, _attributeBorderWidth);
	_lblAttribute->setPosition(position);
}

void CUnit::SetGroup(UnitGroup campType)
{
	_groupType = campType;
	if (!_group)
	{
		_group = Sprite::create();
	}
	if (campType == UG_YOURSELF)
		_group->setTexture("GroupBlue.png");
	else if (campType == UG_ENEMY)
		_group->setTexture("GroupRed.png");
	_group->setAnchorPoint(Point(1, 1));
}

void CUnit::SetGroupVisible(bool visible)
{
	if (_group != NULL)
		_group->setVisible(visible);
}

void CUnit::SetGroupPosition(ChessboardPosition position)
{
	if (_group)
	{
		_group->setPosition(position.CastPoint() + Point(75.F / 2 - 1, 75.F / 2 - 1));
	}
}
