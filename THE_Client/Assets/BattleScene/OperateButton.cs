using UnityEngine;
using System.Collections;
using System;

namespace BattleScene
{
	public class OperateButton : MonoBehaviour
	{
		public void OnClickButtonSkill()
		{
			BattleProcess.ChangeState(PlayerState.SelectSkill);
		}

		public void OnClickButtonMove()
		{
			BattleProcess.ChangeState(PlayerState.SelectMovePosition);
		}

		public void OnClickButtonAttack()
		{
			BattleProcess.ChangeState(PlayerState.SelectAttackTarget);
		}

		public void OnClickButtonSkill_1()
		{
			Skill skill = Chessboard.SelectedCell.UnitOnCell.Skill_1;
			if (skill != null)
				skill.RunSkillThread();
		}

		public void OnClickButtonSkill_2()
		{
			Skill skill = Chessboard.SelectedCell.UnitOnCell.Skill_2;
			if (skill != null)
				skill.RunSkillThread();
		}

		public void OnClickButtonSkill_3()
		{
			Skill skill = Chessboard.SelectedCell.UnitOnCell.Skill_3;
			if (skill != null)
				skill.RunSkillThread();
		}

		public void OnClickButtonDialogYes()
		{
			SkillOperate.MutexDialog.WaitOne();
			SkillOperate.DialogReturn = true;
			SkillOperate.ClickDialogButton = true;
			Chessboard.SetChessboardDialogVisible(false);
			SkillOperate.MutexDialog.ReleaseMutex();
		}

		public void OnClickButtonDialogNo()
		{
			SkillOperate.MutexDialog.WaitOne();
			SkillOperate.DialogReturn = false;
			SkillOperate.ClickDialogButton = true;
			Chessboard.SetChessboardDialogVisible(false);
			SkillOperate.MutexDialog.ReleaseMutex();
		}

		public void OnButtonGiveUp()
		{
			throw new NotImplementedException();
		}

		public void OnButtonEndTurn()
		{
			// Test 结束回合时转变阵营，使同一个玩家可以操纵两个阵营
			if(BattleProcess.GroupType == EGroupType.BlueSide)
				BattleProcess.GroupType = EGroupType.RedSide;
			else
				BattleProcess.GroupType = EGroupType.BlueSide;
			BattleProcess.NewRound(BattleProcess.GroupType);
			BattleProcess.ChangeState(PlayerState.WaitingOperate);
			//
		}

		public void OnButtonReturnMenu()
		{
			throw new NotImplementedException();
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