using UnityEngine;
using System.Collections;

namespace BattleScene
{
	public class Button : MonoBehaviour
	{
		public void OnClickButtonSkill()
		{
			Chessboard.SelectedCell.HideOperateButton();
			BattleProcess.ChangeState(PlayerState.PS_SelectSkill);
		}

		public void OnClickButtonMove()
		{
			Chessboard.SelectedCell.HideOperateButton();
			BattleProcess.ChangeState(PlayerState.PS_SelectMovePosition);
		}

		public void OnClickButtonAttack()
		{
			Chessboard.SelectedCell.HideOperateButton();
			BattleProcess.ChangeState(PlayerState.PS_SelectAttackTarget);
		}

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}