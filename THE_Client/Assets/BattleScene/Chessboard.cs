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

		/// <summary>
		/// 检测两个位置是否相邻
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public bool Adjacent(ChessboardPosition position)
		{
			return Math.Abs(position.x - x) + Math.Abs(position.y - y) == 1;
		}

		/// <summary>
		/// 计算两个位置的距离
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
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
		public GameObject Background;				// Background所在的Panel

		public static bool UnitMove = false;		// 是否有单位正在移动

		public static Cell SelectedCell;
		public static GameObject SelectedCard;
		public static float CellSize;

		public static bool RecordingMovePath;
		public static List<ChessboardPosition> ListMovePath = new List<ChessboardPosition>();

		private static Chessboard singleton = null;
		private Cell[,] cellArray = new Cell[12, 8];
		public const int ChessboardMaxX = 12;
		public const int ChessboardMaxY = 8;

		public const int BlueSideSummonColumn = 0;						// 蓝色方可以召唤的列
		public const int RedSideSummonColumn = ChessboardMaxX - 1;      // 红色方可以召唤的列

		public static Chessboard GetInstance()
		{
			if (singleton == null)
				singleton = new Chessboard();
			return singleton;
		}

		/// <summary>
		/// 得到x，y坐标表示的cell
		/// </summary>
		/// <param name="position">左下角为(0, 0)</param>
		/// <returns></returns>
		public static Cell GetCell(ChessboardPosition position)
		{
			try
			{
				return GetInstance().cellArray[position.x, position.y];
			}
			catch (Exception) {
				return null;
			}
		}

		/// <summary>
		/// 将每个格子的背景设为黑色
		/// </summary>
		public static void ClearBackground()
		{
			foreach (var it in singleton.cellArray)
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
					cellArray[x, y] = new Cell();
					//每个格子的背景
					Cell cell = cellArray[x, y];
					cell.Position = new ChessboardPosition(x, y);
					cell.Background = Instantiate(ChessboardCellPrefab.transform.FindChild("Background").gameObject);
					cell.Background.name = x + " " + y;
					cell.Background.transform.SetParent(Background.transform);
					cell.Background.transform.localPosition = GetCellPosition(new ChessboardPosition(x, y));
					cell.Background.transform.localScale = new Vector3(CellSize / 75, CellSize / 75, 1);
				}
			//在最下面的一行放上单位
			for (int x = 0; x < ChessboardMaxX; ++x)
				for (int y = 0; y < ChessboardMaxY; ++y)
				{
					var cell = cellArray[x, y];
					if (y == 0)
					{
						if ((x + y) % 2 == 0)
							SkillOperate.SummonUnit(cell, 1, EGroupType.BlueSide);
						else
							SkillOperate.SummonUnit(cell, 1, EGroupType.RedSide);
					}
				}
			//
		}

		/// <summary>
		/// </summary>
		/// <param name="position">左下为(0, 0)</param>
		/// <returns></returns>
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

		/// <summary>
		/// 记录selected cell的移动路径
		/// </summary>
		/// <param name="targetPosition"></param>
		public static void RecordMovePath(ChessboardPosition targetPosition)
		{
			Debug.Log("Record");
            int index;
			if (!RecordingMovePath)
			{
				//当移到初始点时开始记录移动路径
				if (SelectedCell.Position.Distance(targetPosition) == 0)
				{
					GetCell(targetPosition).SetBackgroundColor(Cell.HighLightMovableColor);
					RecordingMovePath = true;
				}
			}
			else
			{
				if (GetCell(targetPosition).UnitOnCell != null) return;              //格子上已有单位
                index = ListMovePath.IndexOf(targetPosition);
				if (ListMovePath.Count >= SelectedCell.UnitOnCell.UnitAttribute.motility && index == -1) return;            //超过单位的机动
				if (ListMovePath.Count == 0)
				{
					//应与初始点相邻
					if (SelectedCell.Position.Adjacent(targetPosition))
					{
						GetCell(targetPosition).SetBackgroundColor(Cell.HighLightMovableColor);
						ListMovePath.Add(targetPosition);
					}else{
						return;
					}
				}
                else if ( index  == -1 && ListMovePath[ListMovePath.Count - 1].Adjacent(targetPosition))
                {
					//应与上一个选择的点相邻
					Chessboard.GetCell(targetPosition).SetBackgroundColor(Cell.HighLightMovableColor);
					ListMovePath.Add(targetPosition);
				}
                else if ( (index = ListMovePath.IndexOf(targetPosition)) != -1)
                {
                    ChessboardPosition lastPos = ListMovePath[ListMovePath.Count - 1];
                    if (lastPos.x != targetPosition.x || lastPos.y != targetPosition.y)
                    {
                        for (int i=index+1;i<ListMovePath.Count;)
                        {
                            lastPos = ListMovePath[i];
                            ListMovePath.RemoveAt(index);
                            Chessboard.GetCell(lastPos).SetBackgroundColor(Cell.MovableColor);
                        }
                    }
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

		/// <summary>
		/// 显示或隐藏对话框。实际是修改了DialogObject的Active属性
		/// </summary>
		/// <param name="visible"></param>
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

		/// <summary>
		/// 显示范围，根据传入的函数判断格子是否需要改变颜色
		/// </summary>
		/// <param name="isChangeColor"></param>
		/// <param name="color"></param>
		public static void SetBackgroundColor(Func<Cell, bool> isChangeColor, Color color)
		{
			for (int x = 0; x < ChessboardMaxX; ++x)
				for (int y = 0; y < ChessboardMaxY; ++y)
				{
					var cell = GetCell(new ChessboardPosition(x, y));
					if (isChangeColor(cell))
						cell.SetBackgroundColor(color);
				}
		}
	}
}