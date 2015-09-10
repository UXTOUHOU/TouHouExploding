#include "Config.h"
#include "MainMenu.h"

CConfig *CConfig::pConfig = NULL;

bool CConfig::init()
{
	Layer::init();
	auto configMenuNode = CSLoader::createNode("Config.csb");
	addChild(configMenuNode);
	setVisible(false);

	auto buttonConfigReturn = configMenuNode->getChildByName<ui::Button *>("Button_ReturnMainMenu");
	buttonConfigReturn->addClickEventListener(CC_CALLBACK_0(CConfig::OnButtonReturn, this));
	auto buttonTutorial = configMenuNode->getChildByName<ui::Button *>("Button_Tutorial");
	buttonTutorial->addClickEventListener(CC_CALLBACK_0(CConfig::OnButtonTutorial, this));
	auto buttonStaff = configMenuNode->getChildByName<ui::Button *>("Button_Staff");
	buttonStaff->addClickEventListener(CC_CALLBACK_0(CConfig::OnButtonStaff, this));
	return true;
}

CConfig::CConfig()
{
}

CConfig::~CConfig()
{
}

void CConfig::Enter()
{
	setVisible(true);
	CSceneMenu::currentScene = getInstance();
}

void CConfig::OnButtonReturn()
{
	Leave();
	CMainMenu::getInstance()->Enter();
}