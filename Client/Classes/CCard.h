#pragma once
#include "cocos2d.h"
#include "ui\CocosGUI.h"
#include "cocostudio/CocoStudio.h"
USING_NS_CC;

float GetCardViewWidth(Sprite *sprite);
float GetCardViewHeight(Sprite *sprite);
Rect GetCardViewRect(Sprite *sprite,Point father);	//father是父容器的左下点在窗口中的坐标

class CCard : public Sprite
{
public:
	int ID;			//卡片编号

	int hp;			//HP
	int motility;	//机动性
	int atk;		//攻击力
	int atkRange;	//攻击范围

	static CCard *create(int cardID);
	void ShowCard(Rect rect, Node *father);
	void ShowCard(Point position, float scale, Node *father);
	void DelHandCards();
};

char *GetCardFileName(char *fileName, int cardID);
