using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Threading;

[System.Runtime.InteropServices.Guid("8E332A3A-1CC1-4F2D-A30A-A9BCE2D9E5DD")]
public static class Chessboard
{

	//public static Mutex DialogMutex;

	public static bool UnitMove = false;        // 是否有单位正在移动

	public static Cell SelectedCell;
	public static GameObject SelectedCard;
	public static float CellSize;

	public static bool RecordingMovePath;
	public static List<ChessboardPosition> ListMovePath = new List<ChessboardPosition>();

	private static Cell[,] cellArray = new Cell[12, 8];
	//public const int ChessboardMaxX = 12;
	//public const int ChessboardMaxY = 8;

	//public const int BlueSideSummonColumn = 0;                      // 蓝色方可以召唤的列
	//public const int RedSideSummonColumn = ChessboardMaxX - 1;      // 红色方可以召唤的列

	private static GameObject _bgLayer;
	private static GameObject _unitLayer;
	private static GameObject _uiLayer;
	/// <summary>
	/// 选择框
	/// </summary>
	private static GameObject _selectBox;
	/// <summary>
	/// 是否显示选择框
	/// </summary>
	private static bool _showSelectBox;

	private static int[] _showRangeList;
	private static int[] _moveRange;
	private static int[] _movePaths;
	/// <summary>
	/// 当前是否正在显示移动范围
	/// </summary>
	private static bool _isShowingRange;
	/// <summary>
	/// 当前是否正在显示移动范围
	/// </summary>
	public static bool isShowingMoveRange
	{
		get { return _isShowingRange;  }
	}

	/// <summary>
	/// 记录BattleField里对应的层
	/// </summary>
	private static Dictionary<int, GameObject> _layersMap;

