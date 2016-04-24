using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 处理事件-结果的状态
/// </summary>
public class ProcessingState : BattleStateBase
{
    public ProcessingState(IFSM fsm)
        : base(fsm)
    {

    }

    public override void onStateEnter()
    {
        ProcessManager.getInstance().startProcess();
    }

    public override void onStateExit()
    {

    }

    public override void update()
    {
        BattleInfo info = BattleGlobal.Core.battleInfo;
        if ( info.isProcessingComplete )
        {
            this._fsm.setState(info.nextState);
        }
    }
}