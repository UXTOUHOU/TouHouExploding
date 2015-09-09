#include "GameHall.h"

CGameHall *CGameHall::pGameHall = NULL;

CGameHall::CGameHall()
{
}


CGameHall::~CGameHall()
{
}

SceneType CGameHall::Enter()
{
	setVisible(true);
	return sceneGameHall;
}