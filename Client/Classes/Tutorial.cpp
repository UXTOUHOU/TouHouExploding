#include "Tutorial.h"
#include "Config.h"

CTutorial *CTutorial::pTutorial = NULL;

bool CTutorial::init()
{
	Layer::init();
	auto tutorialNode = CSLoader::createNode("Tutorial.csb");
	addChild(tutorialNode);
	setVisible(false);

	auto buttonTutorialReturn = tutorialNode->getChildByName<ui::Button *>("Button_ReturnConfig");
	buttonTutorialReturn->addClickEventListener(CC_CALLBACK_0(CTutorial::OnButtonReturn, this));
	scrollViewTutorial = tutorialNode->getChildByName<ui::ScrollView *>("ScrollView_Tutorial");
	return true;
}

CTutorial::CTutorial()
{
}

CTutorial::~CTutorial()
{
}

void CTutorial::Enter()
{
	setVisible(true);
	CSceneMenu::currentScene = getInstance();
}

void CTutorial::OnButtonReturn()
{
	if (CSceneMenu::currentScene != getInstance())
		return;
	Leave();
	CConfig::getInstance()->Enter();
}

void CTutorial::OnMouseScroll(EventMouse *eventMouse)
{
	static float currentPercent = 0;
	currentPercent += eventMouse->getScrollY() * 3.0 / 20 * 100;
	if (currentPercent < 0)
		currentPercent = 0;
	else if (currentPercent > 100)
		currentPercent = 100;
	scrollViewTutorial->scrollToPercentVertical(currentPercent, 0.1F, false);
}