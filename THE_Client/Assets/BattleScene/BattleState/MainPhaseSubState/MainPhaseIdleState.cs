using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MainPhaseIdleState : IState
{
    private IFSM _fsm;

    public MainPhaseIdleState(IFSM fsm)
    {
        this._fsm = fsm;
    }

    public void onStateEnter()
    {
        BattleGlobal.Core.chessboard.addClickEventHandler(this.onCellClick);
    }

    public void onStateExit()
    {
        BattleGlobal.Core.chessboard.removeClickEventHandler(this.onCellClick);
    }

    public void update()
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
                this._fsm.setState(BattleConsts.MainPhaseSubState_SelectUnitAction);
            }
        }
    }
}
