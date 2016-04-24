using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class CheckBPointCostByAttackProperties : IBattleProperties
{
    public int playerId;

    public int unitId;

    public int costBase;

    public int costBaseExtra;

    public int costCurrent
    {
        get { return this.costBase + this.costBaseExtra; }
    }

    private int _code;

    public CheckBPointCostByAttackProperties()
    {
        this._code = BattleConsts.CODE_CHECK_BPOINT_COST_BY_ATTACK;
    }

    public void addPropertyValue(int propId, object value)
    {
        switch (propId)
        {
            case BattleConsts.PROPERTY_BPOINT_COST_BASE:
                this.costBase += (int)value;
                break;
            case BattleConsts.PROPERTY_BPOINT_COST_EXTRA:
                this.costBaseExtra += (int)value;
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
        switch ( propId )
        {
            case BattleConsts.PROPERTY_PLAYER_ID:
                return this.playerId;
            case BattleConsts.PROPERTY_UNIT_ID:
                return this.unitId;
            case BattleConsts.PROPERTY_BPOINT_COST_BASE:
                return this.costBase;
            case BattleConsts.PROPERTY_BPOINT_COST_EXTRA:
                return this.costBaseExtra;
            case BattleConsts.PROPERTY_BPOINT_COST_CURRENT:
                return this.costCurrent;
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

