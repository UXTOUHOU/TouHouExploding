#pragma once

#include "CSceneMenu.h"
#include "cocostudio\CocoStudio.h"

class CLayerMenu : public Layer
{
public:
	virtual SceneType Enter() = 0;
	virtual void Leave();
};