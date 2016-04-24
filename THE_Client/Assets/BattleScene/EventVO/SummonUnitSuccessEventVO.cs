using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SummonUnitSuccessEventVO : EventVOBase
{
    public Unit summoningUnit;
    public int summoningPos;
    public int summoningReason;

    public override void setProperty(int propId, object value)
    {
        switch (propId)
        {
            case BattleConsts.PROPERTY_SUMMONING_UNIT:
                this.summoningUnit = (Unit)value;
                break;
            case BattleConsts.PROPERTY_SUMMONING_POS:
                this.summoningPos = (int)value;
                break;
            case BattleConsts.PROPERTY_SUMMONING_REASON:
                this.summoningReason = (int)value;
                break;
        }
    }

    public override object getProperty(int propId)
    {
        switch (propId)
        {
            case BattleConsts.PROPERTY_SUMMONING_UNIT:
                return this.summoningUnit;
            case BattleConsts.PROPERTY_SUMMONING_POS:
                return this.summoningPos;
            case BattleConsts.PROPERTY_SUMMONING_REASON:
                return this.summoningReason;
            default:
                throw new NotImplementedException();
        }
    }

    public override int getEventCode()
    {
        return BattleConsts.CODE_SUMMON_UNIT_SUCCESS;
    }
}

