#include "CardDetail.h"
#include "CardsGallery.h"
#include "Card.h"

CCardDetail *CCardDetail::pCardDetail = NULL;

bool CCardDetail::init()
{
	Layer::init();
	setVisible(false);
	auto cardDetailNode = CSLoader::createNode("CardDetail.csb");
	addChild(cardDetailNode);
	return true;
}

CCardDetail::CCardDetail()
{

}

CCardDetail::~CCardDetail()
{

}

void CCardDetail::Enter()
{
	setVisible(true);
	CSceneMenu::currentScene = getInstance();
}

void CCardDetail::OnButtonReturn()
{
	Leave();
	CCardsGallery::getInstance()->Enter();
}

void CCardDetail::ShowCardDetail(int cardID)
{
	auto card = getChildByName<Sprite *>("Sprite_Card");
	card->setTexture(GetCardFileName(cardID));
	//Enter();
	//currentScene = sceneCardDetail;
}