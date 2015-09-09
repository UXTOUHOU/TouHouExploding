#include "MainMenu.h"

CMainMenu *CMainMenu::pMainMenu = NULL;

bool CMainMenu::init()
{
	Layer::init();
	//
	auto nodeMainMenu = CSLoader::createNode("MainScene.csb");
	//添加按钮click事件的捕捉
	auto buttonStart = nodeMainMenu->getChildByName<ui::Button *>("Button_StartGame");
	buttonStart->addClickEventListener(CC_CALLBACK_0(CSceneMenu::OnButtonStart, this));
	auto buttonGallery = nodeMainMenu->getChildByName<ui::Button *>("Button_CardsGallery");
	buttonGallery->addClickEventListener(CC_CALLBACK_0(CSceneMenu::OnButtonGallery, this));
	auto buttonConfig = nodeMainMenu->getChildByName<ui::Button *>("Button_Config");
	buttonConfig->addClickEventListener(CC_CALLBACK_0(CSceneMenu::OnButtonConfig, this));
	auto buttonExit = nodeMainMenu->getChildByName<ui::Button *>("Button_Exit");
	buttonExit->addClickEventListener(CC_CALLBACK_0(CSceneMenu::OnButtonExit, this));

	addChild(nodeMainMenu);
	//
	setVisible(false);
}

SceneType CMainMenu::Enter()
{
	setVisible(true);
	return sceneMainMenu;
}