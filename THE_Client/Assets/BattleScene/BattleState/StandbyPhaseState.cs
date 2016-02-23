using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class StandbyPhaseState : IState
{
    public StandbyPhaseState()
    {

    }

    public void onStateEnter()
    {
        
    }

    public void onStateExit()
    {

    }

    public void update()
    {
        BattleStateManager.getInstance().setState(BattleConsts.BattleState_MainPhase);
    }
}