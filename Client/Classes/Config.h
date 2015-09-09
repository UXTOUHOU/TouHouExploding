#pragma once
#include "LayerMenu.h"

class CConfig : public CLayerMenu
{
public:
	static CConfig *pConfig;
	static CConfig *getInstance()
	{
		if (!pConfig)
			pConfig = CConfig::create();
		return pConfig;
	}
	virtual bool init() override;
	CREATE_FUNC(CConfig);

	virtual SceneType Enter() override;

	CConfig();
	~CConfig();
};

