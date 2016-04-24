using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class OperationManager : ICommand
{
    private static OperationManager _instance;

    public static OperationManager getInstance()
    {
        if (_instance == null)
        {
            _instance = new OperationManager();
        }
        return _instance;
    }

    private Dictionary<int, ICellOperation> _cellOpsMap;

    private ICellOperation _curOp;

    public void init()
    {
        this._cellOpsMap = new Dictionary<int, ICellOperation>();
        this._cellOpsMap.Add(BattleConsts.CellOp_Idle, new CellOperationIdle());
        BattleGlobal.Core.chessboard.addClickEventHandler(this.onCellClickHandler);
        BattleGlobal.Core.chessboard.addEnterEventHandler(this.onCellEnterHandler);
        BattleGlobal.Core.chessboard.addExitEventHandler(this.onCellExitHandler);
    }

    public void setOperation(int opId)
    {
        ICellOperation cellOp;
        if ( this._cellOpsMap.TryGetValue(opId, out cellOp) )
        {
            this._curOp = cellOp;
        }
    }

    public void onCellEnterHandler(GameObject go)
    {
        Cell cell = go.GetComponent<Cell>();
        if (this._curOp != null && cell != null)
        {
            this._curOp.onCellEnter(cell);
        }
    }

    public void onCellExitHandler(GameObject go)
    {
        Cell cell = go.GetComponent<Cell>();
        if (this._curOp != null && cell != null)
        {
            this._curOp.onCellExit(cell);
        }
    }

    public void onCellClickHandler(GameObject go)
    {
        Cell cell = go.GetComponent<Cell>();
        if (this._curOp != null && cell != null)
        {
            this._curOp.onCellClick(cell);
        }
    }

    public void clearOperationState()
    {
        if (this._curOp != null)
        {
            this._curOp.clear();
            this._curOp = null;
        }
    }

    public void recvCommand(int command, params object[] args)
    {
    }
}
