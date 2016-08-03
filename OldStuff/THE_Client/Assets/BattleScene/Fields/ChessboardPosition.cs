using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ChessboardPosition
{
    public int col;
    public int row;
    public ChessboardPosition(int newCol, int newRow)
    {
        col = newCol;
        row = newRow;
    }

    /// <summary>
    /// 检测两个位置是否相邻
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool Adjacent(ChessboardPosition position)
    {
        return Math.Abs(position.col - col) + Math.Abs(position.row - row) == 1;
    }

    /// <summary>
    /// 计算两个位置的距离
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public int Distance(ChessboardPosition position)
    {
        return Math.Abs(col - position.col) + Math.Abs(row - position.row);
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
