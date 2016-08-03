using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MainPhaseState : BattleStateBase, IFSM
{
    private Dictionary<BattleConsts.BattleState, IState> _subStates;

    private BattleConsts.BattleState _curStateId;

    private BattleConsts.BattleState _nextStateId;

    private IState _curState;

    public MainPhaseState(IFSM fsm)
        :base(fsm)
    {
        this.initStates();
    }

    public override void onStateEnter()
    {
        this.setState(BattleConsts.BattleState.MainPhase_Idle);
    }

    public override void onStateExit()
    {

    }

    public override void update()
    {
        if (this._curStateId != this._nextStateId)
        {
            Debug.Log("MainPhase : cur state " + this._curStateId + "  ----> next state " + this._nextStateId);
            if (this._curState != null)
            {
                this._curState.onStateExit();
            }
            this._subStates.TryGetValue(this._nextStateId, out this._curState);
            this._curStateId = this._nextStateId;
            if (this._curState != null)
            {
                this._curState.onStateEnter();
            }
        }
        if (this._curState != null)
        {
            this._curState.update();
        }
    }

    public void initStates()
    {
        this._subStates = new Dictionary<BattleConsts.BattleState, IState>();
        this._subStates.Add(BattleConsts.BattleState.MainPhase_Idle, new MainPhaseIdleState(this));
        this._subStates.Add(BattleConsts.BattleState.MainPhase_SelectUnitAction, new MainPhaseSelectUnitActionState(this));
        this._subStates.Add(BattleConsts.BattleState.MainPhase_SelectMovePath, new MainPhaseSelectMovePathState(this));
        this._subStates.Add(BattleConsts.BattleState.MainPhase_MoveUnit, new MainPhaseMoveUnitState(this));
        this._subStates.Add(BattleConsts.BattleState.MainPhase_SelectAttackTarget, new MainPhaseSelectAttackTargetState(this));
        this._subStates.Add(BattleConsts.BattleState.MainPhase_UnitAttack, new MainPhaseUnitAttackState(this));
        this._subStates.Add(BattleConsts.BattleState.MainPhase_CounterAttack, new MainPhaseCounterAttackState(this));
    }

    public void setState(BattleConsts.BattleState stateId)
    {
        this._nextStateId = stateId;
    }
}