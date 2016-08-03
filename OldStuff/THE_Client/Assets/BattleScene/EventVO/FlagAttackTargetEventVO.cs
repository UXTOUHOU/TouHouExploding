using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class FlagAttackTargetEventVO : EventVOBase
{
    public Unit attacker;
    public Unit defender;

    public override void setProperty(BattleConsts.Property propId, object value)
    {
        switch (propId)
        {
            case BattleConsts.Property.AttackAttacker:
                this.attacker = (Unit)value;
                break;
            case BattleConsts.Property.AttackDefender:
                this.defender = (Unit)value;
                break;
        }
    }

    public override object getProperty(BattleConsts.Property propId)
    {
        switch (propId)
        {
            case BattleConsts.Property.AttackAttacker:
                return this.attacker;
            case BattleConsts.Property.AttackDefender:
                return this.defender;
            default:
                throw new NotImplementedException();
        }
    }

    public override BattleConsts.Code getEventCode()
    {
        return BattleConsts.Code.FlagAttackTarget;
    }
}

