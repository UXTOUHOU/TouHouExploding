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

USING_NS_CC;

using namespace cocostudio::timeline;
CSceneMenu *CSceneMenu::pSceneMenu = NULL;
CLayerMenu *CSceneMenu::currentScene = CMainMenu::getInstance();

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
	////mainMenu
	addChild(CMainMenu::getInstance());
	//

	//gameHallMenu
	addChild(CGameHall::getInstance());

	//cardsGallery
	addChild(CCardsGallery::getInstance());

	addChild(CCardDetail::getInstance());
	//

	//configMenu
	addChild(CConfig::getInstance());
	//

	////Tutorial
	//addChild(layerTutorial);
	////
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
	//staff
	addChild(layerStaff);
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
		}else if (sceneCardDetail == currentScene){
			/////
			Point point = eventMouse->getLocationInView();

			point -= eventMouse->getLocationInView();
			auto &vecCards = scrollViewCardGallery->getChildren();
			for (auto itCard : vecCards)
			{
				CCard *card = (CCard *)itCard;
				Rect rect;
				rect.origin = card->getPosition();
				rect.size = card->getContentSize();
				if (rect.containsPoint(point))
				{
					ShowCardDetail(card->ID);
				}
			}
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

void CSceneMenu::ShowCardDetail(int cardID)
{
	auto card = layerCardDetail->getChildByName<Sprite *>("Sprite_Card");
	char fileName[100];
	card->setTexture(GetCardFileName(fileName, cardID));
	layerCardDetail->setVisible(true);
	currentScene = sceneCardDetail;
}

void CSceneMenu::ReturnCardGallery()
{
	layerCardDetail->setVisible(false);
	layerCardsGallery->setVisible(true);
	currentScene = sceneGallery;
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