#pragma once
#include "SceneBattle.h"
/*
	教程模式
*/

class CTutorialMode : public CSceneBattle
{
public:
	virtual bool init() override;
	CREATE_FUNC(CTutorialMode);
	CTutorialMode();
	~CTutorialMode();

	Layer *layerDilogue;
	Label *labelDialogue;
	ui::RichText *richTextDialogue;
	std::vector< std::pair<int,std::wstring> > vecEvent;

	void startTutorial();
protected:
	void _showText(std::wstring wstrName, std::wstring wstrWord);

	std::wstring _textShown;
	std::wstring _textUnshown;
	void _updateText(float dt);
	void _showAllText();
};