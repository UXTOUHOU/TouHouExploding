using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 回合开始阶段
/// </summary>
public class TurnStartPhaseState : IState
{
    public TurnStartPhaseState()
    {

    }

    public void onStateEnter()
    {
        //BattleSceneMain.getInstance().chessboard.init();
    }

    public void onStateExit()
    {
    }

    public void update()
    {
        BattleStateManager.getInstance().setState(BattleConsts.BattleState_StandbyPhase);
    }
}