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

    public override void setProperty(BattleConsts.Property propId, object value)
    {
        switch (propId)
        {
            case BattleConsts.Property.DamageAttacker:
                this.attacker = (Unit)value;
                break;
            case BattleConsts.Property.DamageVictim:
                this.victim = (Unit)value;
                break;
            case BattleConsts.Property.CalcPhysicalDamage:
                this.phycicsDamage = (int)value;
                break;
            case BattleConsts.Property.CalcSpellDamage:
                this.spellDamage = (int)value;
                break;
            case BattleConsts.Property.CalcHpRemoval:
                this.hpRemoval = (int)value;
                break;
            case BattleConsts.Property.DamageReason:
                this.damageReason = (int)value;
                break;
        }
    }

    public override object getProperty(BattleConsts.Property propId)
    {
        switch (propId)
        {
            case BattleConsts.Property.DamageAttacker:
                return this.attacker;
            case BattleConsts.Property.DamageVictim:
                return this.victim;
            case BattleConsts.Property.CalcPhysicalDamage:
                return this.phycicsDamage;
            case BattleConsts.Property.CalcSpellDamage:
                return this.spellDamage;
            case BattleConsts.Property.CalcHpRemoval:
                return this.hpRemoval;
            case BattleConsts.Property.DamageReason:
                return this.damageReason;
            default:
                throw new NotImplementedException();
        }
    }

    public override BattleConsts.Code getEventCode()
    {
        return BattleConsts.Code.TakeDamage;
    }
}

