#include "Staff.h"

CStaff *CStaff::pStaff = NULL;

CStaff::CStaff()
{
}

CStaff::~CStaff()
{
}

SceneType CStaff::Enter()
{
	setVisible(true);
	return sceneStaff;
}