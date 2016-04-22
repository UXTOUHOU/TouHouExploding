using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class IDList
{
    private int _startId;
    /// <summary>
    /// 当前id
    /// </summary>
    private int _currentId;
    /// <summary>
    /// 记录当前实现IID接口的对象的list
    /// </summary>
    private List<IID> _idList;

    public IDList(int startId=0)
    {
        this._startId = startId;
        this._currentId = startId;
        this._idList = new List<IID>();
    }

    /// <summary>
    /// 对于IID接口，申请一个唯一id
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public int applyId(IID obj)
    {
        int objId;
        if ( !this.isExist(obj) )
        {
            objId = this._currentId++;
        }
        else
        {
            objId = obj.id;
        }
        return objId;
    }

    public bool isExist(IID obj)
    {
        return this._idList.IndexOf(obj) != -1;
    }

    public void clear()
    {
        this._currentId = this._startId;
        this._idList.Clear();
    }
}
