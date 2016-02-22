using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BattleScene
{
	public class CellEvent : MonoBehaviour
	{
		public GameObject Background;
		public Cell ThisCell;

		public void OnLeftButtonDown()
		{
			if (!Input.GetMouseButtonDown(0)) return;                                       //按下的是右键
			switch (BattleProcess.CurrentState)
			{
			case PlayerState.WaitingOperate:
				if (ThisCell.UnitOnCell == null ||                                          //格子上没有单位
					ThisCell.UnitOnCell.GroupType != BattleProcess.GroupType) return;       //非己方单位
				BattleProcess.ChangeState(PlayerState.SelectUnitBehavior);
				ThisCell.ShowOperateButton();
				ThisCell.SetBackgroundColor(Cell.SelectedColor);
				Chessboard.SelectedCell = ThisCell;
				break;
			case PlayerState.SelectMovePosition:
				if (Chessboard.RecordingMovePath == false) return;
				if (Chessboard.ListMovePath.Count != 0)
					Chessboard.UnitMove = true;
				break;
			case PlayerState.SelectAttackTarget:
				if (ThisCell == null ||
						ThisCell == Chessboard.SelectedCell ||                                          //选择了自身
						ThisCell.UnitOnCell == null ||                                                  //格子上没有单位
						ThisCell.UnitOnCell.GroupType == Chessboard.SelectedCell.UnitOnCell.GroupType)  //攻击目标有相同的阵营
					return;
				//是否在攻击范围内
				int distance = ThisCell.Position.Distance(Chessboard.SelectedCell.Position);
				var attribue = Chessboard.SelectedCell.UnitOnCell.UnitAttribute;
				if (distance < attribue.minAtkRange ||
					distance > attribue.maxAtkRange) return;
				//计算伤害等
				//ThisCell.UnitOnCell.HP -= Chessboard.SelectedCell.UnitOnCell.UnitAttribute.attack;
				SkillOperate.NormalHurt(ThisCell.UnitOnCell,
					Chessboard.SelectedCell.UnitOnCell.UnitAttribute.attack);   //直接造成了等于攻击力的伤害
				Chessboard.SelectedCell.UnitOnCell.Attackable = false;
				BattleProcess.ChangeState(PlayerState.SelectUnitBehavior);
				break;
			case PlayerState.SelectSummonPosition:
				if (!ThisCell.IsCanSummonPlace()) break;
				if (BattleProcess.GroupType == EGroupType.BlueSide)
				{
					if (ThisCell.Position.x != Chessboard.BlueSideSummonColumn) break;
				}
				else if (BattleProcess.GroupType == EGroupType.RedSide)
				{
					if (ThisCell.Position.x != Chessboard.RedSideSummonColumn) break;
				}
				SkillOperate.SummonUnit(ThisCell, 1 /*Test 应解析成unitID*/, BattleProcess.GroupType);
				Deck.RemoveDeckCard(Chessboard.SelectedCard);
				BattleProcess.ChangeState(PlayerState.WaitingOperate);
				break;
			case PlayerState.RunningSkill:
				if (SkillOperate.WaitSelectCell)
				{
					SkillOperate.CellSkillTargetMutex.WaitOne();
					SkillOperate.CellSkillTarget = ThisCell;
					SkillOperate.CellSkillTargetMutex.ReleaseMutex();
				}
				break;
			default:
				break;
			}
			Debug.Log("Press Cell:" + Background.name);
		}

		public void OnMouseEnter()
		{
			switch (BattleProcess.CurrentState)
			{
			case PlayerState.WaitingOperate:
				ThisCell.SetBackgroundColor(Cell.SelectedColor);
				break;
			case PlayerState.SelectMovePosition:
				Chessboard.RecordMovePath(ThisCell.Position);
				break;
			default:
				break;
			}
		}

		public void OnMouseLeave()
		{
			switch (BattleProcess.CurrentState)
			{
			case PlayerState.WaitingOperate:
				Chessboard.ClearBackground();
				break;
			default:
				break;
			}
		}
		// Use this for initialization
		void Start()
		{
			int x, y;
			string str = Background.name.ToString();
			if (str == "Background") return;
			x = int.Parse(str.Split(' ')[0]);
			y = int.Parse(str.Split(' ')[1]);
			ThisCell = Chessboard.GetCell(new ChessboardPosition(x, y));
		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}