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
    public int damageReason;

    public void setProperty(int propId, object value)
    {
        switch ( propId )
        {
            case BattleConsts.PROPERTY_DAMAGE_ATTACKER:
                this.attacker = (Unit)value;
                break;
        }
    }

    public object getProperty(int propId)
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

