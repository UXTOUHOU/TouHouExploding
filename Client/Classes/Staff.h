#pragma once
#include "LayerMenu.h"
class CStaff : public CLayerMenu
{
public:
	static CStaff *pStaff;
	static CStaff *getInstance()
	{
		if (!pStaff)
			pStaff = CStaff::create();
		return pStaff;
	}
	virtual bool init() override;
	CREATE_FUNC(CStaff);

	virtual void Enter() override;
	virtual void OnButtonReturn() override;

	CStaff();
	~CStaff();
};

