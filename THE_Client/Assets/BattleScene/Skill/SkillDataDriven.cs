using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniLua;
using UnityEngine;

public class SkillDataDriven
{
    private string _skillId;
    private int _cost;


    public SkillDataDriven()
    {
        this._cost = 0;
    }

    private LuaState _lua;

    public void initSkill(ILuaState luaState)
    {
        if ( !luaState.IsTable(-1) )
        {
            Debug.Log(this._skillId + ".lua does  not return a table!");
        }
        this._cost = storeMethod(luaState,"cost");
        luaState.Pop(1);
        // 主动效果
        // 被动效果
    }

    private int storeMethod(ILuaState luaState, string name)
    {
        luaState.GetField(-1, "cost");
        if (luaState.IsFunction(-1))
        {
            return luaState.L_Ref(LuaDef.LUA_REGISTRYINDEX);
        }
        return 0;
    }

    public void registerToUnit(Unit unit)
    {

    }

    public delegate void OnUnitAttackAnnounceHandler(IBattleProperties props);
    /// <summary>
    /// 攻击宣言前触发
    /// </summary>
    public OnUnitAttackAnnounceHandler OnUnitAttackAnnounce;
}

