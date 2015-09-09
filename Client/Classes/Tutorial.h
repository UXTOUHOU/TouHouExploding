#pragma once
#include "layermenu.h"
class CTutorial : public CLayerMenu
{
public:
	static CTutorial *pTutorial;
	static CTutorial *getInstance()
	{
		if (!pTutorial)
			pTutorial = CTutorial::create();
		return pTutorial;
	}
	virtual bool init() override;
	CREATE_FUNC(CTutorial);

	virtual SceneType Enter() override;

	CTutorial();
	~CTutorial();
};

