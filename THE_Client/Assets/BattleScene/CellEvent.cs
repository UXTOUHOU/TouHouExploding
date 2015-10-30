using UnityEngine;
using System.Collections;
using System.Globalization;

namespace BattleScene
{
	public class CellEvent : MonoBehaviour
	{
		public GameObject background;
		public Cell cell;

		public void OnLeftButtonDown()
		{
			switch(BattleProcess.playerState)
			{
			case PlayerState.PS_WaitingOperate:
				//if(unit.group == yours)
				BattleProcess.ChangeState(PlayerState.PS_SelectUnitBehavior);
				cell.ShowOperateButton();
				Chessboard.SelectedCell = cell;
				break;
			default:
				break;
			}
			Debug.Log("Press Cell:" + background.name);
		}

		public void OnMouseEnter()
		{
			switch (BattleProcess.playerState)
			{
			case PlayerState.PS_WaitingOperate:
				cell.SetBackgroundColor(Cell.selected);
				break;
			default:
				break;
			}
			Debug.Log("Enter Cell:" + background.name);
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