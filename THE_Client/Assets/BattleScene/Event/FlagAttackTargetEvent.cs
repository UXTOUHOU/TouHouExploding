using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class FlagAttackTargetEvent : BattleEventBase
{
    public FlagAttackTargetEvent(EventVOBase vo)
        : base(vo)
    {

    }

    override public BattleConsts.Code getEventCode()
    {
        return BattleConsts.Code.FlagAttackTarget;
    }

    override public List<ISkillEffect> getTriggerEffects()
    {
        List<ISkillEffect> effects = new List<ISkillEffect>();
        Unit defender = (Unit)this._eventVO.getProperty(BattleConsts.Property.AttackDefender);
        defender.getBuffEffectsByCode(this.getEventCode(), effects);
        ISkillEffect effect;
        for (int i = 0; i < effects.Count; i++)
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

