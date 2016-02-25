using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

interface ICellOperation
{
    void onCellEnter(Cell cell);
    void onCellExit(Cell cell);
    /// <summary>
    /// 左键点击单元格
    /// </summary>
    /// <param name="cell"></param>
    void onCellClick(Cell cell);
    void clear();
}