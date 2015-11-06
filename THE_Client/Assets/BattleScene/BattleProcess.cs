using UnityEngine;
using System.Collections;
using BattleScene;

public enum PlayerState
{
	PS_Wait,					//对方的回合，不可操作
	PS_SetDeck,					//选择牌组
	PS_WaitingOperate,			//等待选择单位或者卡片
	PS_SelectUnitBehavior,		//选择攻击，移动或技能
	//PS_SelectUnit,			//指定单位
	PS_WaitMoveAnimateEnd,		//等待移动动画结束
	PS_WaitAttackAnimateEnd,	//等待攻击动画结束
	PS_SelectAttackTarget,		//指定攻击目标
	PS_SelectMovePosition,		//指定移动位置
	PS_SelectSkill,				//选择使用的技能
	PS_RunningSkill,            //执行技能中
	PS_SelectSummonPosition,	//选择召唤位置
	//PS_WaitSelectCell,		//自定技能的等待选择目标状态
};

namespace BattleScene
{
	public class BattleProcess : MonoBehaviour
	{
		public static PlayerState playerState;

		public GameObject CanvasObject;
		public GameObject Account;
		public static void ChangeState(PlayerState newState)
		{
			Debug.Log("new state:" + newState);
			//退出的state
			switch (playerState)
			{
			case PlayerState.PS_Wait:
			case PlayerState.PS_WaitingOperate:
				break;
			case PlayerState.PS_SelectUnitBehavior:
				Cell.HideOperateButton();
				Chessboard.ClearBackground();
				break;
			case PlayerState.PS_SelectMovePosition:
			case PlayerState.PS_SelectAttackTarget:
				Chessboard.ClearBackground();
				break;
			case PlayerState.PS_SelectSummonPosition:
				Chessboard.SelectedCard = null;
				Chessboard.ClearBackground();
				break;
			case PlayerState.PS_WaitMoveAnimateEnd:
				Chessboard.SetAllUnitAttributeVisible(true);
				break;
			default:
				Debug.Log("未定义退出state的行为");
				break;
			}
			//进入的state
			switch (newState)
			{
			case PlayerState.PS_Wait:
			case PlayerState.PS_SelectSkill:
				break;
			case PlayerState.PS_WaitMoveAnimateEnd:
				Chessboard.SetAllUnitAttributeVisible(false);
				break;
			case PlayerState.PS_WaitingOperate:
				break;
			case PlayerState.PS_SelectMovePosition:
				Chessboard.SelectedCell.ShowMovableRange();
				break;
			case PlayerState.PS_SelectAttackTarget:
				Chessboard.SelectedCell.ShowAttackableRange();
				break;
			case PlayerState.PS_SelectSummonPosition:
				//显示所有可能召唤的地点
				for (int x = 0; x < Chessboard.ChessboardMaxX; ++x)
					for (int y = 0; y < Chessboard.ChessboardMaxY; ++y)
					{
						var cell = Chessboard.GetCell(new ChessboardPosition(x, y));
						if (cell.IsCanSummonPlace())
							cell.SetBackgroundColor(Cell.MovableColor);
					}
				break;
			case PlayerState.PS_SelectUnitBehavior:
				if (Chessboard.SelectedCell != null)
					Chessboard.SelectedCell.ShowOperateButton();
				break;
			default:
				Debug.Log("未定义进入state的行为");
				break;
			}
			//
			playerState = newState;
		}

		public void SetAccountActive(bool active)
		{
			Account.SetActive(active);
			Account.transform.SetSiblingIndex(CanvasObject.transform.childCount);
			//account.transform.SetParent(GameObject.Find("/Canvas").transform);
		}

		// Use this for initialization
		void Start()
		{
			//test
			playerState = PlayerState.PS_WaitingOperate;
			//
			Chessboard.ClearBackground();
		}

		// Update is called once per frame
		void Update()
		{
			//按下了右键
			if (Input.GetMouseButtonUp(1))
			{
				switch (playerState)
				{
				case PlayerState.PS_SelectUnitBehavior:
					ChangeState(PlayerState.PS_WaitingOperate);
					break;
				case PlayerState.PS_SelectAttackTarget:
					ChangeState(PlayerState.PS_SelectUnitBehavior);
					break;
				case PlayerState.PS_SelectMovePosition:
					if (Cell.RecordingMovePath)
					{
						//清空记录的移动路径
						Cell.ClearMovePath();
						//重新显示可移动范围
						Chessboard.ClearBackground();
						Chessboard.SelectedCell.ShowMovableRange();
					}
					else
					{
						ChangeState(PlayerState.PS_SelectUnitBehavior);
					}
					break;
				case PlayerState.PS_SelectSkill:
					ChangeState(PlayerState.PS_SelectUnitBehavior);
					break;
				case PlayerState.PS_SelectSummonPosition:
					ChangeState(PlayerState.PS_WaitingOperate);
					break;
				case PlayerState.PS_WaitingOperate:
					break;
				}
			}
		}
	}
}