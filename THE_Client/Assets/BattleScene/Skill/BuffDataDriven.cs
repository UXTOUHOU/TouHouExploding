using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniLua;

public class BuffDataDriven : ILuaUserData
{
    private List<int> _effectCodes;

    private List<ISkillEffect> _effectList;

    private int _ref;

    public BuffDataDriven()
    {
        this._effectList = new List<ISkillEffect>();
    }

    //public bool canTrigger(int code)
    //{
    //    return this._effectCodes.IndexOf(code) != -1;
    //}

    public IBattleProperties applyTo(IBattleProperties props)
    {
        int count = this._effectList.Count;
        ISkillEffect effect;
        for (int i=0;i<count;i++)
        {
            effect = this._effectList[i];
            if ( effect.getCode() == props.getCode() )
            {
                props = effect.applyTo(props);
            }
        }
        return props;
    }

    public void registerEffect(ISkillEffect effect)
    {
        this._effectList.Add(effect);
        if ( this._effectCodes.IndexOf(effect.getCode()) != -1 )
        {
            this._effectCodes.Add(effect.getCode());
        }
    }

    public void addEffect(ISkillEffect effect)
    {
        this._effectList.Add(effect);
    }

    public void getEffectsByCode(int code,List<ISkillEffect> effects)
    {
        int count = this._effectList.Count;
        ISkillEffect effect;
        for (int i = 0; i < count; i++)
        {
            effect = this._effectList[i];
            if ( effect.getCode() == code )
            {
                effects.Add(effect);
            }
        }
    }

    public int getRef()
    {
        return this._ref;
    }

    public void setRef(int value)
    {
        this._ref = value;
    }

    public delegate void OnUnitAttackAnnounceHandler(IBattleProperties props);
    /// <summary>
    /// 攻击宣言前触发
    /// </summary>
    public OnUnitAttackAnnounceHandler OnUnitAttackAnnounce;
}

