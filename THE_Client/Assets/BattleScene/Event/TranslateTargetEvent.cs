using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TranslateTargetEvent : BattleEventBase
{
    public TranslateTargetEvent(EventVOBase vo)
        :base(vo)
    {

    }

    override public int getEventCode()
    {
        return BattleConsts.CODE_TRANSLATE;
    }

    override public List<ISkillEffect> getTriggerEffects()
    {
        List<ISkillEffect> effects = new List<ISkillEffect>();
        Unit attacker = (Unit)this._eventVO.getProperty(BattleConsts.PROPERTY_DAMAGE_ATTACKER);
        attacker.getBuffEffectsByCode(this.getEventCode(), effects);
        ISkillEffect effect;
        for (int i = 0; i < effects.Count; i++)
        {
            effect = effects[i];
            InterpreterManager.getInstance().addParam(this._eventVO, BattleConsts.PARAM_TYPE_VO);
            if (!InterpreterManager.getInstance().checkCondition(effect.getCondition(), 1))
            {
                effects.Remove(effect);
                i--;
            }
        }
        return effects;
    }
}

