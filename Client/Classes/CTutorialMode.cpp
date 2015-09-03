#include "CTutorialMode.h"

bool CTutorialMode::init()

{
	CSceneBattle::init();
	layerDilogue = Layer::create();
	layerDilogue->setVisible(false);
	addChild(layerDilogue);
	//
	layerDilogue->setVisible(true);

	auto layout = ui::Layout::create();
	layout->setAnchorPoint(Point(0, 0));
	layout->setPosition(panelCards->getPosition());
	layout->setContentSize(panelCards->getContentSize());
	layout->setBackGroundColorType(ui::Layout::BackGroundColorType::SOLID);
	layout->setColor(Color3B(67, 67, 67));
	layerDilogue->addChild(layout);
	//
	richTextDialogue = ui::RichText::create();
	richTextDialogue->setAnchorPoint(Point(0, 1));
	richTextDialogue->setPosition(Point(0, 220));//////
	//richTextDialogue->setContentSize(Size(800, 200));
	layout->addChild(richTextDialogue);
	//
	labelDialogue = Label::create();
	labelDialogue->setSystemFontSize(28);
	labelDialogue->setLineBreakWithoutSpace(true);
	labelDialogue->setWidth(800);
	labelDialogue->setAnchorPoint(Point(0, 1));
	labelDialogue->setPosition(Point(40, 200));
	layout->addChild(labelDialogue);
	//
	auto listenerMouse = EventListenerMouse::create();
	listenerMouse->onMouseUp = [](Event *event)
	{
		EventMouse *eventMouse = (EventMouse *)event;
		///ÏÂÒ»¾ä»°
	};

	_eventDispatcher->addEventListenerWithSceneGraphPriority(listenerMouse, layerBattleScene);

	if (gbTest)
	{
		_showText(L"",L"£¨»ÃÏëÏçÄ³´¦£¬ÁéÃÎÃþ×ÅÄÔ´ü£¬Õö¿ªÐÊâìµÄË¯ÑÛ£©");
	}
	return true;
}

CTutorialMode::CTutorialMode()
{

}

CTutorialMode::~CTutorialMode()
{
	
}

void CTutorialMode::startTutorial()
{
	auto director = Director::getInstance();
	director->replaceScene(this);
}

void CTutorialMode::_showText(std::wstring wstrName, std::wstring wstrWord)
{
	_textShown = wstrName;
	_textShown.append(L"\n");
	_textUnshown = wstrWord;
	schedule(schedule_selector(CTutorialMode::_updateText), 0.1F);

	////////////////////
	std::vector<std::wstring> vecTest;
	auto re1 = ui::RichElementText::create(0, Color3B::WHITE, 255, "test", "Helvetica", 28);
	struct
	{
		Color3B color;
	}richTextParam;
	richTextParam.color = Color3B::WHITE;
	std::wstring wstr = L"²âÊÔ²âÊÔ²âÊÔ";
	std::string str;
	vecTest.push_back(wstr);
	for (auto itWstr : vecTest)
	{
		if (itWstr == L"[BLACK]")
		{
			richTextParam.color = Color3B::BLACK;
		}
		else if (itWstr == L"[WHITE]")
		{
			richTextParam.color = Color3B::WHITE;
		}
		else if (itWstr == L"[BLUE]")
		{
			richTextParam.color = Color3B::BLUE;
		}
		else if (itWstr == L"[GREEN]")
		{
			richTextParam.color = Color3B::GREEN;
		}
		else
		{
			auto re1 = ui::RichElementText::create(1, richTextParam.color, 255, WStrToUTF8(itWstr), "Helica", 28);
			richTextDialogue->pushBackElement(re1);
		}
	}
}

void CTutorialMode::_updateText(float dt)
{
	if (_textUnshown.empty())
	{
		unschedule(schedule_selector(CTutorialMode::_updateText));
		return;
	}
	_textShown.append(_textUnshown.substr(0, 1));
	_textUnshown.erase(_textUnshown.begin());
	labelDialogue->setString(WStrToUTF8(_textShown));
}

void CTutorialMode::_showAllText()
{
	unschedule(schedule_selector(CTutorialMode::_updateText));
	_textShown.append(_textUnshown);
	_textUnshown.clear();
	labelDialogue->setString(WStrToUTF8(_textShown));
}