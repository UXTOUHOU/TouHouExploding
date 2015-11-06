using UnityEngine;
using System.Collections;

namespace BattleScene
{
	public class CellEvent : MonoBehaviour
	{
		public GameObject Background;
		public Cell ThisCell;

		public void OnLeftButtonDown()
		{
			if (!Input.GetMouseButtonDown(0)) return;										//按下的是右键
			switch(BattleProcess.playerState)
			{
			case PlayerState.PS_WaitingOperate:
				if (ThisCell.UnitOnCell == null ||											//格子上没有单位
					ThisCell.UnitOnCell.GroupType != EGroupType.GT_Yourself) return;   //非己方单位
				BattleProcess.ChangeState(PlayerState.PS_SelectUnitBehavior);
				ThisCell.ShowOperateButton();
				ThisCell.SetBackgroundColor(Cell.SelectedColor);
				Chessboard.SelectedCell = ThisCell;
				break;
			case PlayerState.PS_SelectMovePosition:
				if (Cell.RecordingMovePath == false) return;
				if (Cell.listMovePath.Count != 0)
					Chessboard.UnitMove = true;
				break;
			case PlayerState.PS_SelectAttackTarget:
				if (ThisCell == null ||																	
						ThisCell == Chessboard.SelectedCell ||											//选择了自身
						ThisCell.UnitOnCell == null ||													//格子上没有单位
						ThisCell.UnitOnCell.GroupType == Chessboard.SelectedCell.UnitOnCell.GroupType)	//攻击目标有相同的阵营
					return;
				//是否在攻击范围内
				int distance = ThisCell.ThisPosition.Distance(Chessboard.SelectedCell.ThisPosition);
				var attribue = Chessboard.SelectedCell.UnitOnCell.UnitAttribute;
				if (distance < attribue.minAtkRange ||
					distance > attribue.maxAtkRange) return;
				//计算伤害等
				ThisCell.UnitOnCell.HP -= Chessboard.SelectedCell.UnitOnCell.UnitAttribute.attack;
				Chessboard.SelectedCell.UnitOnCell.Attackable = false;
				BattleProcess.ChangeState(PlayerState.PS_SelectUnitBehavior);
				break;
			case PlayerState.PS_SelectSummonPosition:
				if (!ThisCell.IsCanSummonPlace()) break;
				if (ThisCell.SummomUnit(1 /*Test 应解析成unitID*/, EGroupType.GT_Yourself))
				{
					Deck.RemoveDeckCard(Chessboard.SelectedCard);
					BattleProcess.ChangeState(PlayerState.PS_WaitingOperate);
				}
				break;
			default:
				break;
			}
			Debug.Log("Press Cell:" + Background.name);
		}

		public void OnMouseEnter()
		{
			switch (BattleProcess.playerState)
			{
			case PlayerState.PS_WaitingOperate:
				ThisCell.SetBackgroundColor(Cell.SelectedColor);
				break;
			case PlayerState.PS_SelectMovePosition:
				Chessboard.SelectedCell.RecordMovePath(ThisCell.ThisPosition);
				break;
			default:
				break;
			}
		}

		public void OnMouseLeave()
		{
			switch (BattleProcess.playerState)
			{
			case PlayerState.PS_WaitingOperate:
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