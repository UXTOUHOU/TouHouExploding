using UnityEngine;
using System.Collections;
using System.Globalization;

namespace Chessboard
{
	public class CellEvent : MonoBehaviour
	{
		public GameObject background;
		public Cell cell;

		public void OnLeftButtonDown()
		{
			Debug.Log("Press Cell:" + background.name);
		}

		public void OnMouseEnter()
		{
			switch (BattleScene.playerState)
			{
			case PlayerState.PS_WaitingOperate:
				cell.SetBackgroundColor(new Color(0, 0, 0));
				break;
			default:
				break;
			}
			Debug.Log("Enter Cell:" + background.name);
		}

		public void OnMouseLeave()
		{
			Debug.Log("Leave Cell:" + background.name);
		}
		// Use this for initialization
		void Start()
		{
			int x, y;
			string str = background.name.ToString();
			if (str == "Background") return;
			x = int.Parse(str.Split(' ')[0]);
			y = int.Parse(str.Split(' ')[1]);
			cell = Chessboard.GetCell(new ChessboardPosition(x, y));
		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}