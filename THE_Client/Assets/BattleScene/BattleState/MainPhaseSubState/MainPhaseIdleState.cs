using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MainPhaseIdleState : BattleStateBase
{

	public MainPhaseIdleState(IFSM fsm)
		:base(fsm)
	{

	}

	public override void onStateEnter()
	{
		BattleInfo info = BattleGlobal.Core.battleInfo;
		if ( BattleGlobal.Core.getPlayer(BattleGlobal.MyPlayerId).curSummoningCount < BattleConsts.DEFAULT_MAX_SUMMONING_COUNT_PER_TURN )
		{
			info.isSummoningOpAvailabel = true;
		}
		Chessboard.addClickEventHandler(this.onCellClick);
	}

	public override void onStateExit()
	{
		BattleInfo info = BattleGlobal.Core.battleInfo;
		info.isSummoningOpAvailabel = false;
		Chessboard.removeClickEventHandler(this.onCellClick);
	}

	public override void update()
	{
		
	}

	private void onCellClick(GameObject go)
	{
		Cell cell = go.GetComponent<Cell>();
		if ( cell != null )
		{
			BattleGlobal.SelectedCell = cell;
			if ( cell.UnitOnCell != null )
			{
				BattleInfo info = BattleGlobal.Core.battleInfo;
				info.unitSelected = cell.UnitOnCell;
				this._fsm.setState(BattleConsts.BattleState.MainPhase_SelectUnitAction);
			}
		}
	}
}
