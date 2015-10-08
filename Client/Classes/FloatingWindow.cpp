#include "FloatingWindow.h"

CFloatingWindow::CFloatingWindow(Node *father, int zOrder) :
	_borderWidth(5)
{
	if (father == NULL)
	{
		assert(false);
		return;
	}
	_label = Label::create();
	_drawNodeBackground = DrawNode::create();
	_label->setSystemFontSize(24);
	_label->setAnchorPoint(Point(0, 0));
	_drawNodeBackground->setAnchorPoint(Point(0, 0));
	_label->setVisible(false);
	_drawNodeBackground->setVisible(false);

	father->addChild(_drawNodeBackground, zOrder);
	_drawNodeBackground->addChild(_label);
}

void CFloatingWindow::SetString(string str)
{
	_label->setString(str);
	_label->setPosition(Point(_borderWidth, _borderWidth));
	_drawNodeBackground->setAnchorPoint(Point(0, 0));

	auto position = _label->getPosition();
	auto size = _label->getContentSize();
	Size border(_borderWidth, _borderWidth);

	_drawNodeBackground->clear();
	Vec2 verts[4] = { position, position, position, position };
	verts[0] -= border;
	verts[1] += Point(-border.width, size.height + border.height);
	position += border;
	verts[2] += size + border;
	verts[3] += Point(size.width + border.width, -border.height);
	_drawNodeBackground->drawPolygon(verts, 4, Color4F(0, 0, 0, 0.7F), 0, Color4F(0, 0, 0, 0));
	_drawNodeBackground->setOpacity(50);
}

void CFloatingWindow::SetVisible(bool visible)
{
	_label->setVisible(visible);
	_drawNodeBackground->setVisible(visible);
}

void CFloatingWindow::SetPosition(Point point)
{
	_drawNodeBackground->setPosition(point);
}