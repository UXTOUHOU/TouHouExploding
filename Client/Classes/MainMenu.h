#pragma once
#include "LayerMenu.h"

class CMainMenu : public CLayerMenu
{
public:
	static CMainMenu *pMainMenu;
	static CMainMenu *getInstance()
	{
		if (!pMainMenu)
			pMainMenu = create();
		return pMainMenu;
	}
	virtual bool init() override;
	CREATE_FUNC(CMainMenu);

	virtual void Enter() override;
	virtual void OnButtonReturn() override;

	void OnButtonStart();
	void OnButtonGallery();
	void OnButtonConfig();
	void OnButtonExit();

	CMainMenu();
	~CMainMenu();
};