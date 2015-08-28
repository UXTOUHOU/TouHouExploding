#include "Effects.h"
#include "CLayerChessBoard.h"

void Effects::Moveable(Sprite *sprite)
{
	sprite->stopAllActions();
	auto move1 = MoveBy::create(1, Point(0, 5));
	auto move2 = move1->reverse();
	sprite->runAction(RepeatForever::create(Sequence::create(move1, move2, NULL)));
}

void Effects::Moved(Sprite *sprite)
{
	sprite->stopAllActions();
	sprite->setOpacity(100);
}

void Effects::Normal(Sprite *sprite)
{
	sprite->stopAllActions();
}

void Effects::Blink(Sprite *sprite, Point position)
{
	sprite->setPosition(position);
}

void Effects::MoveTo(Sprite *sprite, Point position)
{
	//untested
	sprite->stopAllActions();
	auto move = MoveTo::create(1, position);
	sprite->runAction(move);
}