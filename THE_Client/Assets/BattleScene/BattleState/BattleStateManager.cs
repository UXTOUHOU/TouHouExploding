using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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
    private Dictionary<int, IState> _states;
    /// <summary>
    /// 当前状态
    /// </summary>
    private int _curStateId;
    /// <summary>
    /// 下一个状态
    /// </summary>
    private int _nextStateId;
    /// <summary>
    /// 当前正在运行的状态
    /// </summary>
    private IState _curState;

    public BattleStateManager()
    {
        this.initStates();
    }

    public void initStates()
    {
        this._states = new Dictionary<int, IState>();
        this._states.Add(BattleConsts.BattleState_InitGame, new InitGameState());
    }

    public void setState(int stateId)
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
