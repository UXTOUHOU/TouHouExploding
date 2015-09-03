#pragma once

#include "cocos2d.h"
#include "cocos-ext.h"
#include "ui\CocosGUI.h"
#include "cocostudio\CocoStudio.h"

USING_NS_CC_EXT;

class CSceneMenu : public cocos2d::Scene
{
public:
	static CSceneMenu *pSceneMenu;
	static CSceneMenu *getInstance()
	{
		if (pSceneMenu == NULL)
			pSceneMenu = CSceneMenu::create();
		return pSceneMenu;
	}
	// there's no 'id' in cpp, so we recommend returning the class instance pointer
	//static cocos2d::Scene* createScene();

	// Here's a difference. Method 'init' in cocos2d-x returns bool, instead of returning 'id' in cocos2d-iphone
	virtual bool init() override;

	// implement the "static create()" method manually
	CREATE_FUNC(CSceneMenu);

	Layer *layerMainMenu;
	Layer *layerConfig;
	Layer *layerGameHall;
	Layer *layerCardsGallery;
	Layer *layerTutorial;
	Layer *layerStaff;

	ui::ScrollView *scrollViewTutorial;
	
	ui::ListView *listViewRoomList;

	enum {
		sceneMainMenu,
		sceneGameHall,
		sceneGallery,
		sceneConfig,
		sceneTutorial,
		sceneStaff,
	}currentScene;

	//主菜单
	void OnButtonStart();
	void OnButtonGallery();
	void OnButtonConfig();
	void OnButtonExit();
	//游戏大厅
	ui::Layout *selectedRoom;
	void OnButtonGameHallJoinGame();
	void OnButtonGameHallReturn();
	//void CSceneMenu::ButtonCreateGame();
	void SelectRoom(ui::Layout *room);
	//设置
	void OnButtonConfigReturn();
	void OnButtonConfigTutorial();
	void OnButtonConfigStaff();
	//卡牌浏览
	void OnButtonGalleryReturn();
	//教程
	void OnButtonTutorialReturn();
	//职员表
	void OnButtonStaffReturn();
private:
	void AddRoomList(std::string ID,std::string roomName,std::string playerName);
};