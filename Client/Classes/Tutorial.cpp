#include "Tutorial.h"

CTutorial *CTutorial::pTutorial = NULL;

CTutorial::CTutorial()
{
}

CTutorial::~CTutorial()
{
}

SceneType CTutorial::Enter()
{
	setVisible(true);
	return sceneTutorial;
}