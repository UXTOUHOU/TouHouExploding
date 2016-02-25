using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MainPhaseSelectMovePathState : IState, ICommand
{
    private IFSM _fsm;

    public MainPhaseSelectMovePathState(IFSM fsm)
    {
        this._fsm = fsm;
    }

    public void onStateEnter()
    {
        OperationManager.getInstance().setOperation(BattleConsts.CellOp_Idle);
        CommandManager.getInstance().addCommand(BattleConsts.CMD_OnCellSelected, this);
    }

    public void onStateExit()
    {
        CommandManager.getInstance().removeCommand(BattleConsts.CMD_OnCellSelected, this);
    }

    public void update()
    {

    }

    public void recvCommand(int cmd, params object[] args)
    {
        switch (cmd)
        {
            case BattleConsts.CMD_OnCellSelected:
                this._fsm.setState(BattleConsts.MainPhaseSubState_SelectUnitAction);
                break;
            default:
                break;
        }
    }
}
