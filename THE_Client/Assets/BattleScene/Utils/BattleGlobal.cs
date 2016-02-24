using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class BattleGlobal
{
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
    /// <summary>
    /// 当前选中的格子
    /// </summary>
    public static Cell SelectCell;
}

