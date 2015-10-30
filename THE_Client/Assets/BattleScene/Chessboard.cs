using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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
	}
	public class Chessboard : MonoBehaviour
	{
		public GameObject ChessboardRect;
		public GameObject ChessboardCellPrefab;
		public GameObject CanvasObject;

		public static Cell SelectedCell;
		public static float CellSize;
	
		private static Chessboard singleton = null;
		private Cell[,] cell = new Cell[12, 8];
		private const int chessboardMaxX = 12;
		private const int chessboardMaxY = 8;

		public static Chessboard GetInstance()
		{
			if (singleton == null)
				singleton = new Chessboard();
			return singleton;
		}
		public static Cell GetCell(ChessboardPosition position)
		{
			return GetInstance().cell[position.x, position.y];
		}
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
			CellSize = Math.Min(chessboardSize.x / chessboardMaxX, chessboardSize.y / chessboardMaxY);

			//测试
			for (int x = 0; x < chessboardMaxX; ++x)
				for (int y = 0; y < chessboardMaxY; ++y)
				{
					//原来是为了在Grid中定位，有空删掉
					cell[x, y] = new Cell();
					GameObject newObj = new GameObject();
					newObj.name = x + " " + y;
					newObj.AddComponent<RectTransform>();
					cell[x, y].obj = newObj;

					//每个格子的背景
					var item = cell[x, y];
					item.background = Instantiate(ChessboardCellPrefab.transform.FindChild("Background").gameObject);
					item.background.name = x + " " + y;
					item.background.transform.SetParent(CanvasObject.transform);
					item.background.transform.localPosition = GetCellLocation(new ChessboardPosition(x, y));
					item.background.transform.localScale = new Vector3(CellSize / 75, CellSize / 75, 1);

					//每个格子添加一个单位
					item.unit = new Unit(1);
					item.unit.image.transform.SetParent(CanvasObject.transform);
					item.unit.image.transform.localPosition = GetCellLocation(new ChessboardPosition(x, y));
					item.unit.image.transform.localScale = new Vector3(CellSize / 75, CellSize / 75, 1);
				}
			//
		}

		//左下为起点
		private Vector2 GetCellLocation(ChessboardPosition position)
		{
			const float pivot = 0.5F;
			RectTransform anchor = ChessboardRect.GetComponent<RectTransform>();
			return new Vector2(anchor.localPosition.x + (position.x + pivot) * CellSize,
				anchor.localPosition.y - (chessboardMaxY - position.y - pivot) * CellSize);
		}
		// Update is called once per frame
		void Update () {
	
		}
	}
}