using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniLua;

class MainPhaseSelectUnitActionState : IState
{
    private IFSM _fsm;

    private bool _isInit;

    private GameObject _unitActionView;

    private GameObject _btnMove;
    private GameObject _btnAttack;
    private GameObject _btnSkill;

    private bool _isPopUp;

    public MainPhaseSelectUnitActionState(IFSM fsm)
    {
        this._fsm = fsm;
        this._isInit = false;
        this._isPopUp = false;
    }

    private void init()
    {
        this._unitActionView = ResourceManager.getInstance().loadPrefab("Prefabs/UnitActionView");
        this._btnMove = this._unitActionView.transform.FindChild("ButtonMove").gameObject;
        this._btnAttack = this._unitActionView.transform.FindChild("ButtonAttack").gameObject;
        this._btnSkill = this._unitActionView.transform.FindChild("ButtonSkill").gameObject;
    }

    public void onStateEnter()
    {
        if (!this._isInit)
        {
            this.init();
        }
        this.showUnitActionView();
        BattleGlobal.Core.chessboard.addClickEventHandler(this.onCellClick);
    }

    public void onStateExit()
    {
        this.removeUnitActionView();
        BattleGlobal.Core.chessboard.removeClickEventHandler(this.onCellClick);
    }

    public void update()
    {
        if (Input.GetMouseButton(1))
        {
            this._fsm.setState(BattleConsts.MainPhaseSubState_Idle);
        }
    }

    private void onCellClick(GameObject go)
    {
        Cell cell = go.GetComponent<Cell>();
        if (cell != null)
        {
            BattleGlobal.SelectedCell = cell;
            if (cell.UnitOnCell != null)
            {
                this._fsm.setState(BattleConsts.MainPhaseSubState_SelectUnitAction);
            }
        }
    }

    public void recvCommand(int cmd, params object[] args)
    {
        switch (cmd)
        {
            case BattleConsts.CMD_OnCellSelected:
                this.showUnitActionView();
                break;
            default:
                break;
        }
    }

    private void showUnitActionView()
    {
        Cell cell = BattleGlobal.SelectedCell;
        BattleSceneMain.getInstance().chessboard.addChildOnLayer(this._unitActionView, BattleConsts.BattleFieldLayer_UI, cell.location.y, cell.location.x);
        if (!this._isPopUp)
        {
            this._unitActionView.SetActive(true);
            UIEventListener.Get(this._btnMove).onClick += this.btnMoveClickHander;
            UIEventListener.Get(this._btnAttack).onClick += this.btnAttackClickHander;
            UIEventListener.Get(this._btnSkill).onClick += this.btnSkillClickHander;
            this._isPopUp = true;
        }
    }

    private void removeUnitActionView()
    {
        UIEventListener.Get(this._btnMove).onClick -= this.btnMoveClickHander;
        UIEventListener.Get(this._btnAttack).onClick -= this.btnAttackClickHander;
        UIEventListener.Get(this._btnSkill).onClick -= this.btnSkillClickHander;
        this._unitActionView.SetActive(false);
        this._isPopUp = false;
    }

    private void btnMoveClickHander(GameObject go)
    {
        this._fsm.setState(BattleConsts.MainPhaseSubState_SelectMovePath);
    }

    private void btnAttackClickHander(GameObject go)
    {
    }

    private string LuaScriptFile = "character/pachouli.lua";
    private ILuaState _luaState;
    private int _testCallLua;
    private int _testCallLuaWithArgs;
    private int _testTable;
    private int _testLuaCall;
    private int _outputTable;

