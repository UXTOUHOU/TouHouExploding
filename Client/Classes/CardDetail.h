#pragma once
#include "LayerMenu.h"

class CCardDetail : public CLayerMenu
{
public:
	static CCardDetail *pCardDetail;
	static CCardDetail *getInstance()
	{
		if (!pCardDetail)
			pCardDetail = CCardDetail::create();
		return pCardDetail;
	}
	virtual bool init() override;
	CREATE_FUNC(CCardDetail);

	virtual SceneType Enter() override;

	CCardDetail();
	~CCardDetail();
};

