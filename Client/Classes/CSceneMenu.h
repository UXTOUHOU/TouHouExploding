#pragma once

#include "cocos2d.h"
#include "cocos-ext.h"
#include "ui\CocosGUI.h"
#include "cocostudio\CocoStudio.h"
#include "LayerMenu.h"

USING_NS_CC_EXT;

enum SceneType{
	sceneMainMenu,
	sceneGameHall,
	sceneGallery,
	sceneConfig,
	sceneTutorial,
	sceneStaff,
	sceneCardDetail,
};
class CLayerMenu;
class CSceneMenu : public Scene
{
public:
	static CLayerMenu *currentScene;
	static CSceneMenu *pSceneMenu;
	static CSceneMenu *getInstance()
	{
		if (pSceneMenu == NULL)
			pSceneMenu = CSceneMenu::create();
		return pSceneMenu;
	}

	virtual bool init() override;

	CREATE_FUNC(CSceneMenu);
};