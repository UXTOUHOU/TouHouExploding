#pragma once

#include "cocos2d.h"
#include <string>
USING_NS_CC;
using namespace std;

class CFloatingWindow
{
public:
	CFloatingWindow(Node *father, int zOrder);
	void SetBorderWidth();
	void SetString(string str);
	void SetVisible(bool visible);
	void SetPosition(Point point);
private:
	Label *_label;
	DrawNode *_drawNodeBackground;

	int _borderWidth;
};