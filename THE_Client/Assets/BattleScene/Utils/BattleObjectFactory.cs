using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BattleObjectFactory
{
    public static BattleEventBase createBattleEvent(BattleConsts.Code code,EventVOBase vo)
    {
        BattleEventBase evt = null;
        switch ( code )
        {
            case BattleConsts.Code.TakeDamage:
                evt = new TakeDamageEvent(vo);
                break;
            case BattleConsts.Code.Translate:
                evt = new TranslateTargetEvent(vo);
                break;
            case BattleConsts.Code.FlagAttackTarget:
                evt = new FlagAttackTargetEvent(vo);
                break;
            case BattleConsts.Code.PreCounterAttack:
                evt = new PreCounterAttackEvent(vo);
                break;
            case BattleConsts.Code.SummonUnitSuccess:
                evt = new SummonUnitSuccessEvent(vo);
                break;
        }
        return evt;
    }

    public static EventVOBase createEventVO(BattleConsts.Code code)
    {
        EventVOBase vo = null;
        switch ( code )
        {
            case BattleConsts.Code.TakeDamage:
                vo = new TakeDamageEventVO();
                break;
            case BattleConsts.Code.Translate:
                vo = new TranslateTargetEventVO();
                break;
            case BattleConsts.Code.FlagAttackTarget:
                vo = new FlagAttackTargetEventVO();
                break;
            case BattleConsts.Code.PreCounterAttack:
                vo = new PreCounterAttackEventVO();
                break;
            case BattleConsts.Code.SummonUnitSuccess:
                vo = new SummonUnitSuccessEventVO();
                break;
        }
        return vo;
    }
}

