#include "Config.h"

CConfig *CConfig::pConfig = NULL;

CConfig::CConfig()
{
}

CConfig::~CConfig()
{
}

SceneType CConfig::Enter()
{
	setVisible(true);
	return sceneConfig;
}