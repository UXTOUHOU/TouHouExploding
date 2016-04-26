using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class HurtVO : IBattleVO
{
    public Unit attacker;
    public Unit victim;
    public int phycicsDamage;
    public int spellDamage;
    public int hpRemoval;
    public BattleConsts.DamageReason damageReason;

    public void setProperty(BattleConsts.Property propId, object value)
    {
        switch ( propId )
        {
            case BattleConsts.Property.DamageAttacker:
                this.attacker = (Unit)value;
                break;
        }
    }

    public object getProperty(BattleConsts.Property propId)
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

    public IBattleVO clone()
    {
        HurtVO vo = new HurtVO();
        vo.attacker = this.attacker;
        vo.victim = this.victim;
        vo.phycicsDamage = this.phycicsDamage;
        vo.spellDamage = this.spellDamage;
        vo.hpRemoval = this.hpRemoval;
        vo.damageReason = this.damageReason;
        return vo;
    }
}

