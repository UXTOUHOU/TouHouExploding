using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class InitGameState : BattleStateBase
{
    public InitGameState(IFSM fsm)
        :base(fsm)
    {

    }

    public override void onStateEnter()
    {
        BattleSceneMain.getInstance().chessboard.init();
        OperationManager.getInstance().init();
        PopUpManager.getInstance().init();
    }

    public override void onStateExit()
    {
    }

    public override void update()
    {
        BattleStateManager.getInstance().setState(BattleConsts.BattleState_TurnStartPhase);
    }
}