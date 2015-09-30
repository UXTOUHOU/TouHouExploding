#include "Card.h"
//#include "SceneBattle.h"

//void CardSprite::ShowCard(Point point)
//{
//	sprite->setPosition(point);
//}

CCard::CCard()
{
}

CCard::~CCard()
{
}


float GetCardViewWidth(Sprite *sprite)
{
	float scale = sprite->getScale();
	float width = sprite->getTexture()->getPixelsWide();
	return width*scale;
}

float GetCardViewHeight(Sprite *sprite)
{
	float scale = sprite->getScale();
	float height = sprite->getTexture()->getPixelsHigh();
	return height*scale;
}

Rect GetCardViewRect(Sprite *sprite, Point father)
{
	float scale = sprite->getScale();
	Rect rect = sprite->getTextureRect();
	rect.size = rect.size * scale;
	rect.origin = father + sprite->getPosition();
	rect.origin.x -= sprite->getAnchorPoint().x * GetCardViewWidth(sprite);
	rect.origin.y -= sprite->getAnchorPoint().y * GetCardViewHeight(sprite);
	return rect;
}

CCard *CCard::create(int cardID)
{
	CCard *card = new CCard;
	card->Sprite::initWithFile(GetCardFileName(cardID));
	//card->cardSprite = Sprite::create(GetCardFileName(fileName, cardID));

	card->ID = cardID;
	switch (cardID)
	{
	case 1:	//博丽灵梦
		card->hp = 9;
		card->atk = 2;
		card->motility = 2;
		card->minAtkRange = 1;
		card->maxAtkRange = 3;

		card->vecSkillAttribute.push_back(L"梦想封印:选定周围范围2内一个单位，驱散并沉默该单位，你可以额外支付1b对该单位造成2伤害。");
		card->vecSkillAttribute.push_back(L"弹幕结界:被动，非己方回合内，灵梦及其周围范围2内的所有单位受到的非近程伤害减1。");

		//card->skill_1_Describe = "梦想封印:选定周围范围2内一个单位，驱散并沉默该单位，你可以额外支付1b对该单位造成2伤害。";
		//card->skill_2_Describe = "弹幕结界:被动，非己方回合内，灵梦及其周围范围2内的所有单位受到的非近程伤害减1。";
		break;
	default:
		break;
	}

	return card;
}

void CCard::ShowCard(Rect rect, Node *father)
{
	Rect rectCard = getTextureRect();
	float scale = MIN(rect.size.width / rectCard.size.width, rect.size.height / rectCard.size.height);
	ShowCard(rectCard.origin, scale, father);
}

void CCard::ShowCard(Point position, float scale, Node *father)
{
	setPosition(position);
	setScale(scale);
	father->addChild(this);
}

void CCard::DelHandCards()
{

}


//Sprite *CreateCardSprite(int cardID)
//{
//	char buf[20];
//	return Sprite::create(GetCardFileName(buf, cardID));
//}

//void Card::SetCardSize(Size size)
//{
//
//}
//
//void SetCardPosition(Point position)
//{
//
//}
