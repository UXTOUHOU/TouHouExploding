using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UnitAttackAnnounceProperties : IBattleProperties
{
    public int bPointCostExtra;
    public int bPointCostLimit;


    public void addPropertyValue(BattleConsts.Property propId, object value)
    {
        switch ( propId )
        {
            case BattleConsts.Property.BPointCostExtra:
                this.bPointCostExtra += (int)value;
                break;
        }
    }

    public BattleConsts.Code getCode()
    {
        throw new NotImplementedException();
    }

    public object getProperty(BattleConsts.Property propId)
    {
        throw new NotImplementedException();
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
