using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BattleStateBase : IState
{
    /// <summary>
    /// 状态机
    /// </summary>
    protected IFSM _fsm;

    public BattleStateBase(IFSM fsm)
    {
        this._fsm = fsm;
    }

    public virtual void onStateEnter()
    {
        
    }

    public virtual void onStateExit()
    {
    }

    public virtual void update()
    {
        
    }
}