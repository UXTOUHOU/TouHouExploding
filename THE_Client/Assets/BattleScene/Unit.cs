using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace BattleScene
{
	public enum EGroupType
	{
		GT_Yourself,
		GT_Enemy
	}

	public class Unit
	{
		public Cell CurrentCell;
		public CardAttribute UnitAttribute;

		private EGroupType groupType;

		public bool Movable = true;
		public bool Attackable = true;

		public Skill Skill_1;
		public Skill Skill_2;
		public Skill Skill_3;

		private UnitUI unitSprite;

		public int HP
		{
			get
			{
				return UnitAttribute.hp;
			}

			set
			{
				UnitAttribute.hp = value;
				if (value <= 0)
					UnitDeath();
			}
		}

		public EGroupType GroupType
		{
			get
			{
				return groupType;
			}

			set
			{
				if(groupType != value)
					UnitUIManager.GroupChanged = true;
				groupType = value;
			}
		}

		public Unit(int unitID, ChessboardPosition targetPosition)
		{
			CurrentCell = Chessboard.GetCell(targetPosition);

			Skill_1 = null;
			Skill_2 = null;
			Skill_3 = null;

			switch (unitID)
			{
			case 1:
				Skill_1 = new Skill_1_1(this);
				break;
			}

			//Test
			UnitAttribute = new CardAttribute();
			//
			unitSprite = new UnitUI(this, targetPosition);
			////事件穿透
			//UnitImage.AddComponent<RayIgnore>();
		}


		public void UnitDeath()
		{
			if (CurrentCell != null)
				CurrentCell.UnitOnCell = null;

			unitSprite.RemoveUnitSprite();
			//删除技能产生的效果
		}

		public void NormalHurt(int damage)
		{
			HP -= damage;
			UnitUIManager.HPChanged = true;
		}

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}

		public void UpdatePosition()
		{
			throw new NotImplementedException();
		}

		public void MoveWithPath(List<ChessboardPosition> ListMovePath)
		{
			if (BattleProcess.currentState != PlayerState.WaitMoveAnimateEnd)
				BattleProcess.ChangeState(PlayerState.WaitMoveAnimateEnd);
			var lastPosition = ListMovePath[0];
			Vector3 targetPosition = lastPosition.GetPosition();
			//移动
			unitSprite.UnitImage.transform.position = Vector3.MoveTowards(unitSprite.UnitImage.transform.position,
				targetPosition,
				Chessboard.CellSize / 50F);
			if (unitSprite.UnitImage.transform.position == targetPosition)
			{//一段移动结束
				ListMovePath.RemoveAt(0);
				if (ListMovePath.Count == 0)
				{//移动结束
				 //更新Unit的Cell
					var targetCell = Chessboard.GetCell(lastPosition);
					targetCell.SwapUnit(Chessboard.SelectedCell);
					Chessboard.SelectedCell = targetCell;

					targetCell.UnitOnCell.Movable = false;              //单位不可再次移动
					Chessboard.UnitMove = false;
					BattleProcess.ChangeState(PlayerState.SelectUnitBehavior);
				}
			}
		}
	}
}