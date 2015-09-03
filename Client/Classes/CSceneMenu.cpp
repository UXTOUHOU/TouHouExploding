#include "CSceneMenu.h"
#include "cocostudio/CocoStudio.h"
#include "ui/CocosGUI.h"
#include "CTutorialMode.h"
#include "CPVPMode.h"

USING_NS_CC;

using namespace cocostudio::timeline;
CSceneMenu *pSceneMenu = NULL;

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
	//mainMenu
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

	layerMainMenu = Layer::create();
	layerMainMenu->addChild(nodeMainMenu);
	addChild(layerMainMenu);
	//

	//gameHallMenu
	selectedRoom = NULL;
	auto gameHallMenuNode = CSLoader::createNode("GameHall.csb");
	layerGameHall = Layer::create();
	layerGameHall->addChild(gameHallMenuNode);
	addChild(layerGameHall);
	layerGameHall->setVisible(false);

	auto buttonGameGallJoinGame = gameHallMenuNode->getChildByName<ui::Button *>("Button_JoinGame");
	buttonGameGallJoinGame->addClickEventListener(CC_CALLBACK_0(CSceneMenu::OnButtonGameHallJoinGame, this));
	auto buttonGameGallReturn = gameHallMenuNode->getChildByName<ui::Button *>("Button_ReturnMainMenu");
	buttonGameGallReturn->addClickEventListener(CC_CALLBACK_0(CSceneMenu::OnButtonGameHallReturn, this));
	
	listViewRoomList = gameHallMenuNode->getChildByName<ui::ListView *>("ListView_RoomList");

	//cardsGallery
	auto cardsGalleryMenuNode = CSLoader::createNode("CardsGallery.csb");
	layerCardsGallery = Layer::create();
	layerCardsGallery->addChild(cardsGalleryMenuNode);
	addChild(layerCardsGallery);
	layerCardsGallery->setVisible(false);

	auto buttonGalleryReturn = cardsGalleryMenuNode->getChildByName<ui::Button *>("Button_ReturnMainMenu");
	buttonGalleryReturn->addClickEventListener(CC_CALLBACK_0(CSceneMenu::OnButtonGalleryReturn, this));
	//

	//configMenu
	auto configMenuNode = CSLoader::createNode("Config.csb");
	layerConfig = Layer::create();
	layerConfig->addChild(configMenuNode);
	addChild(layerConfig);
	layerConfig->setVisible(false);

	auto buttonConfigReturn = configMenuNode->getChildByName<ui::Button *>("Button_ReturnMainMenu");
	buttonConfigReturn->addClickEventListener(CC_CALLBACK_0(CSceneMenu::OnButtonConfigReturn, this));
	auto buttonTutorial = configMenuNode->getChildByName<ui::Button *>("Button_Tutorial");
	buttonTutorial->addClickEventListener(CC_CALLBACK_0(CSceneMenu::OnButtonConfigTutorial, this));
	auto buttonStaff = configMenuNode->getChildByName<ui::Button *>("Button_Staff");
	buttonStaff->addClickEventListener(CC_CALLBACK_0(CSceneMenu::OnButtonConfigStaff, this));
	//

	//Tutorial
	auto tutorialNode = CSLoader::createNode("Tutorial.csb");
	layerTutorial = Layer::create();
	layerTutorial->addChild(tutorialNode);
	addChild(layerTutorial);
	layerTutorial->setVisible(false);

	auto buttonTutorialReturn = tutorialNode->getChildByName<ui::Button *>("Button_ReturnConfig");
	buttonTutorialReturn->addClickEventListener(CC_CALLBACK_0(CSceneMenu::OnButtonTutorialReturn, this));
	scrollViewTutorial = tutorialNode->getChildByName<ui::ScrollView *>("ScrollView_Tutorial");
	//
	//esc键
	auto listener = EventListenerKeyboard::create();
	listener->onKeyReleased = [this](EventKeyboard::KeyCode keyCode, Event *event)
	{
		//按下了ESC
		if (keyCode == EventKeyboard::KeyCode::KEY_ESCAPE)
		{
			switch (currentScene)
			{
			case sceneMainMenu:
				exit(0);
				break;
			case sceneConfig:
				OnButtonConfigReturn();
				break;
			case sceneGallery:
				OnButtonGalleryReturn();
				break;
			case sceneGameHall:
				OnButtonGameHallReturn();
				break;
			case sceneTutorial:
				OnButtonTutorialReturn();
				break;
			case sceneStaff:
				OnButtonStaffReturn();
				break;
			}
			log("%s", "esc");
		}
	};
	_eventDispatcher->addEventListenerWithSceneGraphPriority(listener, this);
	//
	//staff
	auto staffNode = CSLoader::createNode("Staff.csb");
	layerStaff = Layer::create();
	layerStaff->addChild(staffNode);
	addChild(layerStaff);
	layerStaff->setVisible(false);

	auto buttonStaffReturn = staffNode->getChildByName<ui::Button *>("Button_ReturnConfig");
	buttonStaffReturn->addClickEventListener(CC_CALLBACK_0(CSceneMenu::OnButtonStaffReturn, this));
	//
	//鼠标事件
	auto listenerMouse = EventListenerMouse::create();
	//鼠标点击
	listenerMouse->onMouseDown = [this](Event *event)
	{
		EventMouse *eventMouse = (EventMouse *)event;

		if (sceneGameHall == currentScene)
		{
			Point point = eventMouse->getLocationInView();
			Rect rect;
			rect.origin = listViewRoomList->getPosition();
			rect.size = listViewRoomList->getCustomSize();
			if (rect.containsPoint(point))
			{
				auto vec = listViewRoomList->getChildren();
				for (auto it : vec)
				{
					Point t = point - listViewRoomList->getPosition();
					Rect rect2;
					rect2.origin = it->getPosition() + listViewRoomList->getInnerContainer()->getPosition();
					rect2.size = it->getContentSize();
					if (rect2.containsPoint(t))
					{
						SelectRoom((ui::Layout *)it);
						//it->removeFromParent();// Color(Color3B::BLACK);
						return;
					}
				}
				//listViewRoomList->setFocused(true);
			}
			log("MouseDown");
		}
	};
	//鼠标滚轮
	listenerMouse->onMouseScroll = [this](Event *event)
	{
		EventMouse *eventMouse = (EventMouse *)event;
		switch (currentScene)
		{
		case sceneGameHall:
		{
			auto layout = listViewRoomList->getInnerContainer();
			float listViewHeight = listViewRoomList->getCustomSize().height;
			float position = layout->getPosition().y;
			float height = layout->getCustomSize().height;
			float currentPercent;
			if (listViewHeight >= height)
				return;//currentPercent = 0.F;
			else
				currentPercent = 100.F - (100 * abs(position) / (height - listViewHeight));
			currentPercent += 100 * (eventMouse->getScrollY() * 40 * 8.0) / (height - listViewHeight);
			if (currentPercent < 0)
				currentPercent = 0;
			else if (currentPercent > 100)
				currentPercent = 100;
			listViewRoomList->scrollToPercentVertical(currentPercent, 0.2F, false);
		}
			break;
		case sceneTutorial:
		{
			static float currentPercent = 0;
			currentPercent += eventMouse->getScrollY() * 3.0 / 20 * 100;
			if (currentPercent < 0)
				currentPercent = 0;
			else if (currentPercent > 100)
				currentPercent = 100;
			scrollViewTutorial->scrollToPercentVertical(currentPercent, 0.1F, false);
			log("%s %f", "MouseScroll", eventMouse->getScrollY());
		}
			break;
		default:
			break;
		}
	};
	//鼠标移动
	listenerMouse->onMouseMove = [](Event *event)
	{
		EventMouse *eventMouse = (EventMouse *)event;
		log("%s %f %f", "MouseMove", eventMouse->getCursorX(), eventMouse->getCursorY());
	};
	//currentScene
	currentScene = sceneMainMenu;
	//test
	for (int i = 0; i < 100; ++i)
	{
		char num[10];
		AddRoomList(std::string(_itoa(i, num, 10)), "RoomName", "PlayerName");
	}
	//
	_eventDispatcher->addEventListenerWithSceneGraphPriority(listenerMouse, this);
	return true;
}

