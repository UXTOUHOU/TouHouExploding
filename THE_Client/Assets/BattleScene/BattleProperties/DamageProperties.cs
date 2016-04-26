using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class DamageProperties : IBattleProperties
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

    private BattleConsts.Code _code;

    public DamageProperties()
    {
        //this._code = BattleConsts.CODE_PRE_DAMAGE;
        this.physicalDamage = 0;
        this.physicalDamageBaseOutgoing = 0;
        this.physicalDamagePercentage = 0;
        this.physicalDamageExtraOutgoing = 0;
        this.spellDamage = 0;
        this.spellDamageBaseOutgoing = 0;
        this.spellDamagePercentage = 0;
        this.spellDamageExtraOutgoing = 0;
        this.totalDamageReduction = 0;
        this.physicalDamageReduction = 0;
        this.spellDamage = 0;
    }

    //public 
    public void addPropertyValue(BattleConsts.Property propId, object value)
    {
        switch (propId)
        {
            case BattleConsts.Property.PhysicalDamage:
                this.physicalDamage += (int)value;
                break;
            case BattleConsts.Property.PhysicalDamageBaseOutgoing:
                this.physicalDamageBaseOutgoing += (int)value;
                break;
            case BattleConsts.Property.PhysicalDamagePercentage:
                this.physicalDamageExtraOutgoing += (int)value;
                break;
            case BattleConsts.Property.PhysicalDamageExtraOutgoing:
                this.physicalDamageExtraOutgoing += (int)value;
                break;
            case BattleConsts.Property.SpellDamage:
                this.spellDamage += (int)value;
                break;
            case BattleConsts.Property.SpellDamageBaseOutgoing:
                this.spellDamageBaseOutgoing += (int)value;
                break;
            case BattleConsts.Property.SpellDamagePercentage:
                this.spellDamagePercentage += (int)value;
                break;
            case BattleConsts.Property.SpellDamageExtraOutgoing:
                this.spellDamageExtraOutgoing += (int)value;
                break;
            case BattleConsts.Property.TotalDamageReduction:
                this.totalDamageReduction += (int)value;
                break;
            case BattleConsts.Property.PhysicalDamageReduction:
                this.physicalDamageReduction += (int)value;
                break;
            case BattleConsts.Property.SpellDamageReduction:
                this.spellDamageReduction += (int)value;
                break;
            default:
                throw new NotImplementedException();
        }
    }

    public BattleConsts.Code getCode()
    {
        return this._code;
    }

    public object getProperty(BattleConsts.Property propId)
    {
        switch (propId)
        {
            case BattleConsts.Property.AttackAttacker:
                return this.attacker;
            case BattleConsts.Property.DamageVictim:
                return this.victim;
            case BattleConsts.Property.PhysicalDamage:
                return this.physicalDamage;
            case BattleConsts.Property.PhysicalDamageBaseOutgoing:
                return this.physicalDamageBaseOutgoing;
            case BattleConsts.Property.PhysicalDamagePercentage:
                return this.physicalDamageExtraOutgoing;
            case BattleConsts.Property.PhysicalDamageExtraOutgoing:
                return this.physicalDamageExtraOutgoing;
            case BattleConsts.Property.SpellDamage:
                return this.spellDamage;
            case BattleConsts.Property.SpellDamageBaseOutgoing:
                return this.spellDamageBaseOutgoing;
            case BattleConsts.Property.SpellDamagePercentage:
                return this.spellDamagePercentage;
            case BattleConsts.Property.SpellDamageExtraOutgoing:
                return this.spellDamageExtraOutgoing;
            case BattleConsts.Property.TotalDamageReduction:
                return this.totalDamageReduction;
            case BattleConsts.Property.PhysicalDamageReduction:
                return this.physicalDamageReduction;
            case BattleConsts.Property.SpellDamageReduction:
                return this.spellDamageReduction;
            default:
                throw new NotImplementedException();
        }
    }

    public void setProperty(BattleConsts.Property propId, object value)
    {
        throw new NotImplementedException();
    }

    public IBattleVO clone()
    {
        return null;
    }
}

