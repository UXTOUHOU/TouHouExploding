#include "CCard.h"
#include "CSceneBattle.h"

//void CardSprite::ShowCard(Point point)
//{
//	sprite->setPosition(point);
//}

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
	char fileName[20];
	card->Sprite::initWithFile(GetCardFileName(fileName, cardID));

	card->ID = cardID;

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

char *GetCardFileName(char *fileName, int cardID)
{
	//返回 Card_ID.png 格式的图片名
	strcpy(fileName, "Card_");
	char strID[10];
	_itoa(cardID, strID, 10);
	strcat(fileName, strID);
	strcat(fileName, ".png");
	return fileName;
}


//void Card::SetCardSize(Size size)
//{
//
//}
//
//void SetCardPosition(Point position)
//{
//
//}
