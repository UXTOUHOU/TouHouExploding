using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class PreCounterAttackEventVO : EventVOBase
{
    /// <summary>
    /// 攻击者--被反击的人
    /// </summary>
    public Unit attacker;
    /// <summary>
    /// 防御者--反击者
    /// </summary>
    public Unit defender;

    public override void setProperty(int propId, object value)
    {
        switch (propId)
        {
            case BattleConsts.PROPERTY_ATTACK_ATTACKER:
                this.attacker = (Unit)value;
                break;
            case BattleConsts.PROPERTY_ATTACK_DEFENDER:
                this.defender = (Unit)value;
                break;
        }
    }

    public override object getProperty(int propId)
    {
        switch (propId)
        {
            case BattleConsts.PROPERTY_ATTACK_ATTACKER:
                return this.attacker;
            case BattleConsts.PROPERTY_ATTACK_DEFENDER:
                return this.defender;
            default:
                throw new NotImplementedException();
        }
    }

    public override int getEventCode()
    {
        return BattleConsts.CODE_PRE_COUNTER_ATTACK;
    }
}