	/// <summary>
	/// 得到x，y坐标表示的cell
	/// </summary>
	/// <param name="position">左下角为(0, 0)</param>
	/// <returns></returns>
	public static Cell GetCell(ChessboardPosition position)
	{
		try
		{
			return cellArray[position.col, position.row];
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
		foreach (var it in cellArray)
			it.SetBackgroundColor(Color.black);
	}

	/// <summary>
	/// 初始化地图基本单元格
	/// </summary>
	public static void init()
	{
		//_rectTrans = GetComponent<RectTransform>();
		////RectTransform anchor = ChessboardRect.GetComponent<RectTransform>();
		//Vector2 chessboardSize = new Vector2(Screen.width * (_rectTrans.anchorMax.x - _rectTrans.anchorMin.x),
		//    Screen.height * (_rectTrans.anchorMax.y - _rectTrans.anchorMin.y));
		//CellSize = Math.Min(chessboardSize.x / ChessboardMaxX, chessboardSize.y / ChessboardMaxY);
		//BattleGlobal.CellStartX = 0;
		//BattleGlobal.CellStartY = 0;
		//float cellSize = Math.Min(chessboardSize.x / ChessboardMaxX, chessboardSize.y / ChessboardMaxY); ;
		////BattleGlobal.CellSize = Math.Min(chessboardSize.x / ChessboardMaxX, chessboardSize.y / ChessboardMaxY);
		//BattleGlobal.Scale = cellSize / BattleConsts.DefaultCellSize;
		BattleGlobal.CellSize = BattleConsts.DefaultCellSize;
		BattleGlobal.Scale = 1f;
		initLayers();
		GameObject cellGo;
		Cell cell;
		for (int x = 0; x < BattleConsts.MapMaxCol; ++x)
		{
			for (int y = 0; y < BattleConsts.MapMaxRow; ++y)
			{
				cellGo = GameObject.Instantiate(Resources.Load("Prefabs/ChessboardCellPrefab")) as GameObject;
				cell = cellGo.GetComponent<Cell>();
				cellArray[x, y] = cell;
				cell.transform.SetParent(_bgLayer.transform);
				cell.transform.localScale = Vector3.one;
				cell.location = new ChessboardPosition(x, y);
				//每个格子的背景
				//cell.Position = new ChessboardPosition(x, y);
				//cell.Background = Instantiate(ChessboardCellPrefab.transform.FindChild("Background").gameObject);
				//cell.Background.name = x + " " + y;
				//cell.Background.transform.SetParent(Background.transform);
				cell.transform.localPosition = BattleSceneUtils.getCellPosByLocation(cell.location.row, cell.location.col);
				//cell.transform.localScale = new Vector3(CellSize / 75, CellSize / 75, 1);
			}
		}
		addClickEventHandler(testCell);
		addEnterEventHandler(onCellEnterHandler);
		addExitEventHandler(onCellExitHandler);

		for (int x = 0; x < BattleConsts.MapMaxCol; ++x)
			for (int y = 0; y < BattleConsts.MapMaxRow; ++y)
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

	public static void testMove(GameObject go)
	{
		Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//Debug.Log(transform.InverseTransformPoint(vec));
		//transform.InverseTransformVector
	}

	/// <summary>
	/// 初始化BattleField上的层
	/// </summary>
	private static void initLayers()
	{
		var chessboardObject = ChessboardObject.GetInstance();
		// 获取层
		_bgLayer = chessboardObject.BgLayer;
		_unitLayer = chessboardObject.UnitLayer;
		_uiLayer = chessboardObject.UILayer;
		// 设置全局缩放
		_bgLayer.transform.localScale = Vector3.one * BattleGlobal.Scale;
		_unitLayer.transform.localScale = Vector3.one * BattleGlobal.Scale;
		_uiLayer.transform.localScale = Vector3.one * BattleGlobal.Scale;
		// 添加到Map
		_layersMap = new Dictionary<int, GameObject>();
		_layersMap.Add(BattleConsts.BattleFieldLayer_Bg, _bgLayer);
		_layersMap.Add(BattleConsts.BattleFieldLayer_Unit, _unitLayer);
		_layersMap.Add(BattleConsts.BattleFieldLayer_UI, _uiLayer);
		// 初始化selectbox
		_showSelectBox = false;
		_selectBox = _uiLayer.transform.FindChild("SelectBox").gameObject;
	}

	public static void addClickEventHandler(UIEventListener.UIEventHandler eventHandler)
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

	public static void addEnterEventHandler(UIEventListener.UIEventHandler eventHandler)
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

	public static void addExitEventHandler(UIEventListener.UIEventHandler eventHandler)
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

	public static void removeClickEventHandler(UIEventListener.UIEventHandler eventHandler)
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

	public static void removeEnterEventHandler(UIEventListener.UIEventHandler eventHandler)
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

	public static void removeExitEventHandler(UIEventListener.UIEventHandler eventHandler)
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

	private static void testCell(GameObject go)
	{
		Cell cell = go.GetComponent<Cell>();
		//Debug.Log(cell.location.y + " " + cell.location.x);
	}

	private static void onCellEnterHandler(GameObject go)
	{
		Cell cell = go.GetComponent<Cell>();
		//cell.SetBackgroundColor(Cell.SelectedColor);
		if ( !_showSelectBox )
		{
			_showSelectBox = true;
			_selectBox.SetActive(true);
		}
		_selectBox.transform.localPosition = BattleSceneUtils.getCellPosByLocation(cell.location.row, cell.location.col);
	}

	private static void onCellExitHandler(GameObject go)
	{
		Cell cell = go.GetComponent<Cell>();
		//cell.SetBackgroundColor(Color.black);
	}

	/// <summary>
	/// 添加对象到指定层
	/// </summary>
	/// <param name="child"></param>
	/// <param name="layerIndex"></param>
	public static void addChildOnLayer(GameObject child, int layerIndex)
	{
		GameObject layer;
		if (_layersMap.TryGetValue(layerIndex, out layer))
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
	public static void addChildOnLayer(GameObject child,int layerIndex,int row,int col)
	{
		GameObject layer;
		if (_layersMap.TryGetValue(layerIndex, out layer))
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
	public static Cell getCellByPos(int row,int col)
	{
		if ( row >= BattleConsts.MapMaxRow || row < 0 || col >= BattleConsts.MapMaxCol || col < 0 )
		{
			return null;
		}
		return cellArray[col, row];
	}
	/// <summary>
	/// 根据位置拿到对应的单元格
	/// </summary>
	/// <param name="pos">一维数组的对应位置下标</param>
	/// <returns></returns>
	public static Cell getCellByPos(int pos)
	{
		return getCellByPos(pos / BattleConsts.MapMaxCol, pos % BattleConsts.MapMaxCol);
	}

	public static Unit getUnitByPos(int row,int col)
	{
		Cell cell = getCellByPos(row, col);
		return cell == null || cell.UnitOnCell == null ? null : cell.UnitOnCell;
	}

	public static Unit getUnitByPos(int pos)
	{
		Cell cell = getCellByPos(pos);
		return cell == null || cell.UnitOnCell == null ? null : cell.UnitOnCell;
	}

	/// <summary>
	/// 显示移动范围
	/// </summary>
	/// <param name="isShow">是否显示，为false表示取消</param>
	/// <param name="range">移动范围的一维数组</param>
	public static void showAvailableMoveRange(bool isShow,int[] range=null)
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
		_isShowingRange = isShow;
		if ( !isShow )
		{
			_movePaths = null;
			_moveRange = null;
			return;
		}
		int len = range.Length;
		_moveRange = range;
		for (i=0;i< len;i++)
		{
			if ( _moveRange[i] >= 0 )
			{
				tmpRow = i / colLimit;
				tmpCol = i % colLimit;
				cell = cellArray[tmpCol, tmpRow];
				cell.setRangeColor(Cell.MovableColor);
			}
		}
	}

	public static void showMovePath(int[] paths)
	{
		if ( !_isShowingRange )
		{
			return;
		}
		int colLimit = BattleConsts.MapMaxCol;
		int tmpRow, tmpCol,len,posIndex,i;
		Cell cell;
		// 还原之前的颜色
		if ( _movePaths != null )
		{
			len = _movePaths.Length;
			for (i=0;i< len;i++)
			{
				posIndex = _movePaths[i];
				tmpRow = posIndex / colLimit;
				tmpCol = posIndex % colLimit;
				cell = cellArray[tmpCol, tmpRow];
				cell.setRangeColor(Cell.MovableColor);
			}
		}
		// 高亮选中的路径
		_movePaths = paths;
		if (_movePaths != null)
		{
			len = _movePaths.Length;
			for (i = 0; i < len; i++)
			{
				posIndex = _movePaths[i];
				tmpRow = posIndex / colLimit;
				tmpCol = posIndex % colLimit;
				cell = cellArray[tmpCol, tmpRow];
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
	public static void showRangeByManhattanDis(int centerRow,int centerCol,int dis)
	{
		showRangeByManhattanDis(centerRow, centerCol, 0, dis);
	}

	/// <summary>
	/// 根据曼哈顿距离显示范围
	/// </summary>
	/// <param name="centerRow"></param>
	/// <param name="centerCol"></param>
	/// <param name="minDis"></param>
	/// <param name="maxDis"></param>
	public static void showRangeByManhattanDis(int centerRow, int centerCol, int minDis,int maxDis)
	{
		int rowLimit = BattleConsts.MapMaxRow;
		int colLimit = BattleConsts.MapMaxCol;
		int len = rowLimit * colLimit;
		int[] rangeList = new int[len];
		for (int i=0;i< len;i++)
		{
			rangeList[i] = 0;
		}

		ChessboardPosition center = new ChessboardPosition(centerCol, centerRow);
		for (int col = 0; col < colLimit; ++col)
		{
			for (int row = 0; row < rowLimit; ++row)
			{
				int distance = center.Distance(new ChessboardPosition(col, row));
				if (minDis <= distance && distance <= maxDis)
					rangeList[row*colLimit + col] = 1;
			}
		}

		showRangeByRangeList(rangeList);
	}

	public static void showRangeByLineDis(int centerRow,int centerCol,int dis)
	{
		showRangeByLineDis(centerRow, centerCol, 0, dis, (int)BattleConsts.LineFlag.Cross);
	}

	public static void showRangeByLineDis(int centerRow, int centerCol,int minDis,int maxDis,int lineFlag)
	{
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
		if ( (lineFlag & (int)BattleConsts.LineFlag.Horizon) != 0 )
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
		if ( (lineFlag & (int)BattleConsts.LineFlag.Vertical) != 0 )
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
		showRangeByRangeList(rangeList);
	}

	public static void showRangeByRangeList(int[] rangeList)
	{
		activeRangeShow(true);
		int tmpRow, tmpCol, i, j;
		Cell cell;
		int rowLimit = BattleConsts.MapMaxRow;
		int colLimit = BattleConsts.MapMaxCol;
		int len = rowLimit * colLimit;
		_showRangeList = rangeList;
		for (i = 0; i < len; i++)
		{
			if (rangeList[i] > 0)
			{
				tmpRow = i / colLimit;
				tmpCol = i % colLimit;
				cell = cellArray[tmpCol, tmpRow];
				cell.setRangeColor(Cell.MovableColor);
			}
		}
	}

	/// <summary>
	/// 激活/禁用所有范围显示
	/// </summary>
	/// <param name="active">true为激活</param>
	public static void activeRangeShow(bool active)
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

	public static void showSelectBox(bool isShow)
	{
		_showSelectBox = isShow;
		_selectBox.SetActive(isShow);
	}

	/// <summary>
	/// </summary>
	/// <param name="position">左下为(0, 0)</param>
	/// <returns></returns>
	private static Vector2 GetCellPosition(ChessboardPosition position)
	{
		const float pivot = 0.5F;
		RectTransform anchor = ChessboardObject.GetInstance().GetComponent<RectTransform>();
		return new Vector2(anchor.localPosition.x + (position.col + pivot) * CellSize,
			anchor.localPosition.y - (BattleConsts.MapMaxRow - position.row - pivot) * CellSize);
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
				if (lastPos.col != targetPosition.col || lastPos.row != targetPosition.row)
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
		for (int x = 0; x < BattleConsts.MapMaxCol; ++x)
			for (int y = 0; y < BattleConsts.MapMaxRow; ++y)
			{
				var cell = GetCell(new ChessboardPosition(x, y));
				if (isChangeColor(cell))
					cell.SetBackgroundColor(color);
			}
	}
}