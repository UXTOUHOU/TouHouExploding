using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

namespace BattleScene
{
	public abstract class Skill
	{
		public Unit unit = null;
		public List<Unit> affectedUnit;
		protected bool usable = true;
		protected bool cancelable = true;

		public virtual bool GetUsable()
		{
			return usable;
		}

		public virtual bool GetCancelable()
		{
			return cancelable;
		}

		public virtual void InitSkill()
		{
		}

		protected abstract void SkillOperate();

		public void RunSkillThread()
		{
			BattleProcess.ChangeState(PlayerState.RunningSkill);
			new Thread(new ThreadStart(StartSkill)).Start();
		}

		private void StartSkill()
		{
			SkillOperate();
			BattleProcess.ChangeState(PlayerState.SkillEnd);
		}

		protected virtual void OnSkillEnd()
		{
			throw new NotImplementedException();
		}

		public Skill(Unit master)
		{
			unit = master;
		}
	}

	public abstract class ActiveSkill : Skill
	{
		public ActiveSkill(Unit master) : base(master)
		{

		}
		
		public virtual void OnChangeCell(ChessboardPosition position)
		{
		}
	}

	public abstract class PassiveSkill : Skill
	{


		public PassiveSkill(Unit master) : base(master)
		{

		}
	}
}

namespace BattleScene
{
	public class SkillOperate
	{
		public static bool WaitSelectCell = false;
		public static Cell CellSkillTarget;
		public static Mutex CellSkillTargetMutex = new Mutex();

		public static Unit SelectUnit()
		{
			Unit unit = null;
			WaitSelectCell = true;
			while (unit == null)
			{
				CellSkillTargetMutex.WaitOne();
				if (CellSkillTarget != null)
					unit = CellSkillTarget.UnitOnCell;
				CellSkillTargetMutex.ReleaseMutex();
				Thread.Sleep(1);
			}
			WaitSelectCell = false;
			return unit;
		}

		public static void NormalHurt(Unit target, int damage)
		{
			Debug.Assert(damage > 0, "damage <= 0");
			target.NormalHurt(damage);
		}

		public static bool ChangeDialogAttribute = false;
		private static string dialogMessage;
		private static bool dialogVisible = false;
		public static void MainThreadChessboardDialog()
		{
			MutexDialog.WaitOne();
			Chessboard.SetDialogString(dialogMessage);
			Chessboard.SetChessboardDialogVisible(dialogVisible);
			ChangeDialogAttribute = false;
			MutexDialog.ReleaseMutex();
		}

		public static Mutex MutexDialog = new Mutex();
		public static bool ClickDialogButton = false;
		public static bool DialogReturn = false;
		public static bool ChessboardDialog(string message)
		{
			ChangeDialogAttribute = true;
			dialogMessage = message;
			dialogVisible = true;
			ClickDialogButton = false;
			bool res = false;
			while (true)
			{
				MutexDialog.WaitOne();
				if (ClickDialogButton)
				{
					res = DialogReturn;
					MutexDialog.ReleaseMutex();
					break;
				}
				MutexDialog.ReleaseMutex();
				Thread.Sleep(1);
			}
			return res;
		}
	}
}

namespace BattleScene
{
	public class Skill_1_1 : ActiveSkill
	{
		protected override void SkillOperate() 
		{
			//SetSkillRange();
			//SetSkillRangeVisible();
			//SetTargetRange();
			//SetTargetRangeVisible();
			Unit target = BattleScene.SkillOperate.SelectUnit();
			BattleScene.SkillOperate.ChessboardDialog("2");
			target.NormalHurt(2);
			usable = false;
		}

		public Skill_1_1(Unit master) : base(master)
		{

		}
	}
}