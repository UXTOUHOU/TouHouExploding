using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SummonUnitSuccessEventVO : EventVOBase
{
    public Unit summoningUnit;
    public int summoningPos;
    public int summoningReason;

    public override void setProperty(BattleConsts.Property propId, object value)
    {
        switch (propId)
        {
            case BattleConsts.Property.SummoningUnit:
                this.summoningUnit = (Unit)value;
                break;
            case BattleConsts.Property.SummoningPos:
                this.summoningPos = (int)value;
                break;
            case BattleConsts.Property.SummoningReason:
                this.summoningReason = (int)value;
                break;
        }
    }

    public override object getProperty(BattleConsts.Property propId)
    {
        switch (propId)
        {
            case BattleConsts.Property.SummoningUnit:
                return this.summoningUnit;
            case BattleConsts.Property.SummoningPos:
                return this.summoningPos;
            case BattleConsts.Property.SummoningReason:
                return this.summoningReason;
            default:
                throw new NotImplementedException();
        }
    }

    public override BattleConsts.Code getEventCode()
    {
        return BattleConsts.Code.SummonUnitSuccess;
    }
}

