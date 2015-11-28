using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace BattleScene
{
	public class Cell
	{
		public GameObject Obj;
		public GameObject Background;
		public Unit UnitOnCell;
		public ChessboardPosition ThisPosition;

		public static Color SelectedColor = new Color(10F / 255, 23F / 255, 96F / 255);
		public static Color MovableColor = new Color(70F / 255, 112F / 255, 70F / 255);
		public static Color AttackableColor = new Color(149F / 255, 70F / 255, 70F / 255);
		public static Color HighLightMovableColor = new Color(100F / 255, 142F / 255, 100F / 255);

		public static GameObject btnMove = GameObject.Find("Canvas/ButtonMove");
		public static GameObject btnAttack = GameObject.Find("Canvas/ButtonAttack");
		public static GameObject btnSkill = GameObject.Find("Canvas/ButtonSkill");

		public static GameObject btnSkill_1 = GameObject.Find("Canvas/OperateButton/ButtonSkill_1");
		public static GameObject btnSkill_2 = GameObject.Find("Canvas/OperateButton/ButtonSkill_2");
		public static GameObject btnSkill_3 = GameObject.Find("Canvas/OperateButton/ButtonSkill_3");


		public void ShowOperateButton()
		{
			float cellSize = Chessboard.CellSize;
			//显示按钮
			btnMove.SetActive(true);
			btnAttack.SetActive(true);
			btnSkill.SetActive(true);
			//设置按钮是否被禁用
			Debug.Log("Movable" + UnitOnCell.Movable);
			btnMove.GetComponent<Button>().interactable = UnitOnCell.Movable;
			Debug.Log("Attackable" + UnitOnCell.Attackable);
			btnAttack.GetComponent<Button>().interactable = UnitOnCell.Attackable;
			//更新按钮位置
			btnMove.transform.position = Background.transform.position + new Vector3(0, cellSize / 1.3F, 0);
			btnAttack.transform.position = Background.transform.position + new Vector3(-cellSize / 1.5F, cellSize / 2F, 0);
			btnSkill.transform.position = Background.transform.position + new Vector3(cellSize / 1.5F, cellSize / 2F, 0);
			//将按钮设为在所有Object之上
			GameObject canvas = btnMove.transform.parent.gameObject;
			int childCount = canvas.transform.childCount;
			btnMove.transform.SetSiblingIndex(childCount);
			btnAttack.transform.SetSiblingIndex(childCount);
			btnSkill.transform.SetSiblingIndex(childCount);
		}

		public static void HideOperateButton()
		{
			btnMove.SetActive(false);
			btnAttack.SetActive(false);
			btnSkill.SetActive(false);
		}

		public void ShowSkillButton()
		{
			float cellSize = Chessboard.CellSize;
			//显示按钮
			btnSkill_1.SetActive(true);
			btnSkill_2.SetActive(true);
			btnSkill_3.SetActive(true);
			//设置按钮是否被禁用
			if (UnitOnCell.Skill_1 != null)
				btnSkill_1.GetComponent<Button>().interactable = UnitOnCell.Skill_1.GetUsable();
			if (UnitOnCell.Skill_2 != null)
				btnSkill_2.GetComponent<Button>().interactable = UnitOnCell.Skill_2.GetUsable();
			if (UnitOnCell.Skill_3 != null)
				btnSkill_3.GetComponent<Button>().interactable = UnitOnCell.Skill_3.GetUsable();
			//更新按钮位置
			btnSkill_1.transform.position = Background.transform.position + new Vector3(-cellSize / 1.5F, cellSize / 2F, 0); 
			btnSkill_2.transform.position = Background.transform.position + new Vector3(0, cellSize / 1.3F, 0);
			btnSkill_3.transform.position = Background.transform.position + new Vector3(cellSize / 1.5F, cellSize / 2F, 0);
		}

		public static void HideSkillButton()
		{
			btnSkill_1.SetActive(false);
			btnSkill_2.SetActive(false);
			btnSkill_3.SetActive(false);
		}

		public void SetBackgroundColor(Color newColor)
		{
			Background.GetComponent<Image>().color = newColor;
		}

		public void ShowMovableRange()
		{
			var attribute = UnitOnCell.UnitAttribute;
			for (int x = Math.Max(0, ThisPosition.x - attribute.motility);
				x <= Math.Min(Chessboard.ChessboardMaxX, ThisPosition.x + attribute.motility);
				++x)
				for (int y = Math.Max(0, ThisPosition.y - attribute.motility);
					y <= Math.Min(Chessboard.ChessboardMaxY, ThisPosition.y + attribute.motility);
					++y)
				{
					var itPosition = new ChessboardPosition(x, y);
					int distance = ThisPosition.Distance(itPosition);
					if (distance <= attribute.motility)
						Chessboard.GetCell(itPosition).SetBackgroundColor(MovableColor);
				}
		}

		public Vector3 GetLocalPosition()
		{
			return Background.transform.localPosition;
		}

		public Vector3 GetPosition()
		{
			return Background.transform.position;
		}




		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}

		public void ShowAttackableRange()
		{
			var attribute = UnitOnCell.UnitAttribute;
			for (int x = Math.Max(0,ThisPosition.x - attribute.maxAtkRange); 
				x <= Math.Min(Chessboard.ChessboardMaxX, ThisPosition.x + attribute.maxAtkRange);
				++x)
				for (int y = Math.Max(0, ThisPosition.y - attribute.maxAtkRange);
					y <= Math.Min(Chessboard.ChessboardMaxY, ThisPosition.y + attribute.maxAtkRange);
					++y)
				{
					var itPosition = new ChessboardPosition(x, y);
					int distance = ThisPosition.Distance(itPosition);
					if (attribute.minAtkRange <= distance &&
						distance <= attribute.maxAtkRange)
						Chessboard.GetCell(itPosition).SetBackgroundColor(AttackableColor);
				}
		}

		//public void MoveWithPath()
		//{
		//	if (UnitOnCell == null) return;
		//	if (BattleProcess.playerState != PlayerState.PS_WaitMoveAnimateEnd)
		//		BattleProcess.ChangeState(PlayerState.PS_WaitMoveAnimateEnd);
		//	var lastPosition = ListMovePath[0];
		//	Vector3 targetPosition = lastPosition.GetPosition();
		//	//移动
		//	UnitOnCell.UnitImage.transform.position = Vector3.MoveTowards(UnitOnCell.UnitImage.transform.position,
		//		targetPosition,
		//		Chessboard.CellSize / 50F);
		//	if (UnitOnCell.UnitImage.transform.position == targetPosition)
		//	{//一段移动结束
		//		ListMovePath.RemoveAt(0);
		//		if (ListMovePath.Count == 0)
		//		{//移动结束
		//			//更新Unit的Cell
		//			var targetCell = Chessboard.GetCell(lastPosition);
		//			targetCell.SwapUnit(Chessboard.SelectedCell);
		//			Chessboard.SelectedCell = targetCell;

		//			targetCell.UnitOnCell.Movable = false;              //单位不可再次移动
		//			Chessboard.UnitMove = false;
		//			BattleProcess.ChangeState(PlayerState.PS_SelectUnitBehavior);
		//		}
		//	}
		//}

		public void SwapUnit(Cell targetCell)
		{
			Unit temp = targetCell.UnitOnCell;
			targetCell.UnitOnCell = UnitOnCell;
			UnitOnCell = temp;

			if (targetCell.UnitOnCell != null)
			{
				//更新单位所在的cell
				targetCell.UnitOnCell.CurrentCell = targetCell;
				//更新单位的HP和Group位置
				//targetCell.UnitOnCell.UpdateGroupPosition();
				//targetCell.UnitOnCell.UpdateHPPosition();
				targetCell.UnitOnCell.UpdatePosition();
			}
			if (UnitOnCell != null)
			{
				UnitOnCell.CurrentCell = this;
				//UnitOnCell.UpdateGroupPosition();
				//UnitOnCell.UpdateHPPosition();
				UnitOnCell.UpdatePosition();
			}
		}

		//返回召唤是否成功
		public bool SummonUnit(int unitID, EGroupType group)
		{
			if (UnitOnCell != null) return false;       //格子上已有单位

			UnitOnCell = new Unit(unitID, ThisPosition);
			UnitOnCell.GroupType = group;
			UnitOnCell.CurrentCell = this;
			//UnitOnCell.UnitImage.transform.SetParent(GameObject.Find("/Canvas/UnitImage").transform);
			//UnitOnCell.UnitImage.transform.localPosition = GetLocalPosition();
			//UnitOnCell.UnitImage.transform.localScale = new Vector3(Chessboard.CellSize / 75, Chessboard.CellSize / 75, 1);
			return true;
		}

		public bool IsCanSummonPlace()
		{
			if (ThisPosition.x == 0)	//棋盘最左边的一列
				return true;
			return false;
		}
	}
}