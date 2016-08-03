using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UniLua;
using UnityEngine;

public class BattleFieldLib
{
	public static int initLib(ILuaState luaState)
	{
		var define = new NameFuncPair[]
		{
			new NameFuncPair("hasUnitOnCell", hasUnitOnCell),
			new NameFuncPair("getUnitOnCell",getUnitOnCell),
			new NameFuncPair("getUnitLocation",getUnitLocation),
			new NameFuncPair("costBPoint", costBPoint),
			new NameFuncPair("getBPoint", getBPoint),
		};
		luaState.L_NewLib(define);
		return 1;
	}

	public static int hasUnitOnCell(ILuaState luaState)
	{
		int row = luaState.ToInteger(-2);
		int col = luaState.ToInteger(-1);
		Unit unit = Chessboard.getUnitByPos(row, col);
		if (unit == null)
		{
			luaState.PushBoolean(false);
		}
		else
		{
			luaState.PushBoolean(true);
		}
		return 1;
	}

	public static int getUnitOnCell(ILuaState luaState)
	{
		int row = luaState.ToInteger(-2);
		int col = luaState.ToInteger(-1);
		Unit unit = Chessboard.getUnitByPos(row, col);
		if (unit == null)
		{
			luaState.PushNil();
		}
		else
		{
			luaState.PushLightUserData(unit);
		}
		return 1;
	}

	/// <summary>
	/// 获取单位的位置
	/// </summary>
	/// <param name="luaState"></param>
	/// <returns></returns>
	public static int getUnitLocation(ILuaState luaState)
	{
		Unit unit = (Unit)luaState.ToUserData(-1);
		if ( unit != null )
		{
			luaState.PushInteger(unit.row);
			luaState.PushInteger(unit.col);
			return 2;
		}
		else
		{
			throw new ArgumentException("get unit location fail! reason : unit is null!");
		}
	}

	/// <summary>
	/// 使用Bomb
	/// </summary>
	/// <returns>返回是否成功扣除B点</returns>
	public static int costBPoint(ILuaState luaState)
	{
		bool res;
		int cost = luaState.ToInteger(1),
			playerID = luaState.ToInteger(2);

		var player = BattleGlobal.Core.getPlayer(playerID);
		if (player.curBPoint < cost)
		{
			res = false;
		}
		else
		{
			player.costBPoint(cost);
			res = true;
		}

		luaState.PushBoolean(res);
		return 1;
	}

	/// <summary>
	/// 获取剩余Bomb数目
	/// </summary>
	/// <returns></returns>
	public static int getBPoint(ILuaState luaState)
	{
		int playerID = luaState.ToInteger(1);
		var player = BattleGlobal.Core.getPlayer(playerID);
		luaState.PushInteger(player.curBPoint);
		return 1;
	}

}

