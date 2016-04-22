using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class IDProvider
{
    private static IDProvider _instance;

    public static IDProvider getInstance()
    {
        if ( _instance == null )
        {
            _instance = new IDProvider();
        }
        return _instance;
    }

    private const int PlayerStartId = 0;
    private const int UnitStartId = 10000000;

    private IDList _playerList;
    private IDList _unitList;


    public IDProvider()
    {
        this._unitList = new IDList(UnitStartId);
    }

    public int applyUnitId(IID unit)
    {
        return this._unitList.applyId(unit);
    }

    public int applyPlayerId(IID player)
    {
        return this._playerList.applyId(player);
    }
}

