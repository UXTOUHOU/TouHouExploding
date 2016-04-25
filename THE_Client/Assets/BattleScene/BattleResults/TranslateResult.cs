using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TranslateResult : IBattleResult
{
    public Unit target;
    public int offsetRow;
    public int offsetCol;

    public void execute()
    {
        EventVOBase evtVO = BattleObjectFactory.createEventVO(BattleConsts.Code.Translate);
        evtVO.setProperty(BattleConsts.Property.TranslateOriginalRow, this.target.row);
        evtVO.setProperty(BattleConsts.Property.TranslateOriginalCol, this.target.col);
        evtVO.setProperty(BattleConsts.Property.TranslateOffsetRow, this.offsetRow);
        evtVO.setProperty(BattleConsts.Property.TranslateOffsetCol, this.offsetCol);
        if ( target.translate(offsetRow, offsetCol) == BattleConsts.UNIT_ACTION_SUCCESS )
        {
            evtVO.setProperty(BattleConsts.Property.TranslateTargetRow, this.target.row);
            evtVO.setProperty(BattleConsts.Property.TranslateTargetCol, this.target.col);
            BattleEventBase evt = BattleObjectFactory.createBattleEvent(BattleConsts.Code.TakeDamage, evtVO);
            ProcessManager.getInstance().raiseEvent(evt);
        }
    }

    public int getType()
    {
        throw new NotImplementedException();
    }
}

