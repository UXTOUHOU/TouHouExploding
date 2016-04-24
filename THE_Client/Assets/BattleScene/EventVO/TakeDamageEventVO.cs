using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TakeDamageEventVO : EventVOBase
{
    public Unit attacker;
    public Unit victim;
    public int phycicsDamage;
    public int spellDamage;
    public int hpRemoval;
    public int damageReason;

    public override void setProperty(int propId, object value)
    {
        switch (propId)
        {
            case BattleConsts.PROPERTY_DAMAGE_ATTACKER:
                this.attacker = (Unit)value;
                break;
            case BattleConsts.PROPERTY_DAMAGE_VICTIM:
                this.victim = (Unit)value;
                break;
            case BattleConsts.PROPERTY_CALC_PHYSICAL_DAMAGE:
                this.phycicsDamage = (int)value;
                break;
            case BattleConsts.PROPERTY_CALC_SPELL_DAMAGE:
                this.spellDamage = (int)value;
                break;
            case BattleConsts.PROPERTY_CALC_HP_REMOVAL:
                this.hpRemoval = (int)value;
                break;
            case BattleConsts.PROPERTY_DAMAGE_REASON:
                this.damageReason = (int)value;
                break;
        }
    }

    public override object getProperty(int propId)
    {
        switch (propId)
        {
            case BattleConsts.PROPERTY_DAMAGE_ATTACKER:
                return this.attacker;
            case BattleConsts.PROPERTY_DAMAGE_VICTIM:
                return this.victim;
            case BattleConsts.PROPERTY_CALC_PHYSICAL_DAMAGE:
                return this.phycicsDamage;
            case BattleConsts.PROPERTY_CALC_SPELL_DAMAGE:
                return this.spellDamage;
            case BattleConsts.PROPERTY_CALC_HP_REMOVAL:
                return this.hpRemoval;
            case BattleConsts.PROPERTY_DAMAGE_REASON:
                return this.damageReason;
            default:
                throw new NotImplementedException();
        }
    }

    public override int getEventCode()
    {
        return BattleConsts.CODE_TAKE_DAMAGE;
    }
}

