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

    private BattleConsts.Code _code;

    public CheckBPointCostByAttackProperties()
    {
        this._code = BattleConsts.Code.CheckBPointCostByAttack;
    }

    public void addPropertyValue(BattleConsts.Property propId, object value)
    {
        switch (propId)
        {
            case BattleConsts.Property.BPointCostBase:
                this.costBase += (int)value;
                break;
            case BattleConsts.Property.BPointCostExtra:
                this.costBaseExtra += (int)value;
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
        switch ( propId )
        {
            case BattleConsts.Property.PlayerId:
                return this.playerId;
            case BattleConsts.Property.UnitId:
                return this.unitId;
            case BattleConsts.Property.BPointCostBase:
                return this.costBase;
            case BattleConsts.Property.BPointCostExtra:
                return this.costBaseExtra;
            case BattleConsts.Property.BPointCostCurrent:
                return this.costCurrent;
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