void CSceneMenu::OnButtonConfig()
{
	//进入设置菜单
	if (currentScene != sceneMainMenu)
		return;
	layerMainMenu->setVisible(false);
	layerConfig->setVisible(true);
	currentScene = sceneConfig;
}

void CSceneMenu::OnButtonStart()
{
	//进入游戏大厅
	if (currentScene != sceneMainMenu)
		return;
	layerMainMenu->setVisible(false);
	layerGameHall->setVisible(true);
	currentScene = sceneGameHall;

	///ConnectToServer
	///GetRoomList
	///ShowRoomList
}

void CSceneMenu::OnButtonExit()
{
	//退出游戏
	if (currentScene != sceneMainMenu)
		return;
	exit(0);
}

void CSceneMenu::OnButtonGallery()
{
	//进入卡牌浏览
	if (currentScene != sceneMainMenu)
		return;
	layerMainMenu->setVisible(false);
	layerCardsGallery->setVisible(true);
	currentScene = sceneGallery;
}

void CSceneMenu::OnButtonGalleryReturn()
{
	//返回主菜单
	if (currentScene != sceneGallery)
		return;
	layerCardsGallery->setVisible(false);
	layerMainMenu->setVisible(true);
	currentScene = sceneMainMenu;
}

void CSceneMenu::OnButtonGameHallJoinGame()
{
	//测试 进入游戏场景
	if (currentScene != sceneGameHall)
		return;

	auto director = Director::getInstance();
	//Scene *test = CSceneBattle::createScene();
	CPVPMode *scene = CPVPMode::create();
	director->replaceScene(scene);

	//
	return;
	if (NULL == selectedRoom)
		return;
	//join selectedRoom
}

