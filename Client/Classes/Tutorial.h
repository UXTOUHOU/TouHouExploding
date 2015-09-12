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
	virtual void Enter() override;
	virtual void OnButtonReturn() override;
	virtual void OnMouseScroll(EventMouse *eventMouse) override;
	ui::ScrollView *scrollViewTutorial;


	CTutorial();
	~CTutorial();
};

