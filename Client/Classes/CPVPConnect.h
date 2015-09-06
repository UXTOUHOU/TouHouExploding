#pragma once
#include "CPVPMode.h"
#include <WinSock2.h>
using namespace std;

enum Order_Type{
	//Global Order
	OT_Common_Inform		= 1,	//通知
	OT_Common_Reply			= 2,	//回复
	OT_Common_Ask			= 3,	//请求
	OT_CS_KeepConnect		= 1004,	//连接保持
	OT_SC_LiveCheck			= 2003,	//存活询问
	OT_SC_AnnounceChat		= 2004,	//推送聊天内容
	OT_CS_SendChat			= 1005,	//发送聊天内容

	//Login
	OT_CS_VersionCheck		= 11001,//询问版本是否可用
	OT_CS_Login				= 11002,//登陆
	OT_CS_AskRoomList		= 21001,//请求房间列表

	//Add Room
	OT_CS_JoinRoom			= 21002,//加入房间
	OT_CS_Ready				= 21003,//准备
	OT_CS_QuitRoom			= 21004,//退出房间
	OT_SC_FlashRoomList		= 22001,//刷新房间列表
	OT_SC_AnnounceReady		= 22002,//告知准备状态
	OT_SC_Start				= 22003,//游戏开始
	OT_SC_KickingPlayer		= 22004,//踢出某人

	//Game Init
	OT_SC_BattleGround		= 32001,//通告战场情况
	OT_CS_InitReady			= 31001,//准备完成
	OT_SC_NextStage			= 32002,//进入下一阶段

	OT_SC_StateException	= 32003,//告知异常状态
	OT_SC_AllInformation	= 32004,//发送全部战场信息
	OT_CS_AskAllInformation = 31002,//请求全部战场信息
	OT_SC_GameEnd			= 32005,//通知游戏结束
	OT_CS_GiveUp			= 31003,//投降
	//32006
	OT_SC_Event				= 32007,//通知事件

	//Round Start
	OT_SC_NewRoundState		= 42001,//新回合信息

	//Round Prepare
	OT_SC_NewState			= 52001,//新的战场状态信息

	//Round Operation
	OT_CS_UnitMove			= 61001,//单位移动
	OT_CS_UnitAttack		= 61002,//单位攻击
	OT_CS_UseCard			= 61003,//使用卡牌
	OT_CS_UseSkill			= 61004,//使用技能
	OT_CS_Summon			= 61005,//召唤少女
	//横置 61006
	OT_CS_EndTurn			= 61007,//结束行动

	OT_SC_UnitMove			= 62001,//单位移动
	OT_SC_UnitAttack		= 62002,//单位攻击
	OT_SC_UseCard			= 62003,//使用卡牌
	OT_SC_UseSkill			= 62004,//使用技能
	OT_SC_Summon			= 62005,//召唤少女
	OT_SC_EndTurn			= 62006,//结束行动
	OT_SC_SpecialEvent		= 62007,//特殊事件

	OT_SC_MandatoryEndTurn	= 62008,//强制回合结束

	//Game End Account
	OT_SC_BattleResult		= 72001,//宣布战果
};

class CPVPConnect
{
public:
	sockaddr_in serverAddr;
	SOCKET clientSocket;

	void init();

	void login(string playerName,string passWord);

	CPVPConnect();
	~CPVPConnect();
private:
	void _sendToServer(std::string str);
};

