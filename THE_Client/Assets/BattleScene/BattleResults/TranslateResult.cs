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
        EventVOBase evtVO = BattleObjectFactory.createEventVO(BattleConsts.CODE_TRANSLATE);
        evtVO.setProperty(BattleConsts.PROPERTY_TRANSLATE_ORIGINAL_ROW,this.target.row);
        evtVO.setProperty(BattleConsts.PROPERTY_TRANSLATE_ORIGINAL_COL,this.target.col);
        evtVO.setProperty(BattleConsts.PROPERTY_TRANSLATE_OFFSET_ROW,this.offsetRow);
        evtVO.setProperty(BattleConsts.PROPERTY_TRANSLATE_OFFSET_COL,this.offsetCol);
        if ( target.translate(offsetRow, offsetCol) == BattleConsts.UNIT_ACTION_SUCCESS )
        {
            evtVO.setProperty(BattleConsts.PROPERTY_TRANSLATE_TARGET_ROW, this.target.row);
            evtVO.setProperty(BattleConsts.PROPERTY_TRANSLATE_TARGET_COL, this.target.col);
            BattleEventBase evt = BattleObjectFactory.createBattleEvent(BattleConsts.CODE_TAKE_DAMAGE, evtVO);
            ProcessManager.getInstance().raiseEvent(evt);
        }
    }

    public int getType()
    {
        throw new NotImplementedException();
    }
}

