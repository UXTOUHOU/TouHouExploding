using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour
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

    // Use this for initialization
    void Start()
    {

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