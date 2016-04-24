using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 回合开始阶段
/// </summary>
public class TurnStartPhaseState : BattleStateBase
{
    public TurnStartPhaseState(IFSM fsm)
        :base(fsm)
    {

    }

    public override void onStateEnter()
    {
        BattleGlobal.Core.getPlayer(BattleGlobal.MyPlayerId).resetBPoint();
    }

    public override void onStateExit()
    {
    }

    public override void update()
    {
        BattleStateManager.getInstance().setState(BattleConsts.BattleState_StandbyPhase);
    }
}