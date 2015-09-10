#include "Staff.h"
#include "Config.h"

CStaff *CStaff::pStaff = NULL;

bool CStaff::init()
{
	Layer::init();
	auto staffNode = CSLoader::createNode("Staff.csb");
	addChild(staffNode);
	setVisible(false);

	auto buttonStaffReturn = staffNode->getChildByName<ui::Button *>("Button_ReturnConfig");
	buttonStaffReturn->addClickEventListener(CC_CALLBACK_0(CStaff::OnButtonReturn, this));

	return true;
}

CStaff::CStaff()
{
}

CStaff::~CStaff()
{
}

void CStaff::Enter()
{
	setVisible(true);
	CSceneMenu::currentScene = getInstance();
}

void CStaff::OnButtonReturn()
{
	Leave();
	CConfig::getInstance()->Enter();
}