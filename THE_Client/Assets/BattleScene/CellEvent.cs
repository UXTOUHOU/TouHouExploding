﻿using UnityEngine;
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
			switch(BattleProcess.currentState)
			{
			case PlayerState.WaitingOperate:
				if (ThisCell.UnitOnCell == null ||											//格子上没有单位
					ThisCell.UnitOnCell.GroupType != EGroupType.GT_Yourself) return;		//非己方单位
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
				BattleProcess.ChangeState(PlayerState.SelectUnitBehavior);
				break;
			case PlayerState.SelectSummonPosition:
				if (!ThisCell.IsCanSummonPlace()) break;
				if (ThisCell.SummonUnit(1 /*Test 应解析成unitID*/, EGroupType.GT_Yourself))
				{
					Deck.RemoveDeckCard(Chessboard.SelectedCard);
					BattleProcess.ChangeState(PlayerState.WaitingOperate);
				}
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
			switch (BattleProcess.currentState)
			{
			case PlayerState.WaitingOperate:
				ThisCell.SetBackgroundColor(Cell.SelectedColor);
				break;
			case PlayerState.SelectMovePosition:
				Chessboard.RecordMovePath(ThisCell.ThisPosition);
				break;
			default:
				break;
			}
		}

		public void OnMouseLeave()
		{
			switch (BattleProcess.currentState)
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