using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace Chessboard
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
		static Chessboard singleton = null;
		public GameObject cellGrid;
		public GameObject chessboardRect;
		public GameObject chessboardCellPrefab;
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

		// Use this for initialization
		void Start () {
			singleton = this;
			RectTransform anchor = chessboardRect.GetComponent<RectTransform>();
			Vector2 chessboardSize = new Vector2(Screen.width *(anchor.anchorMax.x - anchor.anchorMin.x),
				Screen.height * (anchor.anchorMax.y - anchor.anchorMin.y));
			float newCellSize = Math.Min(chessboardSize.x / chessboardMaxX, chessboardSize.y / chessboardMaxY);
			cellGrid.GetComponent<GridLayoutGroup>().cellSize = new Vector2(newCellSize, newCellSize);
			for (int x = 0; x < chessboardMaxX; ++x)
				for (int y = 0; y < chessboardMaxY; ++y)
				{
					cell[x, y] = new Cell();
					GameObject newObj = new GameObject();
					newObj.name = x + " " + y;
					newObj.AddComponent<RectTransform>();
					cell[x, y].obj = newObj;
					newObj.transform.SetParent(cellGrid.transform);
				}
			foreach (var item in cell)
			{
				if (item.unit == null)
				{
					//Debug.Log();
					item.background = Instantiate(chessboardCellPrefab.transform.FindChild("Background").gameObject);
					item.background.transform.SetParent(item.obj.transform);
					item.background.transform.localScale = new Vector3(newCellSize / 75, newCellSize / 75, 1);

					item.unit = new Unit(1);
					item.unit.image.transform.SetParent(item.obj.transform);
					item.unit.image.transform.localScale = new Vector3(newCellSize / 75, newCellSize / 75, 1);
					//item.unit.image.transform.position = chessboardRect.transform.localPosition;

					//Debug.Log(item.unit);
					//Debug.Log(item.unit.image);
					//Debug.Log(item.obj);
					//item.unit.image.transform.SetParent(item.obj.transform);
				}
			}
		}
	
		// Update is called once per frame
		void Update () {
	
		}
	}
}