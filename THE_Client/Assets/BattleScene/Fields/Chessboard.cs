using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class Chessboard : MonoBehaviour
{
    public GameObject ChessboardRect;
    public GameObject ChessboardCellPrefab;
    public GameObject ChessboardDialog;
    public GameObject CanvasObject;
    public GameObject Background;               // Background所在的Panel

    public static bool UnitMove = false;        // 是否有单位正在移动

    public static Cell SelectedCell;
    public static GameObject SelectedCard;
    public static float CellSize;

    public static bool RecordingMovePath;
    public static List<ChessboardPosition> ListMovePath = new List<ChessboardPosition>();

    private static Chessboard singleton = null;
    private Cell[,] cellArray = new Cell[12, 8];
    public const int ChessboardMaxX = 12;
    public const int ChessboardMaxY = 8;

    public const int BlueSideSummonColumn = 0;                      // 蓝色方可以召唤的列
    public const int RedSideSummonColumn = ChessboardMaxX - 1;      // 红色方可以召唤的列

    private RectTransform _rectTrans;
    private GameObject _bgLayer;
    private GameObject _unitLayer;
    private GameObject _uiLayer;

    private int[] _showRangeList;
    private int[] _moveRange;
    private int[] _movePaths;
    /// <summary>
    /// 当前是否正在显示移动范围
    /// </summary>
    private bool _isShowingRange;
    /// <summary>
    /// 当前是否正在显示移动范围
    /// </summary>
    public bool isShowingMoveRange
    {
        get { return this._isShowingRange;  }
    }

    /// <summary>
    /// 记录BattleField里对应的层
    /// </summary>
    private Dictionary<int, GameObject> _layersMap;

    public static Chessboard GetInstance()
    {
        return singleton;
    }

    /// <summary>
    /// 得到x，y坐标表示的cell
    /// </summary>
    /// <param name="position">左下角为(0, 0)</param>
    /// <returns></returns>
    public static Cell GetCell(ChessboardPosition position)
    {
        try
        {
            return GetInstance().cellArray[position.x, position.y];
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// 将每个格子的背景设为黑色
    /// </summary>
    public static void ClearBackground()
    {
        foreach (var it in singleton.cellArray)
            it.SetBackgroundColor(Color.black);
    }

    // Use this for initialization
    void Start()
    {
        //Init
        singleton = this;
    }

    /// <summary>
    /// 初始化地图基本单元格
    /// </summary>
    public void init()
    {
        //this._rectTrans = this.GetComponent<RectTransform>();
        ////RectTransform anchor = ChessboardRect.GetComponent<RectTransform>();
        //Vector2 chessboardSize = new Vector2(Screen.width * (this._rectTrans.anchorMax.x - this._rectTrans.anchorMin.x),
        //    Screen.height * (this._rectTrans.anchorMax.y - this._rectTrans.anchorMin.y));
        //CellSize = Math.Min(chessboardSize.x / ChessboardMaxX, chessboardSize.y / ChessboardMaxY);
        //BattleGlobal.CellStartX = 0;
        //BattleGlobal.CellStartY = 0;
        //float cellSize = Math.Min(chessboardSize.x / ChessboardMaxX, chessboardSize.y / ChessboardMaxY); ;
        ////BattleGlobal.CellSize = Math.Min(chessboardSize.x / ChessboardMaxX, chessboardSize.y / ChessboardMaxY);
        //BattleGlobal.Scale = cellSize / BattleConsts.DefaultCellSize;
        BattleGlobal.CellSize = BattleConsts.DefaultCellSize;
        BattleGlobal.Scale = 1f;
        this.initLayers();
        GameObject cellGo;
        Cell cell;
        for (int x = 0; x < ChessboardMaxX; ++x)
        {
            for (int y = 0; y < ChessboardMaxY; ++y)
            {
                cellGo = Instantiate(Resources.Load("Prefabs/ChessboardCellPrefab")) as GameObject;
                cell = cellGo.GetComponent<Cell>();
                cellArray[x, y] = cell;
                cell.transform.SetParent(this._bgLayer.transform);
                cell.transform.localScale = Vector3.one;
                cell.location = new ChessboardPosition(x, y);
                //每个格子的背景
                //cell.Position = new ChessboardPosition(x, y);
                //cell.Background = Instantiate(ChessboardCellPrefab.transform.FindChild("Background").gameObject);
                //cell.Background.name = x + " " + y;
                //cell.Background.transform.SetParent(Background.transform);
                cell.transform.localPosition = BattleSceneUtils.getCellPosByLocation(cell.location.y, cell.location.x);
                //cell.transform.localScale = new Vector3(CellSize / 75, CellSize / 75, 1);
            }
        }
        this.addClickEventHandler(this.testCell);
        this.addEnterEventHandler(this.onCellEnterHandler);
        this.addExitEventHandler(this.onCellExitHandler);

        for (int x = 0; x < ChessboardMaxX; ++x)
            for (int y = 0; y < ChessboardMaxY; ++y)
            {
                cell = cellArray[x, y];
                if (y == 0)
                {
                    if ((x + y) % 2 == 0)
                        SkillOperate.SummonUnit(cell, 1, EGroupType.BlueSide);
                    else
                        SkillOperate.SummonUnit(cell, 1, EGroupType.RedSide);
                }
            }
    }

    public void testMove(GameObject go)
    {
        Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(this.transform.InverseTransformPoint(vec));
        //this.transform.InverseTransformVector
    }

    /// <summary>
    /// 初始化BattleField上的层
    /// </summary>
    private void initLayers()
    {
        // 获取层
        this._bgLayer = this.transform.FindChild("BgLayer").gameObject;
        this._unitLayer = this.transform.FindChild("UnitLayer").gameObject;
        this._uiLayer = this.transform.FindChild("UILayer").gameObject;
        // 设置全局缩放
        this._bgLayer.transform.localScale = Vector3.one * BattleGlobal.Scale;
        this._unitLayer.transform.localScale = Vector3.one * BattleGlobal.Scale;
        this._uiLayer.transform.localScale = Vector3.one * BattleGlobal.Scale;
        // 添加到Map
        this._layersMap = new Dictionary<int, GameObject>();
        this._layersMap.Add(BattleConsts.BattleFieldLayer_Bg, this._bgLayer);
        this._layersMap.Add(BattleConsts.BattleFieldLayer_Unit, this._unitLayer);
        this._layersMap.Add(BattleConsts.BattleFieldLayer_UI, this._uiLayer);
    }

    public void addClickEventHandler(UIEventListener.UIEventHandler eventHandler)
    {
        int i, j;
        int rowLimit = BattleConsts.MapMaxRow;
        int colLimit = BattleConsts.MapMaxCol;
        for (i = 0; i < rowLimit; i++)
        {
            for (j = 0; j < colLimit; j++)
            {
                UIEventListener.Get(cellArray[j, i].gameObject).onClick += eventHandler;
            }
        }
    }

    public void addEnterEventHandler(UIEventListener.UIEventHandler eventHandler)
    {
        int i, j;
        int rowLimit = BattleConsts.MapMaxRow;
        int colLimit = BattleConsts.MapMaxCol;
        for (i = 0; i < rowLimit; i++)
        {
            for (j = 0; j < colLimit; j++)
            {
                UIEventListener.Get(cellArray[j, i].gameObject).onEnter += eventHandler;
            }
        }
    }

    public void addExitEventHandler(UIEventListener.UIEventHandler eventHandler)
    {
        int i, j;
        int rowLimit = BattleConsts.MapMaxRow;
        int colLimit = BattleConsts.MapMaxCol;
        for (i = 0; i < rowLimit; i++)
        {
            for (j = 0; j < colLimit; j++)
            {
                UIEventListener.Get(cellArray[j, i].gameObject).onExit += eventHandler;
            }
        }
    }

    public void removeClickEventHandler(UIEventListener.UIEventHandler eventHandler)
    {
        int i, j;
        int rowLimit = BattleConsts.MapMaxRow;
        int colLimit = BattleConsts.MapMaxCol;
        for (i = 0; i < rowLimit; i++)
        {
            for (j = 0; j < colLimit; j++)
            {
                UIEventListener.Get(cellArray[j, i].gameObject).onClick -= eventHandler;
            }
        }
    }

    public void removeEnterEventHandler(UIEventListener.UIEventHandler eventHandler)
    {
        int i, j;
        int rowLimit = BattleConsts.MapMaxRow;
        int colLimit = BattleConsts.MapMaxCol;
        for (i = 0; i < rowLimit; i++)
        {
            for (j = 0; j < colLimit; j++)
            {
                UIEventListener.Get(cellArray[j, i].gameObject).onEnter -= eventHandler;
            }
        }
    }

    public void removeExitEventHandler(UIEventListener.UIEventHandler eventHandler)
    {
        int i, j;
        int rowLimit = BattleConsts.MapMaxRow;
        int colLimit = BattleConsts.MapMaxCol;
        for (i = 0; i < rowLimit; i++)
        {
            for (j = 0; j < colLimit; j++)
            {
                UIEventListener.Get(cellArray[j, i].gameObject).onExit -= eventHandler;
            }
        }
    }

    private void testCell(GameObject go)
    {
        Cell cell = go.GetComponent<Cell>();
        //Debug.Log(cell.location.y + " " + cell.location.x);
    }

    private void onCellEnterHandler(GameObject go)
    {
        Cell cell = go.GetComponent<Cell>();
        cell.SetBackgroundColor(Cell.SelectedColor);
    }

    private void onCellExitHandler(GameObject go)
    {
        Cell cell = go.GetComponent<Cell>();
        cell.SetBackgroundColor(Color.black);
    }

    /// <summary>
    /// 添加对象到指定层
    /// </summary>
    /// <param name="child"></param>
    /// <param name="layerIndex"></param>
    public void addChildOnLayer(GameObject child, int layerIndex)
    {
        GameObject layer;
        if (this._layersMap.TryGetValue(layerIndex, out layer))
        {
            Vector3 tmpScale = child.transform.localScale;
            child.transform.SetParent(layer.transform);
            child.transform.localScale = tmpScale;
            child.transform.SetAsLastSibling();
        }
    }

    /// <summary>
    /// 添加对象到指定层的对应行列位置
    /// </summary>
    /// <param name="child">对象</param>
    /// <param name="layerIndex">层的索引</param>
    /// <param name="row">行</param>
    /// <param name="col">列</param>
    public void addChildOnLayer(GameObject child,int layerIndex,int row,int col)
    {
        GameObject layer;
        if (this._layersMap.TryGetValue(layerIndex, out layer))
        {
            Vector3 tmpScale = child.transform.localScale;
            child.transform.SetParent(layer.transform);
            child.transform.localScale = tmpScale;
            child.transform.SetAsLastSibling();
            child.transform.localPosition = BattleSceneUtils.getCellPosByLocation(row, col);
        }
    }
    /// <summary>
    /// 根据行列拿到对应的单元格
    /// </summary>
    /// <param name="row">行</param>
    /// <param name="col">列</param>
    /// <returns></returns>
    public Cell getCellByPos(int row,int col)
    {
        if ( row >= BattleConsts.MapMaxRow || row < 0 || col >= BattleConsts.MapMaxCol || col < 0 )
        {
            return null;
        }
        return this.cellArray[col, row];
    }
    /// <summary>
    /// 根据位置拿到对应的单元格
    /// </summary>
    /// <param name="pos">一维数组的对应位置下标</param>
    /// <returns></returns>
    public Cell getCellByPos(int pos)
    {
        return getCellByPos(pos / BattleConsts.MapMaxCol, pos % BattleConsts.MapMaxCol);
    }

    public Unit getUnitByPos(int row,int col)
    {
        Cell cell = this.getCellByPos(row, col);
        return cell == null || cell.UnitOnCell == null ? null : cell.UnitOnCell;
    }

    public Unit getUnitByPos(int pos)
    {
        Cell cell = this.getCellByPos(pos);
        return cell == null || cell.UnitOnCell == null ? null : cell.UnitOnCell;
    }

    /// <summary>
    /// 显示移动范围
    /// </summary>
    /// <param name="isShow">是否显示，为false表示取消</param>
    /// <param name="range">移动范围的一维数组</param>
    public void showAvailableMoveRange(bool isShow,int[] range=null)
    {
        int tmpRow, tmpCol,i,j;
        Cell cell;
        int rowLimit = BattleConsts.MapMaxRow;
        int colLimit = BattleConsts.MapMaxCol;
        // 判断是否需要显示移动范围
        for (i = 0; i < rowLimit; i++)
        {
            for (j = 0; j < colLimit; j++)
            {
                cell = cellArray[j, i];
                cell.setRangeColor(Cell.DefaultColor);
                cell.activeRangeImg(isShow);
            }
        }
        this._isShowingRange = isShow;
        if ( !isShow )
        {
            this._movePaths = null;
            this._moveRange = null;
            return;
        }
        int len = range.Length;
        this._moveRange = range;
        for (i=0;i< len;i++)
        {
            if ( this._moveRange[i] >= 0 )
            {
                tmpRow = i / colLimit;
                tmpCol = i % colLimit;
                cell = this.cellArray[tmpCol, tmpRow];
                cell.setRangeColor(Cell.MovableColor);
            }
        }
    }

    public void showMovePath(int[] paths)
    {
        if ( !this._isShowingRange )
        {
            return;
        }
        int colLimit = BattleConsts.MapMaxCol;
        int tmpRow, tmpCol,len,posIndex,i;
        Cell cell;
        // 还原之前的颜色
        if ( this._movePaths != null )
        {
            len = this._movePaths.Length;
            for (i=0;i< len;i++)
            {
                posIndex = this._movePaths[i];
                tmpRow = posIndex / colLimit;
                tmpCol = posIndex % colLimit;
                cell = this.cellArray[tmpCol, tmpRow];
                cell.setRangeColor(Cell.MovableColor);
            }
        }
        // 高亮选中的路径
        this._movePaths = paths;
        if (this._movePaths != null)
        {
            len = this._movePaths.Length;
            for (i = 0; i < len; i++)
            {
                posIndex = this._movePaths[i];
                tmpRow = posIndex / colLimit;
                tmpCol = posIndex % colLimit;
                cell = this.cellArray[tmpCol, tmpRow];
                cell.setRangeColor(Cell.HighLightMovableColor);
            }
        }
    }

    /// <summary>
    /// 根据曼哈顿距离显示范围
    /// </summary>
    /// <param name="isShow"></param>
    /// <param name="centerRow"></param>
    /// <param name="centerCol"></param>
    /// <param name="dis"></param>
    public void showRangeByManhattanDis(int centerRow,int centerCol,int dis)
    {
        this.showRangeByManhattanDis(centerRow, centerCol, 0, dis);
    }

    /// <summary>
    /// 根据曼哈顿距离显示范围
    /// </summary>
    /// <param name="centerRow"></param>
    /// <param name="centerCol"></param>
    /// <param name="minDis"></param>
    /// <param name="maxDis"></param>
    public void showRangeByManhattanDis(int centerRow, int centerCol, int minDis,int maxDis)
    {
        this.activeRangeShow(true);
        int tmpDis,tmpRow, tmpCol, i, j;
        int rowLimit = BattleConsts.MapMaxRow;
        int colLimit = BattleConsts.MapMaxCol;
        int len = rowLimit * colLimit;
        int[] rangeList = new int[len];
        for (i=0;i< len;i++)
        {
            rangeList[i] = 0;
        }
        for (tmpDis=minDis;tmpDis<=maxDis;tmpDis++)
        {
            for (i=-tmpDis;i<=tmpDis;i++)
            {
                tmpRow = centerRow + i;
                if (tmpRow < rowLimit && tmpRow >= 0)
                {
                    tmpCol = tmpDis - Math.Abs(i);
                    if ( tmpCol >= colLimit || tmpCol < 0 )
                    {
                        continue;
                    }
                    rangeList[tmpRow * colLimit + tmpCol] = 1;
                    tmpCol = -tmpCol;
                    if (tmpCol >= colLimit || tmpCol < 0)
                    {
                        continue;
                    }
                    rangeList[tmpRow * colLimit + tmpCol] = 1;
                }
            }
        }
        this.showRangeByRangeList(rangeList);
    }

    public void showRangeByLineDis(int centerRow,int centerCol,int dis)
    {
        this.showRangeByLineDis(centerRow, centerCol, 0, dis, BattleConsts.LINE_FLAG_CROSS);
    }

    public void showRangeByLineDis(int centerRow, int centerCol,int minDis,int maxDis,int lineFlag)
    {
        this.activeRangeShow(true);
        int tmpRow, tmpCol, i, j;
        int rowLimit = BattleConsts.MapMaxRow;
        int colLimit = BattleConsts.MapMaxCol;
        int len = rowLimit * colLimit;
        int[] rangeList = new int[len];
        for (i = 0; i < len; i++)
        {
            rangeList[i] = 0;
        }
        // 横向直线
        if ( (lineFlag & BattleConsts.LINE_FLAG_HORIZON) != 0 )
        {
            for (i = minDis; i <= maxDis; i++)
            {
                tmpRow = centerRow;
                tmpCol = centerCol - i;
                if (tmpCol < colLimit && tmpCol >= 0)
                {
                    rangeList[tmpRow * colLimit + tmpCol] = 1;
                }
                tmpCol = centerCol + i;
                if (tmpCol < colLimit && tmpCol >= 0)
                {
                    rangeList[tmpRow * colLimit + tmpCol] = 1;
                }
            }
        }
        // 纵向直线
        if ( (lineFlag & BattleConsts.LINE_FLAG_VERTICAL) != 0 )
        {
            for (i = minDis; i <= maxDis; i++)
            {
                tmpRow = centerRow - i;
                tmpCol = centerCol;
                if ( tmpRow < rowLimit && tmpRow >= 0 )
                {
                    rangeList[tmpRow * colLimit + tmpCol] = 1;
                }
                tmpRow = centerRow + i;
                if (tmpRow < rowLimit && tmpRow >= 0)
                {
                    rangeList[tmpRow * colLimit + tmpCol] = 1;
                }
            }
        }
        this.showRangeByRangeList(rangeList);
    }

    public void showRangeByRangeList(int[] rangeList)
    {
        int tmpRow, tmpCol, i, j;
        Cell cell;
        int rowLimit = BattleConsts.MapMaxRow;
        int colLimit = BattleConsts.MapMaxCol;
        int len = rowLimit * colLimit;
        this._showRangeList = rangeList;
        for (i = 0; i < len; i++)
        {
            if (rangeList[i] > 0)
            {
                tmpRow = i / colLimit;
                tmpCol = i % colLimit;
                cell = this.cellArray[tmpCol, tmpRow];
                cell.setRangeColor(Cell.MovableColor);
            }
        }
    }

    /// <summary>
    /// 激活/禁用所有范围显示
    /// </summary>
    /// <param name="active">true为激活</param>
    public void activeRangeShow(bool active)
    {
        int i, j;
        Cell cell;
        int rowLimit = BattleConsts.MapMaxRow;
        int colLimit = BattleConsts.MapMaxCol;
        for (i = 0; i < rowLimit; i++)
        {
            for (j = 0; j < colLimit; j++)
            {
                cell = cellArray[j, i];
                cell.setRangeColor(Cell.DefaultColor);
                cell.activeRangeImg(active);
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="position">左下为(0, 0)</param>
    /// <returns></returns>
    private Vector2 GetCellPosition(ChessboardPosition position)
    {
        const float pivot = 0.5F;
        RectTransform anchor = ChessboardRect.GetComponent<RectTransform>();
        return new Vector2(anchor.localPosition.x + (position.x + pivot) * CellSize,
            anchor.localPosition.y - (ChessboardMaxY - position.y - pivot) * CellSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (UnitMove)
            SelectedCell.UnitOnCell.MoveWithPath(ListMovePath);
    }

    /// <summary>
    /// 记录selected cell的移动路径
    /// </summary>
    /// <param name="targetPosition"></param>
    public static void RecordMovePath(ChessboardPosition targetPosition)
    {
        Debug.Log("Record");
        int index;
        if (!RecordingMovePath)
        {
            //当移到初始点时开始记录移动路径
            if (SelectedCell.Position.Distance(targetPosition) == 0)
            {
                GetCell(targetPosition).SetBackgroundColor(Cell.HighLightMovableColor);
                RecordingMovePath = true;
            }
        }
        else
        {
            if (GetCell(targetPosition).UnitOnCell != null) return;              //格子上已有单位
            index = ListMovePath.IndexOf(targetPosition);
            if (ListMovePath.Count >= SelectedCell.UnitOnCell.UnitAttribute.motility && index == -1) return;            //超过单位的机动
            if (ListMovePath.Count == 0)
            {
                //应与初始点相邻
                if (SelectedCell.Position.Adjacent(targetPosition))
                {
                    GetCell(targetPosition).SetBackgroundColor(Cell.HighLightMovableColor);
                    ListMovePath.Add(targetPosition);
                }
                else {
                    return;
                }
            }
            else if (index == -1 && ListMovePath[ListMovePath.Count - 1].Adjacent(targetPosition))
            {
                //应与上一个选择的点相邻
                Chessboard.GetCell(targetPosition).SetBackgroundColor(Cell.HighLightMovableColor);
                ListMovePath.Add(targetPosition);
            }
            else if ((index = ListMovePath.IndexOf(targetPosition)) != -1)
            {
                ChessboardPosition lastPos = ListMovePath[ListMovePath.Count - 1];
                if (lastPos.x != targetPosition.x || lastPos.y != targetPosition.y)
                {
                    for (int i = index + 1; i < ListMovePath.Count;)
                    {
                        lastPos = ListMovePath[i];
                        ListMovePath.RemoveAt(index);
                        Chessboard.GetCell(lastPos).SetBackgroundColor(Cell.MovableColor);
                    }
                }
            }
        }
    }

    public static void ClearMovePath()
    {
        RecordingMovePath = false;
        ListMovePath.Clear();
    }

    public static void SetDialogString(string message)
    {
        var dialog = Chessboard.GetInstance().ChessboardDialog;
        dialog.transform.Find("Text").GetComponent<Text>().text = message;
    }

    /// <summary>
    /// 显示或隐藏对话框。实际是修改了DialogObject的Active属性
    /// </summary>
    /// <param name="visible"></param>
    public static void SetChessboardDialogVisible(bool visible)
    {
        Chessboard.GetInstance().ChessboardDialog.SetActive(visible);
    }

    public static void SetSkillRange(int range)
    {
        throw new NotImplementedException();
    }

    public static void SetSkillRangeVisible(bool visible)
    {
        throw new NotImplementedException();
    }

    public static void SetSkillTargetRange(int range)
    {
        throw new NotImplementedException();
    }

    public static void SetSkillTargetRangeVisible(int visible)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 显示范围，根据传入的函数判断格子是否需要改变颜色
    /// </summary>
    /// <param name="isChangeColor"></param>
    /// <param name="color"></param>
    public static void SetBackgroundColor(Func<Cell, bool> isChangeColor, Color color)
    {
        for (int x = 0; x < ChessboardMaxX; ++x)
            for (int y = 0; y < ChessboardMaxY; ++y)
            {
                var cell = GetCell(new ChessboardPosition(x, y));
                if (isChangeColor(cell))
                    cell.SetBackgroundColor(color);
            }
    }
}