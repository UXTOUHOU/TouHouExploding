using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace BattleScene
{
	public class ChessboardPosition
	{
		public int x;
		public int y;
		public ChessboardPosition(int new_x, int new_y)
		{
			x = new_x;
			y = new_y;
		}

		//检测两个位置是否相邻
		public bool Adjacent(ChessboardPosition position)
		{
			return Math.Abs(position.x - x) + Math.Abs(position.y - y) == 1;
		}

		//返回两个位置相距的步数
		public int Distance(ChessboardPosition position)
		{
			return Math.Abs(x - position.x) + Math.Abs(y - position.y);
		}

		public Vector3 GetLocalPosition()
		{
			return Chessboard.GetCell(this).GetLocalPosition();
		}

		public Vector3 GetPosition()
		{
			return Chessboard.GetCell(this).GetPosition();
		}
	}
	public class Chessboard : MonoBehaviour
	{
		public GameObject ChessboardRect;
		public GameObject ChessboardCellPrefab;
		public GameObject ChessboardDialog;
		public GameObject CanvasObject;
		public GameObject Background;

		public static bool UnitMove = false;

		public static Cell SelectedCell;
		public static GameObject SelectedCard;
		public static float CellSize;

		public static bool RecordingMovePath;
		public static List<ChessboardPosition> ListMovePath;

		private static Chessboard singleton = null;
		private Cell[,] cell = new Cell[12, 8];
		public const int ChessboardMaxX = 12;
		public const int ChessboardMaxY = 8;

		public static Chessboard GetInstance()
		{
			if (singleton == null)
				singleton = new Chessboard();
			return singleton;
		}

		//得到x，y坐标表示的cell。左下角为0，0。
		public static Cell GetCell(ChessboardPosition position)
		{
			return GetInstance().cell[position.x, position.y];
		}

		//将每个格子的背景设为黑色
		public static void ClearBackground()
		{
			foreach (var it in singleton.cell)
				it.SetBackgroundColor(Color.black);
		}

		// Use this for initialization
		void Start () {
			//Init
			singleton = this;
			RectTransform anchor = ChessboardRect.GetComponent<RectTransform>();
			Vector2 chessboardSize = new Vector2(Screen.width *(anchor.anchorMax.x - anchor.anchorMin.x),
				Screen.height * (anchor.anchorMax.y - anchor.anchorMin.y));
			CellSize = Math.Min(chessboardSize.x / ChessboardMaxX, chessboardSize.y / ChessboardMaxY);

			//Test
			for (int x = 0; x < ChessboardMaxX; ++x)
				for (int y = 0; y < ChessboardMaxY; ++y)
				{
					cell[x, y] = new Cell();
					//每个格子的背景
					var item = cell[x, y];
					item.ThisPosition = new ChessboardPosition(x, y);
					item.Background = Instantiate(ChessboardCellPrefab.transform.FindChild("Background").gameObject);
					item.Background.name = x + " " + y;
					item.Background.transform.SetParent(Background.transform);
					item.Background.transform.localPosition = GetCellPosition(new ChessboardPosition(x, y));
					item.Background.transform.localScale = new Vector3(CellSize / 75, CellSize / 75, 1);
				}
			//在最下面的一行放上单位
			for (int x = 0; x < ChessboardMaxX; ++x)
				for (int y = 0; y < ChessboardMaxY; ++y)
				{
					var item = cell[x, y];
					if (y == 0)
					{
						if ((x + y) % 2 == 0)
							item.SummonUnit(1, EGroupType.GT_Yourself);
						else
							item.SummonUnit(1, EGroupType.GT_Enemy);
					}
				}
			//
		}

		//左下为起点
		private Vector2 GetCellPosition(ChessboardPosition position)
		{
			const float pivot = 0.5F;
			RectTransform anchor = ChessboardRect.GetComponent<RectTransform>();
			return new Vector2(anchor.localPosition.x + (position.x + pivot) * CellSize,
				anchor.localPosition.y - (ChessboardMaxY - position.y - pivot) * CellSize);
		}

		// Update is called once per frame
		void Update ()
		{
			if (UnitMove)
				SelectedCell.UnitOnCell.MoveWithPath(ListMovePath);
		}

		//记录selected cell的移动路径
		public static void RecordMovePath(ChessboardPosition targetPosition)
		{
			Debug.Log("Record");
			if (!RecordingMovePath)
			{
				//当移到初始点时开始记录移动路径
				if (SelectedCell.ThisPosition.Distance(targetPosition) == 0)
				{
					GetCell(targetPosition).SetBackgroundColor(Cell.HighLightMovableColor);
					RecordingMovePath = true;
				}
			}
			else
			{
				if (GetCell(targetPosition).UnitOnCell != null) return;              //格子上已有单位
				if (ListMovePath.Count >= SelectedCell.UnitOnCell.UnitAttribute.motility) return;            //超过单位的机动
				if (ListMovePath.Count == 0)
				{
					//应与初始点相邻
					if (SelectedCell.ThisPosition.Adjacent(targetPosition))
					{
						GetCell(targetPosition).SetBackgroundColor(Cell.HighLightMovableColor);
						ListMovePath.Add(targetPosition);
					}else{
						return;
					}
				}else if (ListMovePath[ListMovePath.Count - 1].Adjacent(targetPosition)){
					//应与上一个选择的点相邻
					Chessboard.GetCell(targetPosition).SetBackgroundColor(Cell.HighLightMovableColor);
					ListMovePath.Add(targetPosition);
				}
			}
		}

		public static void ClearMovePath()
		{
			RecordingMovePath = false;
			ListMovePath.Clear();
		}

		public static void SetDialogString(string message)
		{
			var dialog = Chessboard.GetInstance().ChessboardDialog;
			dialog.transform.Find("Text").GetComponent<Text>().text = message;
		}

		//实际是修改了DialogObject的Active属性
		public static void SetChessboardDialogVisible(bool visible)
		{
			Chessboard.GetInstance().ChessboardDialog.SetActive(visible);
		}

		public static void SetSkillRange(int range)
		{
			throw new NotImplementedException();
		}

		public static void SetSkillRangeVisible(bool visible)
		{
			throw new NotImplementedException();
		}

		public static void SetSkillTargetRange(int range)
		{
			throw new NotImplementedException();
		}

		public static void SetSkillTargetRangeVisible(int visible)
		{
			throw new NotImplementedException();
		}
	}
}