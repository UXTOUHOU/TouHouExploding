using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SummonUnitSuccessEvent : BattleEventBase
{
    public SummonUnitSuccessEvent(EventVOBase vo)
        : base(vo)
    {

    }

    override public BattleConsts.Code getEventCode()
    {
        return BattleConsts.Code.SummonUnitSuccess;
    }

    override public string getEventName()
    {
        return "SummonUnitSuccess Event";
    }

    override public List<ISkillEffect> getTriggerEffects()
    {
        List<ISkillEffect> effects = new List<ISkillEffect>();
        Unit[] units = UnitManager.getInatance().getAllUnits();
        int i;
        int len = units.Length;
        for (i=0;i<len;i++)
        {
            units[i].getBuffEffectsByCode(this.getEventCode(), effects);
        }
        ISkillEffect effect;
        for (i = 0; i < effects.Count; i++)
        {
            effect = effects[i];
            InterpreterManager.getInstance().addParam(this._eventVO, BattleConsts.ParamType.VO);
            if (!InterpreterManager.getInstance().checkCondition(effect.getCondition(), 1))
            {
                effects.Remove(effect);
                i--;
            }
        }
        return effects;
    }
}

