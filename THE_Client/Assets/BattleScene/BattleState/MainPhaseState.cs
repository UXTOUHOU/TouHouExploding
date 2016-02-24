using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MainPhaseState : BattleStateBase
{
    public MainPhaseState(IFSM fsm)
        :base(fsm)
    {

    }

    public override void onStateEnter()
    {
        //添加点击事件
        BattleSceneMain.getInstance().chessboard.addClickEventHandler(this.cellClickEventHandler);

        OperationManager.getInstance().setOperation(BattleConsts.CellOp_Idle);
    }

    public override void onStateExit()
    {

    }

    public override void update()
    {

    }

    public void cellClickEventHandler(GameObject go)
    {
        Cell cell = go.GetComponent<Cell>();
    }
}