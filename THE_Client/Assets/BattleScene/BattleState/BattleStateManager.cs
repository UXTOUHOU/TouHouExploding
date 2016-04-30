using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading;

public class BattleStateManager : IFSM
{
	private static BattleStateManager _instance;

	public static BattleStateManager getInstance()
	{
		if (_instance == null)
		{
			_instance = new BattleStateManager();
		}
		return _instance;
	}

	/// <summary>
	/// 状态集合
	/// </summary>
	private Dictionary<BattleConsts.BattleState, BattleStateBase> _states;
	/// <summary>
	/// 当前状态
	/// </summary>
	private BattleConsts.BattleState _curStateId;
	/// <summary>
	/// 下一个状态
	/// </summary>
	private BattleConsts.BattleState _nextStateId;
	/// <summary>
	/// 当前正在运行的状态
	/// </summary>
	private BattleStateBase _curState;

	public BattleStateManager()
	{
		this.initStates();
	}

	public void initStates()
	{
		this._curStateId = 0;
		this._states = new Dictionary<BattleConsts.BattleState, BattleStateBase>();
		this._states.Add(BattleConsts.BattleState.InitGame, new InitGameState(this));
		this._states.Add(BattleConsts.BattleState.TurnStartPhase, new TurnStartPhaseState(this));
		this._states.Add(BattleConsts.BattleState.StandbyPhase, new StandbyPhaseState(this));
		this._states.Add(BattleConsts.BattleState.MainPhase_Idle, new MainPhaseIdleState(this));
		this._states.Add(BattleConsts.BattleState.MainPhase_SelectUnitAction, new MainPhaseSelectUnitActionState(this));
		this._states.Add(BattleConsts.BattleState.MainPhase_SelectMovePath, new MainPhaseSelectMovePathState(this));
		this._states.Add(BattleConsts.BattleState.MainPhase_MoveUnit, new MainPhaseMoveUnitState(this));
		this._states.Add(BattleConsts.BattleState.MainPhase_SelectAttackTarget, new MainPhaseSelectAttackTargetState(this));
		this._states.Add(BattleConsts.BattleState.MainPhase_UnitAttack, new MainPhaseUnitAttackState(this));
		this._states.Add(BattleConsts.BattleState.MainPhase_CounterAttack, new MainPhaseCounterAttackState(this));
		this._states.Add(BattleConsts.BattleState.MainPhase_SummoningUnit, new MainPhaseSummonUnitState(this));
		this._states.Add(BattleConsts.BattleState.Processing, new ProcessingState(this));
	}

	public void setState(BattleConsts.BattleState stateId)
	{
		this._nextStateId = stateId;
	}

	public void update()
	{
		if (this._curStateId != this._nextStateId)
		{
			Debug.Log("cur state " + this._curStateId + "  ----> next state " + this._nextStateId);
			if (this._curState != null)
			{
				this._curState.onStateExit();
			}
			this._states.TryGetValue(this._nextStateId, out this._curState);
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
}
