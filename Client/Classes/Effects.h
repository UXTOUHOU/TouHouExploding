#pragma once
#include "cocostudio\CocoStudio.h"

namespace Effects
{
	void Moveable(Sprite *sprite);				//未移动的单位
	void Moved(Sprite *sprite);					//已移动的单位
	void Normal(Sprite *sprite);				//通常状态
	void Blink(Sprite *sprite, Point position);	//闪烁至某格
	void MoveTo(Sprite *sprite, Point position);
}