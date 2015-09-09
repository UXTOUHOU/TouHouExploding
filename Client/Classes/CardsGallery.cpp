#include "CardsGallery.h"

CCardsGallery *CCardsGallery::pCardGallery = NULL;

CCardsGallery::CCardsGallery()
{
}

CCardsGallery::~CCardsGallery()
{
}

SceneType CCardsGallery::Enter()
{
	setVisible(true);
	return sceneGallery;
}