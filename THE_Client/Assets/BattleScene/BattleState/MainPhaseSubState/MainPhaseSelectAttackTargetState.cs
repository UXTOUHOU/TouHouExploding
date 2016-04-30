using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MainPhaseSelectAttackTargetState : BattleStateBase
{
    private const int STATE_WAITING_SELECTING = 1;
    private const int STATE_TIMING_PROCESSING = 2;

    private Unit _curUnit;
    private Cell _targetCell;
    private Unit _targetUnit;

    private int _curState;
    private int _nextState;

    private int _step;

    public MainPhaseSelectAttackTargetState(IFSM fsm)
        :base(fsm)
    {
        this._fsm = fsm;
    }

    public override void onStateEnter()
    {
        Debug.Log("Enter Select Attack Target State!");
        this._curState = 0;
        BattleInfo info = BattleGlobal.Core.battleInfo;
        this._curUnit = info.unitSelected;
        this._nextState = STATE_WAITING_SELECTING;
    }

    public override void onStateExit()
    {
        this._curUnit = null;
        this._targetCell = null;
        this._targetUnit = null;
    }

    public override void update()
    {
        if (this._curState != this._nextState)
        {
            this.onStateExit(this._curState);
            this._curState = this._nextState;
            this.onStateEnter(this._curState);
        }
        switch (this._curState)
        {
            case STATE_WAITING_SELECTING:
                break;
            case STATE_TIMING_PROCESSING:
                if (BattleGlobal.Core.battleInfo.isProcessingComplete)
                {
                    // 判断是否进入伤害结算部分
                    this._fsm.setState(BattleConsts.BattleState.MainPhase_UnitAttack);
                }
                break;
        }
    }

    private void onStateExit(int stateId)
    {
        switch (stateId)
        {
            case STATE_WAITING_SELECTING:
                Chessboard.removeClickEventHandler(this.cellClickHandler);
                Chessboard.activeRangeShow(false);
                break;
        }
    }

    private void onStateEnter(int stateId)
    {
        switch (stateId)
        {
            case STATE_WAITING_SELECTING:
                Chessboard.addClickEventHandler(this.cellClickHandler);
                // 显示攻击范围
                Chessboard.showRangeByManhattanDis(this._curUnit.row, this._curUnit.col, this._curUnit.UnitAttribute.minAttackRangeCurrent, this._curUnit.UnitAttribute.maxAttackRangeCurrent);
                break;
            case STATE_TIMING_PROCESSING:
                EventVOBase vo = BattleObjectFactory.createEventVO(BattleConsts.Code.FlagAttackTarget);
                vo.setProperty(BattleConsts.Property.AttackAttacker, this._curUnit);
                vo.setProperty(BattleConsts.Property.AttackDefender, this._targetUnit);
                BattleEventBase evt = BattleObjectFactory.createBattleEvent(BattleConsts.Code.FlagAttackTarget, vo);
                ProcessManager.getInstance().startProcess();
                break;
        }
    }

    private void cellClickHandler(GameObject go)
    {
        Cell cell = go.GetComponent<Cell>();
        if (cell.UnitOnCell != null)
        {
            //todo 攻击范围效果判断
            SelectAttackTargetProperties props = new SelectAttackTargetProperties();
            props.attacker = this._curUnit;
            props.defender = cell.UnitOnCell;
            this._curUnit.applyEffects(props);
            int manhattanDis = BattleFieldsUntils.getManhattanDis(props.attacker, props.defender);
            int minAttackDis = props.attacker.UnitAttribute.minAttackRangeCurrent + props.minDisExtra;
            int maxAttackDis = props.attacker.UnitAttribute.maxAttackRangeCurrent + props.maxDisExtra;
            if (manhattanDis >= minAttackDis && manhattanDis <= maxAttackDis)
            {
                this._targetCell = cell;
                this._targetUnit = cell.UnitOnCell;
                BattleInfo info = BattleGlobal.Core.battleInfo;
                info.attacker = this._curUnit;
                info.defender = this._targetUnit;
                // 进入时点
                this._nextState = STATE_TIMING_PROCESSING;
            }
        }
    }
}
