#include "CardAttribute.h"

map < int, CCardAttribute * > CCardAttribute::mapCardAttribute;

void CCardAttribute::InitMapCardAttribute(int cardID)
{
	if (mapCardAttribute.find(cardID) != mapCardAttribute.end()) return;

	switch (cardID)
	{
	case 1:
	{
		auto attribute = new CCardAttribute;
		attribute->hp = 9;
		attribute->atk = 2;
		attribute->motility = 2;
		attribute->minAtkRange = 1;
		attribute->maxAtkRange = 3;

		attribute->vecSkillAttribute.push_back(L"梦想封印:选定周围范围2内一个单位，驱散并沉默该单位，你可以额外支付1b对该单位造成2伤害。");
		attribute->vecSkillAttribute.push_back(L"弹幕结界:被动，非己方回合内，灵梦及其周围范围2内的所有单位受到的非近程伤害减1。");

		mapCardAttribute.insert(make_pair(cardID, attribute));
	}
		break;
	default:
		break;
	}
}