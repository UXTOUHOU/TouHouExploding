#include "CardsGallery.h"
#include "MainMenu.h"

CCardsGallery *CCardsGallery::pCardGallery = NULL;

bool CCardsGallery::init()
{
	Layer::init();
	auto cardsGalleryMenuNode = CSLoader::createNode("CardsGallery.csb");
	addChild(cardsGalleryMenuNode);

	auto buttonGalleryReturn = cardsGalleryMenuNode->getChildByName<ui::Button *>("Button_ReturnMainMenu");
	buttonGalleryReturn->addClickEventListener(CC_CALLBACK_0(CCardsGallery::OnButtonReturn, this));
	scrollViewCardGallery = cardsGalleryMenuNode->getChildByName<ui::ScrollView *>("ScrollView_CardList");

	setVisible(false);
	return true;
}

CCardsGallery::CCardsGallery()
{
}

CCardsGallery::~CCardsGallery()
{
}

void CCardsGallery::Enter()
{
	setVisible(true);
	CSceneMenu::currentScene = getInstance();
}

void CCardsGallery::OnButtonReturn()
{
	//·µ»ØÖ÷²Ëµ¥
	if (CSceneMenu::currentScene != getInstance())
		return;
	Leave();
	CMainMenu::getInstance()->Enter();
}