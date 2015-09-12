#pragma once

#include "CSceneMenu.h"
#include "cocostudio\CocoStudio.h"

class CLayerMenu : public Layer
{
public:
	virtual void Enter() = 0;
	virtual void Leave();
	virtual void OnButtonReturn() = 0;
	virtual void OnMouseMove(EventMouse *eventMouse);
	virtual void OnMouseScroll(EventMouse *eventMouse);
	virtual void OnMouseDown(EventMouse *eventMouse);
	virtual void OnMouseUp(EventMouse *eventMouse);
};