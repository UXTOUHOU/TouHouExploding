#include "GameHall.h"
#include "MainMenu.h"
#include "PVPMode.h"

CGameHall *CGameHall::pGameHall = NULL;

bool CGameHall::init()
{
	Layer::init();
	auto gameHallMenuNode = CSLoader::createNode("GameHall.csb");
	addChild(gameHallMenuNode);
	setVisible(false);

	auto buttonGameGallJoinGame = gameHallMenuNode->getChildByName<ui::Button *>("Button_JoinGame");
	buttonGameGallJoinGame->addClickEventListener(CC_CALLBACK_0(CGameHall::OnButtonJoinGame, this));
	auto buttonGameGallReturn = gameHallMenuNode->getChildByName<ui::Button *>("Button_ReturnMainMenu");
	buttonGameGallReturn->addClickEventListener(CC_CALLBACK_0(CGameHall::OnButtonReturn, this));

	listViewRoomList = gameHallMenuNode->getChildByName<ui::ListView *>("ListView_Room");
	selectedRoom = NULL;
	return true;
}

CGameHall::CGameHall()
{
}


CGameHall::~CGameHall()
{
}

void CGameHall::Enter()
{
	setVisible(true);
	CSceneMenu::currentScene = getInstance();
}

void CGameHall::OnButtonReturn()
{
	//返回主菜单
	if (CSceneMenu::currentScene != getInstance())
		return;
	Leave();
	CMainMenu::getInstance()->Enter();
}

void CGameHall::OnButtonJoinGame()
{
	//测试 进入游戏场景
	if (CSceneMenu::currentScene != getInstance())
		return;
	auto director = Director::getInstance();
	CPVPMode *scene = CPVPMode::create();
	director->pushScene(scene);
	//
	return;

	//if (selectedRoom == NULL)
	//	return;
	//join selectedRoom
}

void CGameHall::OnMouseDown(EventMouse *eventMouse)
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
				EffectSelectRoom((ui::Layout *)it);
				//it->removeFromParent();// Color(Color3B::BLACK);
				return;
			}
		}
		//listViewRoomList->setFocused(true);
	}
}

void CGameHall::OnMouseScroll(EventMouse *eventMouse)
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

void CGameHall::EffectSelectRoom(ui::Layout *room)
{
	if (selectedRoom != NULL)
		selectedRoom->setBackGroundColorOpacity(0);
	selectedRoom = room;
	room->setBackGroundColor(Color3B(255, 255, 255));
	room->setBackGroundColorOpacity(100);
	room->setBackGroundColorType(ui::Layout::BackGroundColorType::SOLID);
}

void CGameHall::AddRoomList(std::string ID, std::string roomName, std::string playerName)
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