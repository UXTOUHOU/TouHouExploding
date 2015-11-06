using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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

		public static bool RecordingMovePath { get; set; }
		public static ArrayList listMovePath = new ArrayList();

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

		public void RecordMovePath(ChessboardPosition targetPosition)
		{
			Debug.Log("Record");
			if (!RecordingMovePath)
			{
				//当移到初始点时开始记录移动路径
				if (ThisPosition.Distance(targetPosition) == 0)
				{
					Chessboard.GetCell(targetPosition).SetBackgroundColor(HighLightMovableColor);
					RecordingMovePath = true;
				}
			}
			else {
				if (Chessboard.GetCell(targetPosition).UnitOnCell != null) return;              //格子上已有单位
				if (listMovePath.Count >= UnitOnCell.UnitAttribute.motility) return;			//超过单位的机动
				if (listMovePath.Count == 0)
				{
					//应与初始点相邻
					if (ThisPosition.Adjacent(targetPosition))
					{
						Chessboard.GetCell(targetPosition).SetBackgroundColor(HighLightMovableColor);
						listMovePath.Add(targetPosition);
					}else{
						return;
					}
				}
				else if (((ChessboardPosition)listMovePath[listMovePath.Count - 1]).Adjacent(targetPosition))
				{
					//应与上一个选择的点相邻
					Chessboard.GetCell(targetPosition).SetBackgroundColor(HighLightMovableColor);
					listMovePath.Add(targetPosition);
				}
			}
		}

		public static void ClearMovePath()
		{
			RecordingMovePath = false;
			listMovePath.Clear();
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

		public void MoveWithPath()
		{
			//if (!RecordingMovePath) return;
			if (UnitOnCell == null) return;
			if (BattleProcess.playerState != PlayerState.PS_WaitMoveAnimateEnd)
				BattleProcess.ChangeState(PlayerState.PS_WaitMoveAnimateEnd);
			var lastPosition = ((ChessboardPosition)listMovePath[0]);
			Vector3 targetPosition = lastPosition.GetPosition();
			UnitOnCell.UnitImage.transform.position = Vector3.MoveTowards(UnitOnCell.UnitImage.transform.position,
				targetPosition,
				Chessboard.CellSize / 50F);
			if (UnitOnCell.UnitImage.transform.position == targetPosition)
				if (listMovePath.Count == 1)
				{//移动结束
					//更新Unit的Cell
					var targetCell = Chessboard.GetCell(lastPosition);
					targetCell.SwapUnit(Chessboard.SelectedCell);
					Chessboard.SelectedCell = targetCell;

					targetCell.UnitOnCell.Movable = false;				//单位不可再次移动
					listMovePath.Clear();
					Chessboard.UnitMove = false;
					BattleProcess.ChangeState(PlayerState.PS_SelectUnitBehavior);
				}
				else {
					listMovePath.RemoveAt(0);
				}
		}

		private void SwapUnit(Cell targetCell)
		{
			Unit temp = targetCell.UnitOnCell;
			targetCell.UnitOnCell = UnitOnCell;
			UnitOnCell = temp;

			if (targetCell.UnitOnCell != null)
			{
				//更新单位所在的cell
				targetCell.UnitOnCell.CurrentCell = targetCell;
				//更新单位的HP和Group位置
				targetCell.UnitOnCell.UpdateGroupPosition();
				targetCell.UnitOnCell.UpdateHPPosition();
			}
			if (UnitOnCell != null)
			{
				UnitOnCell.CurrentCell = this;
				UnitOnCell.UpdateGroupPosition();
				UnitOnCell.UpdateHPPosition();
			}
		}

		//返回召唤是否成功
		public bool SummomUnit(int unitID, EGroupType group)
		{
			if (UnitOnCell != null) return false;       //格子上已有单位

			UnitOnCell = new Unit(unitID);
			UnitOnCell.GroupType = group;
			UnitOnCell.CurrentCell = this;
			UnitOnCell.UnitImage.transform.SetParent(GameObject.Find("/Canvas/UnitImage").transform);
			UnitOnCell.UnitImage.transform.localPosition = GetLocalPosition();
			UnitOnCell.UnitImage.transform.localScale = new Vector3(Chessboard.CellSize / 75, Chessboard.CellSize / 75, 1);

			UnitOnCell.InitUnitGroup();
			UnitOnCell.InitUnitHP();
			UnitOnCell.SetGroupVisible(true);
			UnitOnCell.SetHPVisible(true);

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