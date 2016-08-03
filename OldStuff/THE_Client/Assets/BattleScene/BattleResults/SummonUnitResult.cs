using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SummonUnitResult : IBattleResult
{
    /// <summary>
    /// 召唤的单位id
    /// </summary>
    public string unitId;
    /// <summary>
    /// 召唤的位置
    /// </summary>
    public int summoningPos;

    public int owner;

    public int controller;

    public void execute()
    {
        Unit unit = new Unit(unitId);
        if ( unit.summon(this.summoningPos,this.owner,this.controller) == BattleConsts.UNIT_ACTION_SUCCESS )
        {

        }
    }

    public int getType()
    {
        throw new NotImplementedException();
    }
}

