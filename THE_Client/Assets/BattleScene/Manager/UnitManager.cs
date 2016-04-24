using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class UnitManager
{
    /// <summary>
    /// 保存所有Unit的UI对象
    /// </summary>
    public static List<UnitUI> UnitUIList = new List<UnitUI>();

    /// <summary>
    /// 保存所有棋盘上的Unit
    /// </summary>
    public static List<Unit> UnitList = new List<Unit>();

    // 执行Update时会检测并更新有改动的部分
    public static bool HPChanged = false;
    public static bool GroupChanged = false;
    public static bool CellChanged = false;

    private static UnitManager _instance;

    public static UnitManager getInatance()
    {
        if ( _instance == null )
        {
            _instance = new UnitManager();
        }
        return _instance;
    }
    /// <summary>
    /// 当前所有的unit对象
    /// </summary>
    private List<Unit> _unitList;
    /// <summary>
    /// 单位的配置
    /// </summary>
    private Dictionary<string, IParser> _unitCfgMap;

    public void init()
    {
        this._unitList = new List<Unit>();
        this._unitCfgMap = DataManager.getInstance().getDatasByName("Units") as Dictionary<string, IParser>;
    }

    public void registerUnit(Unit unit)
    {
        this._unitList.Add(unit);
    }
    
    public void update()
    {
        int i, len;
        len = this._unitList.Count;
        for (i=0;i< len;i++)
        {
            this._unitList[i].update();
        }
    }

    /// <summary>
    /// 根据id获取单位
    /// </summary>
    /// <param name="id">单位id</param>
    /// <returns></returns>
    public Unit getUnitById(int id)
    {
        int i, len;
        Unit unit = null;
        len = this._unitList.Count;
        for (i = 0; i < len; i++)
        {
            if ( this._unitList[i].id == id )
            {
                unit = this._unitList[i];
                break;
            }
            unit = this._unitList[i];
        }
        return unit;
    }

    /// <summary>
    /// 获取所有的单位集合
    /// </summary>
    /// <returns></returns>
    public Unit[] getAllUnits()
    {
        return this._unitList.ToArray();
    }

    /// <summary>
    /// 根据配置id获得单位配置
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public UnitCfg getUnitCfgBySysId(string id)
    {
        IParser parser;
        UnitCfg cfg = null;
        if ( this._unitCfgMap.TryGetValue(id,out parser) )
        {
            cfg = parser as UnitCfg;
        }
        return cfg;
    }

    // Update is called once per frame
    void Update()
    {
        if (CellChanged)
            throw new NotImplementedException();
        if (HPChanged)
        {
            foreach (var it in UnitUIList)
            {
                it.UpdateHP();
            }
        }
        if (GroupChanged)
        {
            foreach (var it in UnitUIList)
            {
                it.UpdateGroup();
            }
        }
    }

    public static void AddUnitSprite(UnitUI unitSprite)
    {
        UnitUIList.Add(unitSprite);
    }

    /// <summary>
    /// 设置所有单位阵营和HP是否显示
    /// </summary>
    /// <param name="visible"></param>
    public static void SetAllUnitAttributeVisible(bool visible)
    {
        foreach (var unit in UnitManager.UnitUIList)
        {
            unit.SetGroupVisible(visible);
            unit.SetHPVisible(visible);
        }
    }
}