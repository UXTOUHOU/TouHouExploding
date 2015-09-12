#include "CSceneMenu.h"
#include "cocostudio/CocoStudio.h"
#include "ui/CocosGUI.h"
#include "CTutorialMode.h"
#include "CPVPMode.h"
#include "CCard.h"

#include "MainMenu.h"
#include "Config.h"
#include "GameHall.h"
#include "CardsGallery.h"
#include "CardDetail.h"
#include "Staff.h"
#include "Tutorial.h"

USING_NS_CC;

using namespace cocostudio::timeline;
CLayerMenu *CSceneMenu::currentScene = NULL;
CSceneMenu *CSceneMenu::pSceneMenu = NULL;

//Scene* CSceneMenu::createScene()
//{
//	//// 'scene' is an autorelease object
//	//auto scene = CSceneMenu::create();
//	//
//	//// 'layer' is an autorelease object
//	//auto layer = Layer::create();
//
//	//// add layer as a child to scene
//	//scene->addChild(layer);
//
//	//// return the scene
//	//return scene;
//}

// on "init" you need to initialize your instance
bool CSceneMenu::init()
{
	CMainMenu::getInstance()->Enter();

	addChild(CMainMenu::getInstance());
	addChild(CGameHall::getInstance());
	addChild(CCardsGallery::getInstance());
	addChild(CCardDetail::getInstance());
	addChild(CConfig::getInstance());
	addChild(CStaff::getInstance());
	addChild(CTutorial::getInstance());

	//esc键
	auto listener = EventListenerKeyboard::create();
	listener->onKeyReleased = [this](EventKeyboard::KeyCode keyCode, Event *event)
	{
		//按下了ESC
		if (keyCode == EventKeyboard::KeyCode::KEY_ESCAPE)
		{
			currentScene->OnButtonReturn();
			//switch (currentScene)
			//{
			//case sceneMainMenu:
			//	exit(0);
			//	break;
			//case sceneConfig:
			//	OnButtonConfigReturn();
			//	break;
			//case sceneGallery:
			//	OnButtonGalleryReturn();
			//	break;
			//case sceneGameHall:
			//	OnButtonGameHallReturn();
			//	break;
			//case sceneTutorial:
			//	OnButtonTutorialReturn();
			//	break;
			//case sceneStaff:
			//	OnButtonStaffReturn();
			//	break;
			//}
			log("%s", "esc");
		}
	};
	_eventDispatcher->addEventListenerWithSceneGraphPriority(listener, this);
	//
	//鼠标事件
	auto listenerMouse = EventListenerMouse::create();
	//鼠标点击
	listenerMouse->onMouseDown = [this](Event *event)
	{
		//EventMouse *eventMouse = ;

		currentScene->OnMouseDown((EventMouse *)event);
		//if (sceneGameHall == currentScene)
		//{
		//	log("MouseDown");
		//}else if (sceneCardDetail == currentScene){
		//	/////
		//}
	};
	//鼠标滚轮
	listenerMouse->onMouseScroll = [this](Event *event)
	{
		currentScene->OnMouseScroll((EventMouse *)event);
		//switch (currentScene)
		//{
		//case sceneGameHall:
		//{
		//}
		//	break;
		//case sceneTutorial:
		//{
		//	log("%s %f", "MouseScroll", eventMouse->getScrollY());
		//}
		//	break;
		//default:
		//	break;
		//}
	};
	//鼠标移动
	listenerMouse->onMouseMove = [](Event *event)
	{
		currentScene->OnMouseMove((EventMouse *)event);
		//log("%s %f %f", "MouseMove", eventMouse->getCursorX(), eventMouse->getCursorY());
	};
	//currentScene
	currentScene = CMainMenu::getInstance();
	//test
	for (int i = 0; i < 100; ++i)
	{
		char num[10];
		CGameHall::getInstance()->AddRoomList(std::string(_itoa(i, num, 10)), "RoomName", "PlayerName");
	}
	//
	_eventDispatcher->addEventListenerWithSceneGraphPriority(listenerMouse, this);
	return true;
}


//
//void CSceneMenu::OnButtonConfigReturn()
//{
//	//返回主菜单
//	if (currentScene != sceneConfig)
//		return;
//	layerMainMenu->setVisible(true);
//	layerConfig->setVisible(false);
//	currentScene = sceneMainMenu;
//}
//
//void CSceneMenu::OnButtonConfigTutorial()
//{
//	////从设置界面进入教程界面
//	if (currentScene != sceneConfig)
//		return;
//	CTutorialMode *tutorial = CTutorialMode::create();
//	tutorial->startTutorial();
//	//layerTutorial->setVisible(true);
//	//layerConfig->setVisible(false);
//	currentScene = sceneTutorial;
//}

//void CSceneMenu::OnButtonConfigStaff()
//{
//	//从设置界面进入Staff界面
//	if (currentScene != sceneConfig)
//		return;
//	layerConfig->setVisible(false);
//	layerStaff->setVisible(true);
//	currentScene = sceneStaff;
//}

//void CSceneMenu::OnButtonTutorialReturn()
//{
//	//从教程界面返回设置界面
//	if (currentScene != sceneTutorial)
//		return;
//	scrollViewTutorial->jumpToTop();
//
//	layerTutorial->setVisible(false);
//	layerConfig->setVisible(true);
//	currentScene = sceneConfig;
//}
//
//void CSceneMenu::OnButtonStaffReturn()
//{
//	//从Staff界面返回设置界面
//	if (currentScene != sceneStaff)
//		return;
//	layerStaff->setVisible(false);
//	layerConfig->setVisible(true);
//	currentScene = sceneConfig;
//}
//
//
//
//void CSceneMenu::ReturnCardGallery()
//{
//	layerCardDetail->setVisible(false);
//	layerCardsGallery->setVisible(true);
//	currentScene = sceneGallery;
//}
//
