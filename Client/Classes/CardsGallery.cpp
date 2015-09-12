#include "CardsGallery.h"
#include "CardDetail.h"
#include "MainMenu.h"
#include "CCard.h"

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

void CCardsGallery::OnMouseDown(EventMouse *eventMouse)
{
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
			CCardDetail::getInstance()->ShowCardDetail(card->ID);
		}
	}
}