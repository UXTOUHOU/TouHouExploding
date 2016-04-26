using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UniLua;
using UnityEngine;

public class PropertyLib
{
    public static int initLib(ILuaState luaState)
    {
        var define = new NameFuncPair[]
        {
            new NameFuncPair("setEffectCode", setEffectCode),
            new NameFuncPair("getEffectCode",getEffectCode),
            new NameFuncPair("setEffectCondition", setEffectCondition),
            new NameFuncPair("getEffectConditon",getEffectCondition),
            new NameFuncPair("setEffectCost", setEffectCost),
            new NameFuncPair("getEffectCost",getEffectCost),
            new NameFuncPair("setEffectTarget", setEffectTarget),
            new NameFuncPair("getEffectTarget",getEffectTarget),
            new NameFuncPair("setEffectOperation", setEffectOperation),
            new NameFuncPair("getEffectOperation",getEffectOperation),
            new NameFuncPair("getEventVO",getEventVO),
            new NameFuncPair("getEventVOProperty",getEventVOProperty),
            new NameFuncPair("getVOProperty",getVOProperty),
        };
        luaState.L_NewLib(define);
        return 1;
    }

    public static int setEffectCode(ILuaState luaState)
    {
        SkillEffect effect = (SkillEffect)luaState.ToUserData(-2);
        if ( effect == null )
        {
            throw new ArgumentException("effect is null!");
        }
		BattleConsts.Code code = (BattleConsts.Code)luaState.ToInteger(-1);
        effect.setCode(code);
        return 0;
    }

    public static int getEffectCode(ILuaState luaState)
    {
        SkillEffect effect = (SkillEffect)luaState.ToUserData(-1);
        if (effect == null)
        {
            throw new ArgumentException("effect is null!");
        }
        luaState.PushInteger((int)effect.getCode());
        return 1;
    }

    public static int getEffectCondition(ILuaState luaState)
    {
        SkillEffect effect = (SkillEffect)luaState.ToUserData(-1);
        if (effect == null)
        {
            throw new ArgumentException("effect is null!");
        }
        luaState.PushInteger(effect.getCondition());
        return 1;
    }

    public static int setEffectCondition(ILuaState luaState)
    {
        SkillEffect effect = (SkillEffect)luaState.ToUserData(-2);
        if (effect == null)
        {
            throw new ArgumentException("effect is null!");
        }
        int condition = 0;
        if ( !luaState.IsFunction(-1) )
        {
            throw new ArgumentException("effect condition is not a function!");
        }
        condition = luaState.L_Ref(LuaDef.LUA_REGISTRYINDEX);
        effect.setCondition(condition);
        return 0;
    }

    public static int getEffectCost(ILuaState luaState)
    {
        SkillEffect effect = (SkillEffect)luaState.ToUserData(-1);
        if (effect == null)
        {
            throw new ArgumentException("effect is null!");
        }
        luaState.PushInteger(effect.getCost());
        return 1;
    }

    public static int setEffectCost(ILuaState luaState)
    {
        SkillEffect effect = (SkillEffect)luaState.ToUserData(-2);
        if (effect == null)
        {
            throw new ArgumentException("effect is null!");
        }
        int cost = 0;
        if (!luaState.IsFunction(-1))
        {
            throw new ArgumentException("effect cost is not a function!");
        }
        cost = luaState.L_Ref(LuaDef.LUA_REGISTRYINDEX);
        effect.setCost(cost);
        return 0;
    }

    public static int getEffectTarget(ILuaState luaState)
    {
        SkillEffect effect = (SkillEffect)luaState.ToUserData(-1);
        if (effect == null)
        {
            throw new ArgumentException("effect is null!");
        }
        luaState.PushInteger(effect.getTarget());
        return 1;
    }

    public static int setEffectTarget(ILuaState luaState)
    {
        SkillEffect effect = (SkillEffect)luaState.ToUserData(-2);
        if (effect == null)
        {
            throw new ArgumentException("effect is null!");
        }
        int target = 0;
        if ( !luaState.IsFunction(-1) )
        {
            throw new ArgumentException("effect target is not a function!");
        }
        target = luaState.L_Ref(LuaDef.LUA_REGISTRYINDEX);
        effect.setTarget(target);
        return 0;
    }

    public static int getEffectOperation(ILuaState luaState)
    {
        SkillEffect effect = (SkillEffect)luaState.ToUserData(-1);
        if (effect == null)
        {
            throw new ArgumentException("effect is null!");
        }
        luaState.PushInteger(effect.getOperation());
        return 1;
    }

    public static int setEffectOperation(ILuaState luaState)
    {
        SkillEffect effect = (SkillEffect)luaState.ToUserData(-2);
        if (effect == null)
        {
            throw new ArgumentException("effect is null!");
        }
        int operation = 0;
        if (!luaState.IsFunction(-1))
        {
            throw new ArgumentException("effect target is not a function!");
        }
        operation = luaState.L_Ref(LuaDef.LUA_REGISTRYINDEX);
        effect.setOperation(operation);
        return 0;
    }

    public static int getEventVO(ILuaState luaState)
    {
        BattleEventBase evt = (BattleEventBase)luaState.ToUserData(-1);
        if ( evt == null )
        {
            luaState.PushNil();
        }
        else
        {
            luaState.PushLightUserData(evt.getEventVO());
        }
        return 1;
    }

    public static int getEventVOProperty(ILuaState luaState)
    {
        BattleEventBase evt = (BattleEventBase)luaState.ToUserData(-2);
        if (evt == null)
        {
            luaState.PushNil();
        }
        else
        {
			BattleConsts.Property propId = (BattleConsts.Property)luaState.ToInteger(-1);
            object prop = evt.getEventVOProperty(propId);
            if ( prop == null )
            {
                luaState.PushNil();
            }
            else
            {
                luaState.PushLightUserData(prop);
            }
        }
        return 1;
    }

    /// <summary>
    /// 获取IBattleVO接口中的属性
    /// </summary>
    /// <param name="luaState"></param>
    /// <returns></returns>
    public static int getVOProperty(ILuaState luaState)
    {
        IBattleVO vo = (IBattleVO)luaState.ToUserData(-2);
        if (vo == null)
        {
            luaState.PushNil();
        }
        else
        {
			BattleConsts.Property propId = (BattleConsts.Property)luaState.ToInteger(-1);
            object prop = vo.getProperty(propId);
            if (prop == null)
            {
                luaState.PushNil();
            }
            else
            {
                // todo 暂时使用is来判断
                // 以后可能会加入全局hash
                if ( prop is int )
                {
                    luaState.PushInteger((int)prop);
                }
                else if ( prop is bool )
                {
                    luaState.PushBoolean((bool)prop);
                }
                else if ( prop is string )
                {
                    luaState.PushString((string)prop);
                }
                else
                {
                    luaState.PushLightUserData(prop);
                }
            }
        }
        return 1;
    }

    public static int testLuaCall(ILuaState luaState)
    {
        bool arg2 = luaState.ToBoolean(-1);
        luaState.GetField(1, "n");
        int len = luaState.ToInteger(-1);
        luaState.Pop(1);
        int sum = 0;
        for (int i = 0; i < len; i++)
        {
            luaState.PushInteger(i + 1);
            luaState.GetTable(1);
            sum += luaState.ToInteger(-1);
            luaState.Pop(1);
        }
        Thread.Sleep(5000);
        luaState.PushInteger(sum);
        Debug.Log(sum);
        return 1;
    }


}

