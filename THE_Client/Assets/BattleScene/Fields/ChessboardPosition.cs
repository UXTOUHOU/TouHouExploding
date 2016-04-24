using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ChessboardPosition
{
    public int x;
    public int y;
    public ChessboardPosition(int new_x, int new_y)
    {
        x = new_x;
        y = new_y;
    }

    /// <summary>
    /// 检测两个位置是否相邻
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool Adjacent(ChessboardPosition position)
    {
        return Math.Abs(position.x - x) + Math.Abs(position.y - y) == 1;
    }

    /// <summary>
    /// 计算两个位置的距离
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public int Distance(ChessboardPosition position)
    {
        return Math.Abs(x - position.x) + Math.Abs(y - position.y);
    }

    public Vector3 GetLocalPosition()
    {
        return Chessboard.GetCell(this).GetLocalPosition();
    }

    public Vector3 GetPosition()
    {
        return Chessboard.GetCell(this).GetPosition();
    }

    /*public override bool Equals(object obj)
    {
        ChessboardPosition pos = (ChessboardPosition)obj;
        return this.x == pos.x && this.y == pos.y;
    }*/
}