    private void btnSkillClickHander(GameObject go)
    {
        if ( this._luaState == null )
        {
            this._luaState = LuaAPI.NewState();
            this._luaState.L_OpenLibs();
            this._luaState.L_RequireF("TestLuaLib.cs", TestLuaLib.initLib, false);
            // 加载并运行 Lua 脚本文件
            var status = this._luaState.L_DoFile(LuaScriptFile);
            // 捕获错误
            // capture errors
            if (status != ThreadStatus.LUA_OK)
            {
                throw new Exception(this._luaState.ToString(-1));
            }
            // 确保LuaScriptFile 执行结果是一个 Lua table
            // ensuare the value returned by 'framework/main.lua' is a Lua table
            if (!this._luaState.IsTable(-1))
            {
                throw new Exception(
                    "framework main's return value is not a table");
            }
            this._testCallLua = this.StoreMethod("testCallLua");
            this._testCallLuaWithArgs = this.StoreMethod("testCallLuaWithArgs");
            this._testTable = this.StoreMethod("testTable");
            this._testLuaCall = this.StoreMethod("testLuaCall");
            this._outputTable = this.StoreMethod("outputTable");
            this._luaState.Pop(2);
        }

        DateTime beforeDT,afterDt;
        TimeSpan ts;
        // 测试调用lua
        //this.CallMethod(this._testCallLua);

        // 测试调用带参数的lua方法
        //beforeDT = System.DateTime.Now;
        //this._luaState.RawGetI(LuaDef.LUA_REGISTRYINDEX, this._testCallLuaWithArgs);
        //this._luaState.PushBoolean(false);
        //this._luaState.PushInteger(25);
        //this._luaState.PushString("test str");
        //var tmpStatus = this._luaState.PCall(3, 1, 0);
        //if (tmpStatus != ThreadStatus.LUA_OK)
        //{
        //    Debug.LogError(this._luaState.ToString(-1));
        //}
        //this._luaState.GetField(-1, "a");
        //Debug.Log(this._luaState.ToNumber(2));
        //Debug.Log(this._luaState.GetTop());
        //afterDt = System.DateTime.Now;
        //Debug.Log("run time : " + afterDt.Subtract(beforeDT).TotalMilliseconds);

        // 测试table
        //beforeDT = System.DateTime.Now;
        //this._luaState.RawGetI(LuaDef.LUA_REGISTRYINDEX, this._testTable);
        //this._luaState.CreateTable(0, 0);
        //for (int i = 0; i < 96; i++)
        //{
        //    this._luaState.PushInteger(i + 1);
        //    this._luaState.PushInteger(i);
        //    this._luaState.SetTable(2);
        //}
        //afterDt = System.DateTime.Now;
        //Debug.Log("run time : " + afterDt.Subtract(beforeDT).TotalMilliseconds);
        //beforeDT = System.DateTime.Now;
        //var tmpstatus = this._luaState.PCall(1, 1, 0);
        //if (tmpstatus != ThreadStatus.LUA_OK)
        //{
        //    Debug.LogError(this._luaState.ToString(-1));
        //}
        //int a = this._luaState.ToInteger(1);
        ////Debug.Log("lua return value = " + this._luaState.ToInteger(1));
        //afterDt = System.DateTime.Now;
        //Debug.Log("run time : " + afterDt.Subtract(beforeDT).TotalMilliseconds);

        // 测试Lua调用C#库
        beforeDT = System.DateTime.Now;
        this._luaState.RawGetI(LuaDef.LUA_REGISTRYINDEX, this._testLuaCall);
        this._luaState.PCall(0, 1, 0);
        afterDt = System.DateTime.Now;
        ts = afterDt.Subtract(beforeDT);
        Debug.Log("run time : " + ts.TotalMilliseconds);
    }

    private int StoreMethod(string name)
    {
        this._luaState.GetField(-1, name);
        if (!this._luaState.IsFunction(-1))
        {
            throw new Exception(string.Format(
                "method {0} not found!", name));
        }
        return this._luaState.L_Ref(LuaDef.LUA_REGISTRYINDEX);
    }

    private void CallMethod(int funcRef,object[] args=null,int numResult=0)
    {
        this._luaState.RawGetI(LuaDef.LUA_REGISTRYINDEX, funcRef);
        //int numArgs = args==null ? 0 : 
        var status = this._luaState.PCall(0, 0, 0);
        if (status != ThreadStatus.LUA_OK)
        {
            Debug.LogError(this._luaState.ToString(-1));
        }
    }
}
