#include "CardDetail.h"
#include "CardsGallery.h"

CCardDetail *CCardDetail::pCardDetail = NULL;

bool CCardDetail::init()
{
	Layer::init();
	auto cardDetailNode = CSLoader::createNode("CardDetail.csb");
	addChild(cardDetailNode);
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