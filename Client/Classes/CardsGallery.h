#pragma once
#include "LayerMenu.h"
class CCardsGallery : public CLayerMenu
{
public:
	static CCardsGallery *pCardGallery;
	static CCardsGallery *getInstance()
	{
		if (!pCardGallery)
			pCardGallery = CCardsGallery::create();
		return pCardGallery;
	}
	virtual bool init() override;
	CREATE_FUNC(CCardsGallery);

	virtual SceneType Enter() override;

	CCardsGallery();
	~CCardsGallery();
};

