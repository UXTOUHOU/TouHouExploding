using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MainPhaseCounterAttackState : BattleStateBase
{
    private const int STATE_CHECK_COUNTER_ATTACK = 1;
    private const int STATE_WAITING_PRE_COUNTER_ATTACK_TIMING_PROCESSING = 2;
    private const int STATE_COUNTER_ATTACK = 3;
    private const int STATE_WAITING_TIMING_PROCESSING = 4;

    private Unit _attacker;
    private Unit _defender;

    private int _curState;
    private int _nextState;

    public MainPhaseCounterAttackState(IFSM fsm)
        :base(fsm)
    {
        this._fsm = fsm;
    }

    public override void onStateEnter()
    {
        Debug.Log("Enter Counter Attack State!");
        BattleInfo info = BattleGlobal.Core.battleInfo;
        this._attacker = info.attacker;
        this._defender = info.defender;
        this._curState = 0;
        this._nextState = STATE_CHECK_COUNTER_ATTACK;
    }

    public override void onStateExit()
    {
        this._attacker = null;
        this._defender = null;
    }

    public override void update()
    {
        if ( this._curState != this._nextState )
        {
            this.onStateExit(this._curState);
            this._curState = this._nextState;
            this.onStateEnter(this._curState);
        }
        this.onStateUpdate();
    }

    private void onStateEnter(int stateId)
    {
        switch (stateId)
        {
            case STATE_CHECK_COUNTER_ATTACK:
                this.checkCounterAttack();
                break;
            case STATE_COUNTER_ATTACK:
                // 造成伤害结果
                DamageResult damageResult = new DamageResult();
                damageResult.attacker = this._defender;
                damageResult.victim = this._attacker;
                damageResult.damageReason = BattleConsts.DAMAGE_REASON_COUNTER_ATTACK;
                damageResult.physicalDamage = this._defender.UnitAttribute.attack;
                ProcessManager.getInstance().addResult(damageResult);
                ProcessManager.getInstance().startProcess();
                this._nextState = STATE_WAITING_TIMING_PROCESSING;
                break;
        }
    }

    private void onStateExit(int stateId)
    {

    }

    private void onStateUpdate()
    {
        switch (this._curState)
        {
            case STATE_CHECK_COUNTER_ATTACK:
                break;
            case STATE_WAITING_PRE_COUNTER_ATTACK_TIMING_PROCESSING:
                if (BattleGlobal.Core.battleInfo.isProcessingComplete)
                {
                    this._nextState = STATE_COUNTER_ATTACK;
                }
                break;
            case STATE_WAITING_TIMING_PROCESSING:
                if (BattleGlobal.Core.battleInfo.isProcessingComplete)
                {
                    this._fsm.setState(BattleConsts.MainPhaseSubState_Idle);
                }
                break;
        }
    }

    private void checkCounterAttack()
    {
        if (!this._defender.canCounterAttack())
        {
            this._fsm.setState(BattleConsts.MainPhaseSubState_Idle);
            return;
        }
        bool isInAttackRange = BattleFieldsUntils.isInAttackRange(this._defender, this._attacker);
        if (isInAttackRange)
        {
            // 触发反击事件
            EventVOBase vo = BattleObjectFactory.createEventVO(BattleConsts.CODE_PRE_COUNTER_ATTACK);
            vo.setProperty(BattleConsts.PROPERTY_ATTACK_ATTACKER, this._attacker);
            vo.setProperty(BattleConsts.PROPERTY_ATTACK_DEFENDER, this._defender);
            BattleEventBase evt = BattleObjectFactory.createBattleEvent(BattleConsts.CODE_PRE_COUNTER_ATTACK, vo);
            ProcessManager.getInstance().raiseEvent(evt);
            ProcessManager.getInstance().startProcess();
            this._nextState = STATE_WAITING_PRE_COUNTER_ATTACK_TIMING_PROCESSING;
        }
    }
}
