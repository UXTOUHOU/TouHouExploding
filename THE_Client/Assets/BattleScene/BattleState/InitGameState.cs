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
        UnitManager.getInatance().init();
        BattleGlobal.Core.chessboard.init();
        OperationManager.getInstance().init();
        PopUpManager.getInstance().init();
    }

    public override void onStateExit()
    {
        BattleGlobal.Core.startGame();
    }

    public override void update()
    {
        BattleStateManager.getInstance().setState(BattleConsts.BattleState_TurnStartPhase);
    }
}