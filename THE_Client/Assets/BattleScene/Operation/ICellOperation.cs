using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

interface ICellOperation
{
    void onCellEnterHandler(Cell cell);
    void onCellExitHandler(Cell cell);
    void onCellClickHandler(Cell cell);
}