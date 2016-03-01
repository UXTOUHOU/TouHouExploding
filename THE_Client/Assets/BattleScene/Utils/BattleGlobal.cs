using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class BattleGlobal
{
    public static BattleSceneMain Core;
    /// <summary>
    /// 地形格左下角X坐标
    /// </summary>
    public static float CellStartX;
    /// <summary>
    /// 地形格左下角Y坐标
    /// </summary>
    public static float CellStartY;
    /// <summary>
    /// 地形格实际尺寸
    /// </summary>
    public static float CellSize;

    public static float Scale = 1;

    #region 公用存储部分
    /// <summary>
    /// 当前选中的格子
    /// </summary>
    public static Cell SelectedCell;
    /// <summary>
    /// 待操作的单位
    /// </summary>
    public static Unit UnitToBeOperated;
    /// <summary>
    /// 移动路径
    /// </summary>
    public static int[] MovePaths;
    #endregion
}

