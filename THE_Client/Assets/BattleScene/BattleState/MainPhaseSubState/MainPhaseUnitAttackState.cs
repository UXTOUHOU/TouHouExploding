using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MainPhaseUnitAttackState : IState
{
    private const int STATE_ATTACK = 1;
    private const int STATE_WAITING_TIMING_PROCESSING = 2;

    private IFSM _fsm;

    private Unit _attacker;
    private Unit _defender;

    public MainPhaseUnitAttackState(IFSM fsm)
    {
        this._fsm = fsm;
    }

    public void onStateEnter()
    {
        Debug.Log("Enter Attack State!");
        BattleInfo info = BattleGlobal.Core.battleInfo;
        this._attacker = info.attacker;
        this._defender = info.defender;
        this._attacker.doAttack();
        // 造成伤害结果
        DamageResult damageResult = new DamageResult();
        damageResult.attacker = this._attacker;
        damageResult.victim = this._defender;
        damageResult.damageReason = BattleConsts.DAMAGE_REASON_ATTACK;
        damageResult.physicalDamage = this._attacker.UnitAttribute.attack;
        ProcessManager.getInstance().addResult(damageResult);
        ProcessManager.getInstance().startProcess();
    }

    public void onStateExit()
    {
        this._attacker = null;
        this._defender = null;
    }

    public void update()
    {
        if ( BattleGlobal.Core.battleInfo.isProcessingComplete )
        {
            this._fsm.setState(BattleConsts.MainPhaseSubState_CounterAttack);
        }
    }
}
