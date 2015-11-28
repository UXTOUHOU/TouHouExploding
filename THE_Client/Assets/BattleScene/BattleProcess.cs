using UnityEngine;
using System.Collections;
using BattleScene;

public enum PlayerState
{
	Wait,					//对方的回合，不可操作
	SetDeck,				//选择牌组
	WaitingOperate,			//等待选择单位或者卡片
	SelectUnitBehavior,		//选择攻击，移动或技能
	//SelectUnit,			//指定单位
	WaitMoveAnimateEnd,		//等待移动动画结束
	WaitAttackAnimateEnd,	//等待攻击动画结束
	SelectAttackTarget,		//指定攻击目标
	SelectMovePosition,		//指定移动位置
	SelectSkill,			//选择使用的技能
	RunningSkill,           //执行技能中
	SelectSummonPosition,	//选择召唤位置
	//WaitSelectCell,		//自定技能的等待选择目标状态
	SkillEnd,				//技能结束 为支持多线程将State改变时的操作放到主线程中
};

namespace BattleScene
{
	public class BattleProcess : MonoBehaviour
	{
		public static PlayerState currentState;
		public static PlayerState StateAfterSkillEnd;	//技能结束后在主线程ChangeState到StateAfterSkillEnd

		public GameObject CanvasObject;
		public GameObject Account;
		public static void ChangeState(PlayerState newState)
		{
			Debug.Log("new state:" + newState);

			//将Skill线程的操作推迟给主线程
			if (newState == PlayerState.SkillEnd)
			{
				currentState = newState;
				return;
			}
			//退出的state
			switch (currentState)
			{
			case PlayerState.Wait:
			case PlayerState.WaitingOperate:
				break;
			case PlayerState.SelectUnitBehavior:
				Cell.HideOperateButton();
				Chessboard.ClearBackground();
				break;
			case PlayerState.SelectMovePosition:
			case PlayerState.SelectAttackTarget:
				Chessboard.ClearBackground();
				break;
			case PlayerState.SelectSummonPosition:
				Chessboard.SelectedCard = null;
				Chessboard.ClearBackground();
				break;
			case PlayerState.SelectSkill:
				Cell.HideSkillButton();
				break;
			case PlayerState.WaitMoveAnimateEnd:
				UnitUIManager.SetAllUnitAttributeVisible(true);
				break;
			case PlayerState.RunningSkill:
				Chessboard.ClearBackground();
				break;
			default:
				Debug.Log("未定义退出state的行为");
				break;
			}
			//进入的state
			switch (newState)
			{
			case PlayerState.Wait:
				break;
			case PlayerState.WaitMoveAnimateEnd:
				UnitUIManager.SetAllUnitAttributeVisible(false);
				break;
			case PlayerState.WaitingOperate:
				break;
			case PlayerState.SelectMovePosition:
				Chessboard.SelectedCell.ShowMovableRange();
				break;
			case PlayerState.SelectAttackTarget:
				Chessboard.SelectedCell.ShowAttackableRange();
				break;
			case PlayerState.SelectSummonPosition:
				//显示所有可能召唤的地点
				for (int x = 0; x < Chessboard.ChessboardMaxX; ++x)
					for (int y = 0; y < Chessboard.ChessboardMaxY; ++y)
					{
						var cell = Chessboard.GetCell(new ChessboardPosition(x, y));
						if (cell.IsCanSummonPlace())
							cell.SetBackgroundColor(Cell.MovableColor);
					}
				break;
			case PlayerState.SelectUnitBehavior:
				if (Chessboard.SelectedCell != null)
					Chessboard.SelectedCell.ShowOperateButton();
				break;
			case PlayerState.SelectSkill:
				Chessboard.SelectedCell.ShowSkillButton();
				break;
			case PlayerState.RunningSkill:
				Cell.HideSkillButton();
				break;
			default:
				Debug.Log("未定义进入state的行为");
				break;
			}
			//
			currentState = newState;
		}

		public void SetAccountActive(bool active)
		{
			Account.SetActive(active);
			Account.transform.SetSiblingIndex(CanvasObject.transform.childCount);
		}

		// Use this for initialization
		void Start()
		{
			//test
			currentState = PlayerState.WaitingOperate;
			//
			Chessboard.ClearBackground();
		}

		// Update is called once per frame
		void Update()
		{
			//按下了右键
			if (Input.GetMouseButtonUp(1))
			{
				switch (currentState)
				{
				case PlayerState.SelectUnitBehavior:
					ChangeState(PlayerState.WaitingOperate);
					break;
				case PlayerState.SelectAttackTarget:
					ChangeState(PlayerState.SelectUnitBehavior);
					break;
				case PlayerState.SelectMovePosition:
					if (Chessboard.RecordingMovePath)
					{
						//清空记录的移动路径
						Chessboard.ClearMovePath();
						//重新显示可移动范围
						Chessboard.ClearBackground();
						Chessboard.SelectedCell.ShowMovableRange();
					}
					else
					{
						ChangeState(PlayerState.SelectUnitBehavior);
					}
					break;
				case PlayerState.SelectSkill:
					ChangeState(PlayerState.SelectUnitBehavior);
					break;
				case PlayerState.SelectSummonPosition:
					ChangeState(PlayerState.WaitingOperate);
					break;
				case PlayerState.WaitingOperate:
					break;
				}
			}
		//
			//SkillEnd
			if (currentState == PlayerState.SkillEnd)
			{
				BattleProcess.ChangeState(PlayerState.SelectSkill);
			}
			if (SkillOperate.ChangeDialogAttribute)
			{
				SkillOperate.MainThreadChessboardDialog();
			}

		}
	}
}