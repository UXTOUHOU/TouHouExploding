using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class SelectAttackTargetProperties : IBattleProperties
{
    public Unit attacker;

    public Unit defender;

    public int minDisExtra;

    public int maxDisExtra;

    private int _code;

    public SelectAttackTargetProperties()
    {
        this._code = BattleConsts.CODE_SELECT_ATTACK_TARGET;
        this.minDisExtra = 0;
        this.maxDisExtra = 0;
    }

    //public 
    public void addPropertyValue(int propId, object value)
    {
        switch ( propId )
        {
            case BattleConsts.PROPERTY_MIN_ATTACK_DIS_EXTRA:
                this.minDisExtra += (int)value;
                break;
            case BattleConsts.PROPERTY_MAX_ATTACK_DIS_EXTRA:
                this.maxDisExtra += (int)value;
                break;
            default:
                throw new NotImplementedException();
        }
    }

    public int getCode()
    {
        return this._code;
    }

    public object getProperty(int propId)
    {
        switch (propId)
        {
            case BattleConsts.PROPERTY_ATTACK_ATTACKER:
                return this.attacker;
            case BattleConsts.PROPERTY_ATTACK_DEFENDER:
                return this.defender;
            case BattleConsts.PROPERTY_MIN_ATTACK_DIS_EXTRA:
                return this.minDisExtra;
            case BattleConsts.PROPERTY_MAX_ATTACK_DIS_EXTRA:
                return this.maxDisExtra;
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

