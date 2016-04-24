using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PreCounterAttackEvent : BattleEventBase
{
    public PreCounterAttackEvent(EventVOBase vo)
        : base(vo)
    {

    }

    override public int getEventCode()
    {
        return BattleConsts.CODE_PRE_COUNTER_ATTACK;
    }

    public override string getEventName()
    {
        return "Pre Counter Attack Event";
    }

    override public List<ISkillEffect> getTriggerEffects()
    {
        List<ISkillEffect> effects = new List<ISkillEffect>();
        Unit defender = (Unit)this._eventVO.getProperty(BattleConsts.PROPERTY_ATTACK_DEFENDER);
        defender.getBuffEffectsByCode(this.getEventCode(), effects);
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

