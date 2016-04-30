using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MainPhaseSelectMovePathState : BattleStateBase, ICommand
{
    private Cell _selectedCell;
    private Unit _selectedUnit;
    private int[] _moveRange;
    /// <summary>
    /// 当前是否正在选择路径
    /// </summary>
    private bool _isSelectingMovePath;
    //private int[] _pathList;
    private List<Cell> _pathList;
    private int _pathCount;

    public MainPhaseSelectMovePathState(IFSM fsm)
        :base(fsm)
    {
        this._pathList = new List<Cell>();
    }

    public override void onStateEnter()
    {
        this._selectedCell = BattleGlobal.SelectedCell;
        this._selectedUnit = this._selectedCell.UnitOnCell;
        this._moveRange = this._selectedUnit.getAvailableMoveRange();
        Chessboard.showAvailableMoveRange(true,this._moveRange);
        this._isSelectingMovePath = false;
        Chessboard.addEnterEventHandler(this.onCellEnter);
        //OperationManager.getInstance().setOperation(BattleConsts.CellOp_Idle);
        //CommandManager.getInstance().addCommand(BattleConsts.CMD_OnCellSelected, this);
    }

    public override void onStateExit()
    {
        this._selectedCell = null;
        this._selectedUnit = null;
        this._moveRange = null;
        this._pathList.Clear();
        Chessboard.showAvailableMoveRange(false);
        Chessboard.removeEnterEventHandler(this.onCellEnter);
        Chessboard.removeClickEventHandler(this.onCellClick);
        //CommandManager.getInstance().removeCommand(BattleConsts.CMD_OnCellSelected, this);
    }

    public override void update()
    {
        // 点击了右键，则表示取消当前操作
        if ( Input.GetMouseButtonUp(1) )
        {
            if ( !this._isSelectingMovePath )
            {
                this._fsm.setState(BattleConsts.BattleState.MainPhase_SelectUnitAction);
            }
            else
            {
                this._isSelectingMovePath = false;
                this._pathCount = 0;
                this._pathList.Clear();
                Chessboard.removeClickEventHandler(this.onCellClick);
                Chessboard.showMovePath(this.getMovePathPosIndexArr(this._pathList));
            }
        }
    }

    public void onCellClick(GameObject go)
    {
        Cell cell = go.GetComponent<Cell>();
        int index = this._pathList.IndexOf(cell);
        if ( index != -1 )
        {
            BattleGlobal.UnitToBeOperated = this._selectedUnit;
            BattleGlobal.MovePaths = this.getMovePathPosIndexArr(this._pathList);
            this._fsm.setState(BattleConsts.BattleState.MainPhase_MoveUnit);
        }
    }

    public void onCellEnter(GameObject go)
    {
        Cell cell = go.GetComponent<Cell>();
        //Debug.Log("当前移动: " + cell.location.y + "  " + cell.location.x);
        // 当前不在选择移动路径的时候，需要先移动到初始格
        if ( !this._isSelectingMovePath )
        {
            if ( this._selectedCell == cell )
            {
                //Debug.Log("添加至移动起始位置");
                this._isSelectingMovePath = true;
                this._pathCount = 0;
                this._pathList.Add(cell);
                this._pathCount++;
                Chessboard.addClickEventHandler(this.onCellClick);
                Chessboard.showMovePath(this.getMovePathPosIndexArr(this._pathList));
            }
        }
        else
        {
            // 先判断是否进入当前已经存在的路径上
            int index = this._pathList.IndexOf(cell);
            if ( index != -1 )
            {
                // 不是当前路径上的最后一格
                //Debug.Log("当前路径已经存在！ index = " + index);
                if ( index != this._pathCount-1 )
                {
                    this._pathList.RemoveRange(index+1, this._pathCount-(index+1));
                    this._pathCount = index + 1;
                    Chessboard.showMovePath(this.getMovePathPosIndexArr(this._pathList));
                }
            }
            else
            {
                // 判断是否已经超出单位的机动
                if ( this._pathCount > this._selectedUnit.UnitAttribute.motilityCurrent )
                {
                    return;
                }
                // 判断是否在可移动的格子内
                int posIndex = cell.location.y * BattleConsts.MapMaxCol + cell.location.x;
                if ( this._moveRange[posIndex] < 0 )
                {
                    return;
                }
                // 判断是否与上一格相邻
                Cell lastCell = this._pathList[this._pathList.Count - 1];
                if ( cell.location.Adjacent(lastCell.location) )
                {
                    this._pathList.Add(cell);
                    this._pathCount++;
                    Chessboard.showMovePath(this.getMovePathPosIndexArr(this._pathList));
                    //Debug.Log("添加至新路径 " + this._pathCount);
                }
            }
        }
    }
    /// <summary>
    /// 获取路径的下标数组
    /// </summary>
    /// <param name="movePath">Cell的路径集合</param>
    /// <returns></returns>
    private int[] getMovePathPosIndexArr(List<Cell> movePath)
    {
        List<int> movePathPosIndexList = new List<int>();
        int colLimit = BattleConsts.MapMaxCol;
        ChessboardPosition location;
        for (int i=0;i<this._pathCount;i++)
        {
            location = movePath[i].location;
            movePathPosIndexList.Add(location.x + location.y * colLimit);
        }
        return movePathPosIndexList.ToArray();
    }

    public void recvCommand(int cmd, params object[] args)
    {
        /*switch (cmd)
        {
            case BattleConsts.CMD_OnCellSelected:
                this._fsm.setState(BattleConsts.MainPhaseSubState_SelectUnitAction);
                break;
            default:
                break;
        }*/
    }
}
