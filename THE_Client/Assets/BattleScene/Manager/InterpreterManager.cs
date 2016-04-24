using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniLua;
using UnityEngine;


public class InterpreterManager
{
    private static InterpreterManager _instance;

    public static InterpreterManager getInstance()
    {
        if ( _instance == null )
        {
            _instance = new InterpreterManager();
        }
        return _instance;
    }

    private ILuaState _luaState;
    private LuaState _currentState;

    private List<LuaParam> _params;

    public InterpreterManager()
    {
        this._luaState = LuaAPI.NewState();
        this._luaState.L_OpenLibs();
        this._luaState.L_RequireF("EffectLib.cs", EffectLib.initLib, false);
        this._luaState.L_RequireF("PropertyLib.cs", PropertyLib.initLib, false);
        this._luaState.L_RequireF("BattleFieldLib.cs", BattleFieldLib.initLib, false);
        this.loadScript("Utility.lua");
        this.loadScript("Constants.lua");
        this._currentState = new LuaState();
        this._params = new List<LuaParam>();
        this._luaState.Pop(this._luaState.GetTop());
    }

    public void createSkill()
    {
        this.loadSkillScript("skill1");
        string scriptName = "skills/skill1.lua";
        var status = this._luaState.L_DoFile(scriptName);
        if (status != ThreadStatus.LUA_OK)
        {
            throw new Exception(this._luaState.ToString(-1));
        }

    }

    public int initSkill(string skillId,Unit unit)
    {
        if ( this.loadSkillScript(skillId) != BattleConsts.LUA_OPERATION_SUCCESS )
        {
            Debug.LogError("lua script " + skillId + ".lua occurs an error!");
            return BattleConsts.LUA_OPERATION_FAIL;
        }
        if ( !this._luaState.IsTable(-1) )
        {
            return BattleConsts.LUA_OPERATION_FAIL;
        }
        this._luaState.GetField(-1, "initSkill");
        if ( !this._luaState.IsFunction(-1) )
        {
            Debug.LogError("method initSkill is not exist!");
            return BattleConsts.LUA_OPERATION_FAIL;
        }
        this.pushLightUserData(this._luaState,unit);
        this._luaState.PCall(1, 0, 0);
        return BattleConsts.LUA_OPERATION_SUCCESS;
    }

    public void registerEffect(SkillEffect effect)
    {
        this._luaState.PushLightUserData(effect);
        effect.setRef(this._luaState.L_Ref(LuaDef.LUA_REGISTRYINDEX));
    }

    public void registerLightUserData(ILuaUserData userData)
    {
        this._luaState.PushLightUserData(userData);
        userData.setRef(this._luaState.L_Ref(LuaDef.LUA_REGISTRYINDEX));
    }

    public int callFunction(int funcRef,int paramCount,int retCount=0)
    {
        this._luaState.RawGetI(LuaDef.LUA_REGISTRYINDEX, funcRef);
        if ( !this._luaState.IsFunction(-1) )
        {
            Debug.LogError("funcRef doesn't point to a function!");
            this._luaState.Pop(1);
            return BattleConsts.LUA_OPERATION_FAIL;
        }
        if ( this._params.Count != paramCount )
        {
            Debug.Log("param count is not match!");
            return BattleConsts.LUA_OPERATION_FAIL;
        }
        this.pushParams();
        //int numArgs = args==null ? 0 : 
        var status = this._luaState.PCall(paramCount, retCount, 0);
        if (status != ThreadStatus.LUA_OK)
        {
            Debug.LogError(this._luaState.ToString(-1));
            this._luaState.Pop(1);
            return BattleConsts.LUA_OPERATION_FAIL;
        }
        return BattleConsts.LUA_OPERATION_SUCCESS;
    }

    /// <summary>
    /// 检测条件
    /// </summary>
    /// <param name="funcRef">函数索引</param>
    /// <param name="paramCount">参数个数</param>
    /// <returns></returns>
    public bool checkCondition(int funcRef,int paramCount)
    {
        if ( funcRef == 0 )
        {
            this.clearParams();
            return true;
        }
        int result = this.callFunction(funcRef, paramCount, 1);
        if ( result != BattleConsts.LUA_OPERATION_SUCCESS )
        {
            Debug.LogError("condition function call fail!");
            this.clearParams();
            return false;
        }
        bool checkResult = this._luaState.ToBoolean(-1);
        this._luaState.Pop(1);
        return checkResult;
    }

    public int loadSkillScript(string skillId)
    {
        this._luaState.GetGlobal(skillId);
        if ( this._luaState.IsNil(-1) )
        {
            this._luaState.Pop(1);
            this._luaState.CreateTable(0, 0);
            this._luaState.SetGlobal(skillId);
            this._luaState.GetGlobal(skillId);
            return this.loadScript("skills/" + skillId + ".lua");
        }
        return BattleConsts.LUA_OPERATION_SUCCESS;
    }

    public void createSkillBySkillId(string skillId)
    {

    }

    private int loadScript(string scriptName)
    {
        var status = this._luaState.L_DoFile(scriptName);
        if (status != ThreadStatus.LUA_OK)
        {
            throw new Exception(this._luaState.ToString(-1));
            return BattleConsts.LUA_OPERATION_FAIL;
        }
        return BattleConsts.LUA_OPERATION_SUCCESS;
    }

    /// <summary>
    /// 添加lua函数的参数
    /// </summary>
    /// <param name="param">参数</param>
    /// <param name="paramType">参数类型</param>
    public void addParam(object param,int paramType)
    {
        this._params.Add(this.genLuaParam(param, paramType));
    }

    private LuaParam genLuaParam(object param,int paramType)
    {
        LuaParam luaParam = new LuaParam();
        luaParam.param = param;
        luaParam.paramType = paramType;
        return luaParam;
    }

    /// <summary>
    /// 将缓存的参数压入栈
    /// </summary>
    private void pushParams()
    {
        int len = this._params.Count;
        for (int i = 0; i < len; i++)
        {
            LuaParam it = this._params[i];
            switch ( it.paramType )
            {
                case BattleConsts.PARAM_TYPE_BOOLEAN:
                    this._luaState.PushBoolean((bool)it.param);
                    break;
                case BattleConsts.PARAM_TYPE_INT:
                    this._luaState.PushInteger((int)it.param);
                    break;
                case BattleConsts.PARAM_TYPE_STRING:
                    this._luaState.PushString((string)it.param);
                    break;
                case BattleConsts.PARAM_TYPE_VO:
                    this._luaState.PushLightUserData(it.param);
                    break;
                case BattleConsts.PARAM_TYPE_EVENT:
                    this._luaState.PushLightUserData(it.param);
                    break;
            }
        }
        this.clearParams();
    }

    /// <summary>
    /// 清楚缓存的参数
    /// </summary>
    private void clearParams()
    {
        int len = this._params.Count;
        for (int i=0;i< len;i++)
        {
            this._params[i].param = null;
        }
        this._params.Clear();
    }

    public void pushEffect(ILuaState luaState,ISkillEffect effect)
    {
        if ( effect == null || effect.getRef() == 0 )
        {
            luaState.PushNil();
        }
        else
        {
            luaState.RawGetI(LuaDef.LUA_REGISTRYINDEX, effect.getRef());
        }
    }

    public void pushLightUserData(ILuaState luaState,ILuaUserData userData)
    {
        if (userData == null || userData.getRef() == 0)
        {
            luaState.PushNil();
        }
        else
        {
            luaState.RawGetI(LuaDef.LUA_REGISTRYINDEX, userData.getRef());
        }
    }
}

class LuaParam
{
    public object param;
    public int paramType;
}

