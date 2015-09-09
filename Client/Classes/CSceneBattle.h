#pragma once
#include "cocos2d.h"
#include "cocostudio/CocoStudio.h"
#include "ui/CocosGUI.h"
#include "CLayerChessBoard.h"

USING_NS_CC;

std::string WStrToUTF8(const std::wstring& src);
extern bool gbTest;

class CSceneBattle : public cocos2d::Scene
{
public:
	const float cardWidth = 420;
	const float cardHeight = 590;
	const float panelCardWidth = 900;
	const float panelCardHeight = 220;
	const float cardScale = panelCardHeight / cardHeight;

	static cocos2d::Scene* createScene();
	virtual bool init() override;
	CREATE_FUNC(CSceneBattle);

	CLayerChessBoard *chessBoard;
	Layer *layerBattleScene;
	Layer *layerShade;
	Node *nodeBackGround;
	Node *nodePauseMenu;
	Node *nodeAbandonedCards;
	ui::Layout *panelCards;
	ui::Text *textCardDescribe;
	ui::Text *textRoundCountdown;
	ui::Text *textTotalRound;
	ui::ScrollView *scrollViewAbandonedCards;

	ui::Button *buttonAbandonedCards;
	ui::Button *buttonSwitch;
	ui::Button *buttonEndTurn;
	ui::Button *buttonGiveUp;

	Node *nodeHandCards;
	Node *nodeSummonPool;

	enum EScene{
		sceneBattle,
		scenePauseMenu,
		sceneAbandonedCards,
	}currentScene;

	//行动
	void UnitAttack();
	//投降和结束回合
	void OnButtonGiveUp();
	void OnButtonEndTurn();
	//计时器
	void UpdateRoundCountdown(float t);
	//将卡片显示到panelCard中，新加入的卡片在最右端
	void AddHandCards(int cardID);
	void AddSummonPool(int cardID);
	//弃牌堆
	void AddAbandonedCards(int cardID);
	void OnButtonAbandonedCards();
	void AbandonedCardsRetuen();
	//切换召唤池和手牌
	bool bHandCards;	//true:handCards	false:summonPool
	void OnButtonSwitch();
	//PauseMenu Button
	void OnButtonReturnMainMenu();
	void OnButtonContinue();
	//
	int totalRound;
	void AddTotalRound();
	//回合倒计时
	float roundRestTime;
	int timeCount;
	void RoundCountdown(float n);
private:
	//删除或插入手牌之后对summonPool或者handCards中的卡片重新排序
	void _RedrawCards(Node *node);	
};

////将card添加为father的child并修改为指定的大小
//void ShowCard(Card *card, Rect rect, Node *father);
//void ShowCard(Card *card, Point position, float scale, Node *father);
//返回卡图的文件名
char *GetCardFileName(char *fileName, int cardID);
////由ID创建card的Sprite
//Sprite *CreateCardSprite(int cardID);