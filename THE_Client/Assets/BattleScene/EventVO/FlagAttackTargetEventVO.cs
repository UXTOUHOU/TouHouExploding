using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class FlagAttackTargetEventVO : EventVOBase
{
    public Unit attacker;
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
        return BattleConsts.CODE_FLAG_ATTACK_TARGET;
    }
}

