using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class StandbyPhaseState : BattleStateBase
{
    public StandbyPhaseState(IFSM fsm)
        :base(fsm)
    {

    }

    public override void onStateEnter()
    {
        
    }

    public override void onStateExit()
    {

    }

    public override void update()
    {
		BattleStateManager.getInstance().setState(BattleConsts.BattleState.MainPhase);
    }
}