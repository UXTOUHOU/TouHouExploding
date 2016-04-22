using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Player : IID
{
    /// <summary>
    /// 当前的B点
    /// </summary>
    private int _curBPoint;
    /// <summary>
    /// 当前的B点
    /// </summary>
    public int curBPoint
    {
        get { return this._curBPoint;  }
    }

    private int _bPointCeilingExtra;
    public int bPointCeilingExtra
    {
        get { return this._bPointCeilingExtra; }
    }

    private int _id;
    public int id
    {
        get
        {
            return this._id;
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public void costBPoint(int value)
    {
        this._curBPoint -= value;
    }

    public Player()
    {
        this._id = IDProvider.getInstance().applyUnitId(this);
    }

    public void resetBPoint()
    {
        this._curBPoint = BattleConsts.DefaultBPoint;
    }
}