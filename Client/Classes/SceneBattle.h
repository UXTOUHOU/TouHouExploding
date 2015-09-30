#pragma once
#include <string>
#include <vector>
#include "cocos2d.h"
#include "cocostudio/CocoStudio.h"
#include "ui/CocosGUI.h"
#include "Chessboard.h"
#include "ChessboardPosition.h"
#include "SceneMenu.h"
#include "Effects.h"
#include "Card.h"

USING_NS_CC;

extern bool g_bTest;
class CLayerChessboard;
enum PlayerState;

void ChangeState(PlayerState newState);

enum PlayerState
{
	PS_Wait,				//对方的回合，不可操作
	PS_WaitingOperate,		//等待选择单位或者卡片
	PS_SelectUnitBehavior,	//选择攻击，移动或技能
	PS_SelectUnit,			//指定单位
	PS_WaitMoveAnimateEnd,	//等待移动动画结束
	PS_WaitAttackAnimateEnd,//等待攻击动画结束
	PS_SelectAttackTarget,	//指定攻击目标
	PS_SelectMovePosition,	//指定移动位置
	PS_SelectSkill,			//选择使用的技能
	PS_RunningSkill,		//执行技能中
	PS_WaitSelectCell,		//自定技能的等待选择目标状态
};

class CSceneBattle : public cocos2d::Scene
{
public:
	const float cardWidth = 420;
	const float cardHeight = 590;
	const float panelCardWidth = 800;
	const float panelCardHeight = 180;
	const float cardScale = panelCardHeight / cardHeight;

	static CSceneBattle *pSceneBattle;
	static CSceneBattle *getInstance()
	{
		if (pSceneBattle == NULL)
			pSceneBattle = new CSceneBattle;
		return pSceneBattle;
	}
	//static cocos2d::Scene* createScene();
	virtual bool init() override;
	CREATE_FUNC(CSceneBattle);

	CLayerChessboard *chessboard;
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
	//战斗流程

	void StartYourTurn();
	void EndYourTurn();
	void SelectUnit();
	void SelectPosition();

	PlayerState playerState;
private:
	//删除或插入手牌之后对summonPool或者handCards中的卡片重新排序
	void _RedrawCards(Node *node);

	//vector<CSKill> _vecOnRoundStart;
};

////将card添加为father的child并修改为指定的大小
//void ShowCard(Card *card, Rect rect, Node *father);
//void ShowCard(Card *card, Point position, float scale, Node *father);
//返回卡图的文件名
////由ID创建card的Sprite
//Sprite *CreateCardSprite(int cardID);
