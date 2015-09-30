#pragma once
#include "cocos2d.h"
#include "ui\CocosGUI.h"
#include "cocostudio/CocoStudio.h"
#include "ToolFunc.h"
#include "CardAttribute.h"
USING_NS_CC;

float GetCardViewWidth(Sprite *sprite);
float GetCardViewHeight(Sprite *sprite);
Rect GetCardViewRect(Sprite *sprite,Point father);	//father是父容器的左下点在窗口中的坐标

class CUnit;

class CCard : public Sprite, public CCardAttribute
{
public:
	//int ID;				//卡片编号

	//int hp;				//HP
	//int motility;		//机动性
	//int atk;			//攻击力
	//int minAtkRange;	//攻击范围最小值
	//int maxAtkRange;	//攻击范围最大值

	//std::string skill_1_Describe;
	//std::string skill_2_Describe;
	//std::string skill_3_Describe;

	//Sprite *cardSprite;
	//CUnit *cardUnit;

	static CCard *create(int cardID);
	void ShowCard(Rect rect, Node *father);
	void ShowCard(Point position, float scale, Node *father);
	void DelHandCards();

	CCard();
	~CCard();
};

//Sprite *CreateCardSprite(int cardID);