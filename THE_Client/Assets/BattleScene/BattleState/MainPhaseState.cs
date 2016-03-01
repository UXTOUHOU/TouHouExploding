using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MainPhaseState : BattleStateBase ,IFSM
{
    private Dictionary<int, IState> _subStates;

    private int _curStateId;

    private int _nextStateId;

    private IState _curState;

    public MainPhaseState(IFSM fsm)
        :base(fsm)
    {
        this.initStates();
    }

    public override void onStateEnter()
    {
        this.setState(BattleConsts.MainPhaseSubState_Idle);
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
        this._subStates = new Dictionary<int, IState>();
        this._subStates.Add(BattleConsts.MainPhaseSubState_Idle, new MainPhaseIdleState(this));
        this._subStates.Add(BattleConsts.MainPhaseSubState_SelectUnitAction, new MainPhaseSelectUnitActionState(this));
        this._subStates.Add(BattleConsts.MainPhaseSubState_SelectMovePath, new MainPhaseSelectMovePathState(this));
        this._subStates.Add(BattleConsts.MainPhaseSubState_MoveUnit, new MainPhaseMoveUnitState(this));
    }

    public void setState(int stateId)
    {
        this._nextStateId = stateId;
    }
}