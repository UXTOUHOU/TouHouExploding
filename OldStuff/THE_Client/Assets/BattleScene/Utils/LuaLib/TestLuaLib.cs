using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UniLua;
using UnityEngine;

public class TestLuaLib
{
    private static TestLuaLib _instance;

    private static Mutex _mutex = new Mutex();

    public static TestLuaLib getInstance()
    {
        if ( _instance == null )
        {
            _instance = new TestLuaLib();
        }
        return _instance;
    }

    public static int initLib(ILuaState luaState)
    {
        var define = new NameFuncPair[]
        {
            new NameFuncPair("testLuaCall", testLuaCall),
        };
        luaState.L_NewLib(define);
        return 1;
    }

    public static int testLuaCall(ILuaState luaState)
    {
        bool arg2 = luaState.ToBoolean(-1);
        luaState.GetField(1, "n");
        int len = luaState.ToInteger(-1);
        luaState.Pop(1);
        int sum = 0;
        for (int i=0;i<len;i++)
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

