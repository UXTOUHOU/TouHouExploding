using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class CalcDamageAttackerProperties : IBattleProperties
{
    public Unit attacker;

    public Unit victim;

    public int damageReason;

    public int physicalDamage;

    public int physicalDamageBaseOutgoing;

    public int physicalDamagePercentage;

    public int physicalDamageExtraOutgoing;

    public int spellDamage;

    public int spellDamageBaseOutgoing;

    public int spellDamagePercentage;

    public int spellDamageExtraOutgoing;

    public int totalDamageReduction;

    public int physicalDamageReduction;

    public int spellDamageReduction;

    public int hpRemoval;

    public void addPropertyValue(int propId, object value)
    {
        switch (propId)
        {
            case BattleConsts.PROPERTY_PHYSICAL_DAMAGE:
                this.physicalDamage += (int)value;
                break;
            case BattleConsts.PROPERTY_PHYSICAL_DAMAGE_BASE_OUTGOING:
                this.physicalDamageBaseOutgoing += (int)value;
                break;
            case BattleConsts.PROPERTY_PHYSICAL_DAMAGE_PERCENTAGE:
                this.physicalDamageExtraOutgoing += (int)value;
                break;
            case BattleConsts.PROPERTY_PHYSICAL_DAMAGE_EXTRA_OUTGOING:
                this.physicalDamageExtraOutgoing += (int)value;
                break;
            case BattleConsts.PROPERTY_SPELL_DAMAGE:
                this.spellDamage += (int)value;
                break;
            case BattleConsts.PROPERTY_SPELL_DAMAGE_BASE_OUTGOING:
                this.spellDamageBaseOutgoing += (int)value;
                break;
            case BattleConsts.PROPERTY_SPELL_DAMAGE_PERCENTAGE:
                this.spellDamagePercentage += (int)value;
                break;
            case BattleConsts.PROPERTY_SPELL_DAMAGE_EXTRA_OUTGOING:
                this.spellDamageExtraOutgoing += (int)value;
                break;
            case BattleConsts.PROPERTY_TOTAL_DAMAGE_REDUCTION:
                this.totalDamageReduction += (int)value;
                break;
            case BattleConsts.PROPERTY_PHYSICAL_DAMAGE_REDUCTION:
                this.physicalDamageReduction += (int)value;
                break;
            case BattleConsts.PROPERTY_SPELL_DAMAGE_REDUCTION:
                this.spellDamageReduction += (int)value;
                break;
            default:
                throw new NotImplementedException();
        }
    }

    public int getCode()
    {
        return BattleConsts.CODE_CAlC_DAMAGE_ATTACKER;
    }

    public object getProperty(int propId)
    {
        switch (propId)
        {
            case BattleConsts.PROPERTY_DAMAGE_REASON:
                return this.damageReason;
            case BattleConsts.PROPERTY_DAMAGE_ATTACKER:
                return this.attacker;
            case BattleConsts.PROPERTY_DAMAGE_VICTIM:
                return this.victim;
            case BattleConsts.PROPERTY_PHYSICAL_DAMAGE:
                return this.physicalDamage;
            case BattleConsts.PROPERTY_PHYSICAL_DAMAGE_BASE_OUTGOING:
                return this.physicalDamageBaseOutgoing;
            case BattleConsts.PROPERTY_PHYSICAL_DAMAGE_PERCENTAGE:
                return this.physicalDamageExtraOutgoing;
            case BattleConsts.PROPERTY_PHYSICAL_DAMAGE_EXTRA_OUTGOING:
                return this.physicalDamageExtraOutgoing;
            case BattleConsts.PROPERTY_SPELL_DAMAGE:
                return this.spellDamage;
            case BattleConsts.PROPERTY_SPELL_DAMAGE_BASE_OUTGOING:
                return this.spellDamageBaseOutgoing;
            case BattleConsts.PROPERTY_SPELL_DAMAGE_PERCENTAGE:
                return this.spellDamagePercentage;
            case BattleConsts.PROPERTY_SPELL_DAMAGE_EXTRA_OUTGOING:
                return this.spellDamageExtraOutgoing;
            case BattleConsts.PROPERTY_TOTAL_DAMAGE_REDUCTION:
                return this.totalDamageReduction;
            case BattleConsts.PROPERTY_PHYSICAL_DAMAGE_REDUCTION:
                return this.physicalDamageReduction;
            case BattleConsts.PROPERTY_SPELL_DAMAGE_REDUCTION:
                return this.spellDamageReduction;
            case BattleConsts.PROPERTY_HP_REMOVAL:
                return this.hpRemoval;
            default:
                throw new NotImplementedException();
        }
    }

    public void setProperty(int propId, object value)
    {
        throw new NotImplementedException();
    }

    public IBattleVO clone()
    {
        return null;
    }
}

