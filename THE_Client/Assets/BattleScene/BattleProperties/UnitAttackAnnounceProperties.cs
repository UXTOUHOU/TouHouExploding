using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UnitAttackAnnounceProperties : IBattleProperties
{
    public int bPointCostExtra;
    public int bPointCostLimit;


    public void addPropertyValue(int propId, object value)
    {
        switch ( propId )
        {
            case BattleConsts.PROPERTY_BPOINT_COST_EXTRA:
                this.bPointCostExtra += (int)value;
                break;
        }
    }

    public int getCode()
    {
        throw new NotImplementedException();
    }

    public object getProperty(int propId)
    {
        throw new NotImplementedException();
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