void CSceneMenu::OnButtonGameHallReturn()
{
	//返回主菜单
	if (currentScene != sceneGameHall)
		return;
	layerGameHall->setVisible(false);
	layerMainMenu->setVisible(true);
	currentScene = sceneMainMenu;
}

void CSceneMenu::OnButtonConfigReturn()
{
	//返回主菜单
	if (currentScene != sceneConfig)
		return;
	layerMainMenu->setVisible(true);
	layerConfig->setVisible(false);
	currentScene = sceneMainMenu;
}

void CSceneMenu::OnButtonConfigTutorial()
{
	////从设置界面进入教程界面
	if (currentScene != sceneConfig)
		return;
	CTutorialMode *tutorial = CTutorialMode::create();
	tutorial->startTutorial();
	//layerTutorial->setVisible(true);
	//layerConfig->setVisible(false);
	currentScene = sceneTutorial;
}

void CSceneMenu::OnButtonConfigStaff()
{
	//从设置界面进入Staff界面
	if (currentScene != sceneConfig)
		return;
	layerConfig->setVisible(false);
	layerStaff->setVisible(true);
	currentScene = sceneStaff;
}

void CSceneMenu::OnButtonTutorialReturn()
{
	//从教程界面返回设置界面
	if (currentScene != sceneTutorial)
		return;
	scrollViewTutorial->jumpToTop();

	layerTutorial->setVisible(false);
	layerConfig->setVisible(true);
	currentScene = sceneConfig;
}

void CSceneMenu::OnButtonStaffReturn()
{
	//从Staff界面返回设置界面
	if (currentScene != sceneStaff)
		return;
	layerStaff->setVisible(false);
	layerConfig->setVisible(true);
	currentScene = sceneConfig;
}

void CSceneMenu::AddRoomList(std::string ID, std::string roomName, std::string playerName)
{
	auto layout = ui::Layout::create();
	layout->setContentSize(Size(1300.F, 40.F));
	auto textRoomID = ui::Text::create();
	textRoomID->setFontSize(32);
	textRoomID->setPosition(Point(15, 20));
	textRoomID->setAnchorPoint(Point(0, 0.5F));
	textRoomID->setString(ID);
	auto textRoomName = ui::Text::create();
	textRoomName->setFontSize(32);
	textRoomName->setPosition(Point(160, 20));
	textRoomName->setAnchorPoint(Point(0, 0.5F));
	textRoomName->setString(roomName);
	auto textPlayerName = ui::Text::create();
	textPlayerName->setFontSize(32);
	textPlayerName->setPosition(Point(780, 20));
	textPlayerName->setAnchorPoint(Point(0, 0.5F));
	textPlayerName->setString(playerName);
	layout->addChild(textRoomID);
	layout->addChild(textRoomName);
	layout->addChild(textPlayerName);
	listViewRoomList->addChild(layout);
}

void CSceneMenu::SelectRoom(ui::Layout *room)
{
	if (selectedRoom != NULL)
		selectedRoom->setBackGroundColorOpacity(0);
	selectedRoom = room;
	room->setBackGroundColor(Color3B(255, 255, 255));
	room->setBackGroundColorOpacity(100);
	room->setBackGroundColorType(ui::Layout::BackGroundColorType::SOLID);
}