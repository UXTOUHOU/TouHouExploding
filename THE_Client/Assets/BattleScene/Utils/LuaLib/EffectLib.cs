using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UniLua;
using UnityEngine;

public class EffectLib
{
    public static int initLib(ILuaState luaState)
    {
        var define = new NameFuncPair[]
        {
            new NameFuncPair("testLuaCall", testLuaCall),
            new NameFuncPair("createEffect",createEffect),
            new NameFuncPair("createBuff",createBuff),
            new NameFuncPair("addEffectToBuff",addEffectToBuff),
            new NameFuncPair("addBuffToUnit",addBuffToUnit),
            new NameFuncPair("applyDamage",applyDamage),
            new NameFuncPair("applyTranslate",applyTranslate),
        };
        luaState.L_NewLib(define);
        return 1;
    }

    /// <summary>
    /// 创建buff
    /// <para>int buffId</para>
    /// <para>bool isDebuff</para>
    /// <para>bool isHidden</para>
    /// </summary>
    /// <param name="luaState"></param>
    /// <returns></returns>
    public static int createBuff(ILuaState luaState)
    {
        BuffDataDriven buff = new BuffDataDriven();
        InterpreterManager.getInstance().registerLightUserData(buff);
        InterpreterManager.getInstance().pushLightUserData(luaState, buff);
        return 1;
    }

    public static int createEffect(ILuaState luaState)
    {
        SkillEffect effect = new SkillEffect();
        InterpreterManager.getInstance().registerEffect(effect);
        InterpreterManager.getInstance().pushEffect(luaState, effect);
        return 1;
    }

    public static int addEffectToBuff(ILuaState luaState)
    {
        SkillEffect effect = (SkillEffect)luaState.ToUserData(-2);
        BuffDataDriven buff = (BuffDataDriven)luaState.ToUserData(-1);
        buff.addEffect(effect);
        return 0;
    }

    public static int addBuffToUnit(ILuaState luaState)
    {
        BuffDataDriven buff = (BuffDataDriven)luaState.ToUserData(-2);
        Unit unit = (Unit)luaState.ToUserData(-1);
        unit.addBuff(buff);
        return 0;
    }

    public static int applyDamage(ILuaState luaState)
    {
        if ( luaState.IsTable(-1) )
        {
            DamageResult dmgRes = new DamageResult();
            // 攻击者
            luaState.GetField(-1, "attacker");
            dmgRes.attacker = (Unit)luaState.ToUserData(-1);
            luaState.Pop(1);
            // 受害者
            luaState.GetField(-1, "victim");
            dmgRes.victim = (Unit)luaState.ToUserData(-1);
            luaState.Pop(1);
            // 伤害
            luaState.GetField(-1, "physicalDamage");
            dmgRes.physicalDamage = luaState.ToInteger(-1);
            luaState.Pop(1);
            luaState.GetField(-1, "spellDamage");
            dmgRes.spellDamage = luaState.ToInteger(-1);
            luaState.Pop(1);
            luaState.GetField(-1, "hpRemoval");
            dmgRes.hpRemoval = luaState.ToInteger(-1);
            luaState.Pop(1);
            // 伤害原因
            luaState.GetField(-1, "damageReason");
            dmgRes.damageReason = luaState.ToInteger(-1);
            luaState.Pop(1);
            ProcessManager.getInstance().addResult(dmgRes);
        }
        return 0;
    }

    public static int applyTranslate(ILuaState luaState)
    {
        if (luaState.IsTable(-1))
        {
            TranslateResult transRes = new TranslateResult();
            // 位移目标
            luaState.GetField(-1, "target");
            transRes.target = (Unit)luaState.ToUserData(-1);
            luaState.Pop(1);
            // 行偏移量
            luaState.GetField(-1, "offsetRow");
            transRes.offsetRow = luaState.ToInteger(-1);
            luaState.Pop(1);
            // 列偏移量
            luaState.GetField(-1, "offsetCol");
            transRes.offsetCol = luaState.ToInteger(-1);
            luaState.Pop(1);
            //// 位移原因
            //luaState.GetField(-1, "translateReason");
            //transRes. = luaState.ToInteger(-1);
            //luaState.Pop(1);
            ProcessManager.getInstance().addResult(transRes);
        }
        return 0;
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

