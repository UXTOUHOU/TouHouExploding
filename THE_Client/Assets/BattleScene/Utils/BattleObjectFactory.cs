using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BattleObjectFactory
{
    public static BattleEventBase createBattleEvent(int code,EventVOBase vo)
    {
        BattleEventBase evt = null;
        switch ( code )
        {
            case BattleConsts.CODE_TAKE_DAMAGE:
                evt = new TakeDamageEvent(vo);
                break;
            case BattleConsts.CODE_TRANSLATE:
                evt = new TranslateTargetEvent(vo);
                break;
            case BattleConsts.CODE_FLAG_ATTACK_TARGET:
                evt = new FlagAttackTargetEvent(vo);
                break;
            case BattleConsts.CODE_PRE_COUNTER_ATTACK:
                evt = new PreCounterAttackEvent(vo);
                break;
            case BattleConsts.CODE_SUMMON_UNIT_SUCCESS:
                evt = new SummonUnitSuccessEvent(vo);
                break;
        }
        return evt;
    }

    public static EventVOBase createEventVO(int code)
    {
        EventVOBase vo = null;
        switch ( code )
        {
            case BattleConsts.CODE_TAKE_DAMAGE:
                vo = new TakeDamageEventVO();
                break;
            case BattleConsts.CODE_TRANSLATE:
                vo = new TranslateTargetEventVO();
                break;
            case BattleConsts.CODE_FLAG_ATTACK_TARGET:
                vo = new FlagAttackTargetEventVO();
                break;
            case BattleConsts.CODE_PRE_COUNTER_ATTACK:
                vo = new PreCounterAttackEventVO();
                break;
            case BattleConsts.CODE_SUMMON_UNIT_SUCCESS:
                vo = new SummonUnitSuccessEventVO();
                break;
        }
        return vo;
    }
}

