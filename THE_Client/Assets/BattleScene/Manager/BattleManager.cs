using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class BattleManager
{
    private static BattleManager _instance;

    public static BattleManager getInstance()
    {
        if ( _instance == null )
        {
            _instance = new BattleManager();
        }
        return _instance;
    }

    public bool checkBPointCostByAttack(int playerId,int unitId,int cost)
    {
        Player player = BattleGlobal.Core.getPlayer(playerId);
        Unit unit = UnitManager.getInatance().getUnitById(unitId);
        if ( player != null && unit != null )
        {
            CheckBPointCostByAttackProperties props = new CheckBPointCostByAttackProperties();
            props.playerId = playerId;
            props.unitId = unitId;
            props.costBase = cost;
            unit.applyEffects(props);
            if (player.curBPoint >= cost)
            {
                return true;
            }
        }
        return false;
    }

}

