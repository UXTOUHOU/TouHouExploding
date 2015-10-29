using UnityEngine;
using System.Collections;

public enum PlayerState
{
	PS_Wait,					//对方的回合，不可操作
	PS_SetSelectDeck,			//选择牌组
	PS_WaitingOperate,			//等待选择单位或者卡片
	//PS_SelectUnitBehavior,	//选择攻击，移动或技能
	//PS_SelectUnit,			//指定单位
	PS_WaitMoveAnimateEnd,		//等待移动动画结束
	PS_WaitAttackAnimateEnd,	//等待攻击动画结束
	PS_SelectAttackTarget,		//指定攻击目标
	PS_SelectMovePosition,		//指定移动位置
	PS_SelectSkill,				//选择使用的技能
	PS_RunningSkill,			//执行技能中
	//PS_WaitSelectCell,		//自定技能的等待选择目标状态
};
public class BattleScene : MonoBehaviour
{
	public static PlayerState playerState;
	public void ChangeState(PlayerState newState)
	{ 
		//退出的state
		switch(playerState)
		{
		case PlayerState.PS_Wait:
			break;
		default:
			Debug.Log("未定义退出state的行为");
			break;
		}
		//进入的state
		switch (newState)
		{
		case PlayerState.PS_Wait:
			break;
		default:
			Debug.Log("未定义进入state的行为");
			break;
		}
		//
		playerState = newState;
	}
	// Use this for initialization
	void Start () {
		playerState = PlayerState.PS_WaitingOperate;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
