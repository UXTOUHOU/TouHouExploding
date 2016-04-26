using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class UnitPool
{
    /// <summary>
    /// 该池还能继续刷新的单位id
    /// </summary>
    List<string> _availableIdList;
    /// <summary>
    /// 当前召唤池拥有的单位id
    /// </summary>
    List<string> _curIds;

    private Dictionary<string, IParser> _unitCfgMap;

    public UnitPool()
    {

    }

    public void init()
    {
        this._availableIdList = new List<string>();
        this._curIds = new List<string>();
        this._unitCfgMap = DataManager.getInstance().getDatasByName("Units") as Dictionary<string, IParser>;
        UnitCfg cfg;
        foreach (KeyValuePair<string,IParser> kv in this._unitCfgMap)
        {
            cfg = (UnitCfg)kv.Value;
            if ( cfg.isEnable == 1 )
            {
                this._availableIdList.Add(cfg.id);
            }
        }
        this.fill();
    }

    public List<string> getCurIds()
    {
        return this._curIds;
    }

    /// <summary>
    /// 从召唤池中取出index位置的id
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string summonUnit(int index)
    {
        if ( index < 0 || index >= BattleConsts.MAX_UNIT_POOL_COUNT )
        {
            return "";
        }
        string unitId = this._curIds[index];
        this._curIds.RemoveAt(index);
        this.fill();
        return unitId;
    }

    /// <summary>
    /// 填满召唤池
    /// </summary>
    public void fill()
    {
        int index;
        IParser parser;
        UnitCfg cfg;
        while ( this._curIds.Count < BattleConsts.MAX_UNIT_POOL_COUNT)
        {
            index = BattleGlobal.Core.getRandom(0, this._availableIdList.Count - 1);
            if ( this._unitCfgMap.TryGetValue(this._availableIdList[index], out parser) )
            {
                cfg = (UnitCfg)parser;
                if ( cfg.type == BattleConsts.UNIT_TYPE_GIRL )
                {
                    this._availableIdList.RemoveAt(index);
                }
                this._curIds.Add(cfg.id);
            }
            else
            {
                throw new Exception("unit with id " + this._availableIdList[index] + " is not exist in Units.xml!" );
            }
        }
    }
}

