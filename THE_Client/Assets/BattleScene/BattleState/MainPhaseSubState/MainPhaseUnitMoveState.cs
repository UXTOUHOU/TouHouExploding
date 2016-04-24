using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MainPhaseMoveUnitState : BattleStateBase, ICommand
{
    private Unit _curUnit;
    private int[] _movePaths;
    /// <summary>
    /// 当前路径位置索引
    /// </summary>
    private int _curPathIndex;
    /// <summary>
    ///  当前是否踏上新的一格
    /// </summary>
    private bool _stepOnCell;
    /// <summary>
    /// 移动是否完成
    /// </summary>
    private bool _moveComplete;

    public MainPhaseMoveUnitState(IFSM fsm)
        :base(fsm)
    {
    }

    public override void onStateEnter()
    {
        this._stepOnCell = false;
        this._moveComplete = false;
        CommandManager.getInstance().addCommand(BattleConsts.CMD_UnitStepOnCell, this);
        CommandManager.getInstance().addCommand(BattleConsts.CMD_UnitMoveComplete, this);
        this._curUnit = BattleGlobal.UnitToBeOperated;
        this._movePaths = BattleGlobal.MovePaths;
        this._curUnit.startMoving(this._movePaths);
    }

    public override void onStateExit()
    {
        this._curUnit = null;
        this._movePaths = null;
        CommandManager.getInstance().removeCommand(BattleConsts.CMD_UnitStepOnCell, this);
        CommandManager.getInstance().removeCommand(BattleConsts.CMD_UnitMoveComplete, this);
    }

    public override void update()
    {
        if ( this._stepOnCell )
        {
            this._curPathIndex++;
            // 进入新的格子
            // todo : 各种地形，刷新buff的判定
            // 移动至下一格
            this._stepOnCell = false;
            this._curUnit.moveToNextCell();
        }
        if ( this._moveComplete )
        {
            // todo : 各种地形，刷新buff的判定
            // 进入空闲状态
            this._fsm.setState(BattleConsts.MainPhaseSubState_Idle);
        }
    }

    public void recvCommand(int cmd,object[] args)
    {
        switch (cmd)
        {
            case BattleConsts.CMD_UnitStepOnCell:
                this._stepOnCell = true;
                break;
            case BattleConsts.CMD_UnitMoveComplete:
                this._moveComplete = true;
                break;
            default:
                break;
        }
    }

    private void unitStepOnCellHandler(object[] args)
    {
        // todo: 机动处理等
        // 继续移动到下一格
    }
}
