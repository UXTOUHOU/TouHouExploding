using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class Cell : MonoBehaviour
{
    public GameObject Obj;
    public GameObject Background;
    public Unit UnitOnCell;
    private ChessboardPosition _location;
    public ChessboardPosition location
    {
        get { return this._location; }
        set
        {
            this._location = value;
            // 暂用，之后废置
            this.Position = this._location;
        }
    }
    public ChessboardPosition Position;

    public static Color DefaultColor = new Color(0F, 0F, 0F);
    public static Color SelectedColor = new Color(10F / 255, 23F / 255, 96F / 255);
    public static Color MovableColor = new Color(70F / 255, 112F / 255, 70F / 255);
    public static Color AttackableColor = new Color(149F / 255, 70F / 255, 70F / 255);
    public static Color HighLightMovableColor = new Color(100F / 255, 142F / 255, 100F / 255);

    public static GameObject btnMove;
    public static GameObject btnAttack;
    public static GameObject btnSkill;

    public static GameObject btnSkill_1;
    public static GameObject btnSkill_2;
    public static GameObject btnSkill_3;

    private Image _bgImg;
    private Image _rangeImg;

    /// <summary>
    /// 一维数组中的位置索引
    /// </summary>
    public int posIndex
    {
        get { return this._location.y * BattleConsts.MapMaxCol + this._location.x; }
    }

    /*private int _row;
    public int row
    {
        get { return this._row; }
    }

    private int _col;
    public int col
    {
        get { return this._col; }
    }

    /// <summary>
    /// 设置单元格的位置
    /// </summary>
    /// <param name="row">行</param>
    /// <param name="col">列</param>
    public void setCellLocation(int row,int col)
    {
        this._row = row;
        this._col = col;
    }*/

    public void addListener()
    {
        UIEventListener.Get(this.gameObject).onClick += cellClickHandler;
        UIEventListener.Get(this.gameObject).onEnter += cellEnterHandler;
        UIEventListener.Get(this.gameObject).onExit += cellExitHandler;
    }

    public void removeListener()
    {
        UIEventListener.Get(this.gameObject).onClick -= cellClickHandler;
        UIEventListener.Get(this.gameObject).onEnter -= cellEnterHandler;
        UIEventListener.Get(this.gameObject).onExit -= cellExitHandler;
    }

    public void cellClickHandler(GameObject cell)
    {
        //CommandManager.getInstance().runCommand(CommandConsts.CommandConsts_OnCellClick, this._row, this._col);
    }

    public void cellEnterHandler(GameObject cell)
    {
        //CommandManager.getInstance().runCommand(CommandConsts.CommandConsts_OnCellEnter, this._row, this._col);
    }

    public void cellExitHandler(GameObject cell)
    {
        //CommandManager.getInstance().runCommand(CommandConsts.CommandConsts_OnCellExit, this._row, this._col);
    }


    public void ShowOperateButton()
    {
        float cellSize = Chessboard.CellSize;
        //显示按钮
        btnMove.SetActive(true);
        btnAttack.SetActive(true);
        btnSkill.SetActive(true);
        //设置按钮是否被禁用
        Debug.Log("Movable" + UnitOnCell.Movable);
        btnMove.GetComponent<Button>().interactable = UnitOnCell.Movable;
        Debug.Log("Attackable" + UnitOnCell.Attackable);
        btnAttack.GetComponent<Button>().interactable = UnitOnCell.Attackable;
        //更新按钮位置
        btnMove.transform.position = this._bgImg.transform.position + new Vector3(0, cellSize / 1.3F, 0);
        btnAttack.transform.position = this._bgImg.transform.position + new Vector3(-cellSize / 1.5F, cellSize / 2F, 0);
        btnSkill.transform.position = this._bgImg.transform.position + new Vector3(cellSize / 1.5F, cellSize / 2F, 0);
        //将按钮设为在所有Object之上
        GameObject canvas = btnMove.transform.parent.gameObject;
        int childCount = canvas.transform.childCount;
        btnMove.transform.SetSiblingIndex(childCount);
        btnAttack.transform.SetSiblingIndex(childCount);
        btnSkill.transform.SetSiblingIndex(childCount);
    }

    public static void HideOperateButton()
    {
        btnMove.SetActive(false);
        btnAttack.SetActive(false);
        btnSkill.SetActive(false);
    }

    public void ShowSkillButton()
    {
        float cellSize = Chessboard.CellSize;
        //显示按钮
        btnSkill_1.SetActive(true);
        btnSkill_2.SetActive(true);
        btnSkill_3.SetActive(true);
        //设置按钮是否被禁用
        if (UnitOnCell.Skill_1 != null)
            btnSkill_1.GetComponent<Button>().interactable = UnitOnCell.Skill_1.IsUsable();
        if (UnitOnCell.Skill_2 != null)
            btnSkill_2.GetComponent<Button>().interactable = UnitOnCell.Skill_2.IsUsable();
        if (UnitOnCell.Skill_3 != null)
            btnSkill_3.GetComponent<Button>().interactable = UnitOnCell.Skill_3.IsUsable();
        //更新按钮位置
        btnSkill_1.transform.position = Background.transform.position + new Vector3(-cellSize / 1.5F, cellSize / 2F, 0);
        btnSkill_2.transform.position = Background.transform.position + new Vector3(0, cellSize / 1.3F, 0);
        btnSkill_3.transform.position = Background.transform.position + new Vector3(cellSize / 1.5F, cellSize / 2F, 0);
    }

    public static void HideSkillButton()
    {
        btnSkill_1.SetActive(false);
        btnSkill_2.SetActive(false);
        btnSkill_3.SetActive(false);
    }

    public void SetBackgroundColor(Color newColor)
    {
        this._bgImg.color = newColor;
    }

    public void activeRangeImg(bool active)
    {
        this._rangeImg.gameObject.SetActive(active);
    }

    public void setRangeColor(Color newColor)
    {
        this._rangeImg.color = newColor;
    }

    public void ShowMovableRange()
    {
        var attribute = UnitOnCell.UnitAttribute;
        for (int x = Math.Max(0, Position.x - attribute.motility);
            x <= Math.Min(Chessboard.ChessboardMaxX - 1, Position.x + attribute.motility);
            ++x)
            for (int y = Math.Max(0, Position.y - attribute.motility);
                y <= Math.Min(Chessboard.ChessboardMaxY - 1, Position.y + attribute.motility);
                ++y)
            {
                var itPosition = new ChessboardPosition(x, y);
                int distance = Position.Distance(itPosition);
                if (distance <= attribute.motility)
                    Chessboard.GetCell(itPosition).SetBackgroundColor(MovableColor);
            }
    }

    public Vector3 GetLocalPosition()
    {
        return this.transform.localPosition;
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }

    void Awake()
    {
        if (btnMove == null)
        {
            btnMove = GameObject.Find("Canvas/ButtonMove");
            btnAttack = GameObject.Find("Canvas/ButtonAttack");
            btnSkill = GameObject.Find("Canvas/ButtonSkill");

            btnSkill_1 = GameObject.Find("Canvas/OperateButton/ButtonSkill_1");
            btnSkill_2 = GameObject.Find("Canvas/OperateButton/ButtonSkill_2");
            btnSkill_3 = GameObject.Find("Canvas/OperateButton/ButtonSkill_3");
        }
    }

    // Use this for initialization
    void Start()
    {
        this._bgImg = this.transform.FindChild("Background").GetComponent<Image>();
        this._rangeImg = this.transform.FindChild("Range").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowAttackableRange()
    {
        var attribute = UnitOnCell.UnitAttribute;
        for (int x = Math.Max(0, Position.x - attribute.maxAtkRange);
            x <= Math.Min(Chessboard.ChessboardMaxX, Position.x + attribute.maxAtkRange);
            ++x)
            for (int y = Math.Max(0, Position.y - attribute.maxAtkRange);
                y <= Math.Min(Chessboard.ChessboardMaxY, Position.y + attribute.maxAtkRange);
                ++y)
            {
                var itPosition = new ChessboardPosition(x, y);
                int distance = Position.Distance(itPosition);
                if (attribute.minAtkRange <= distance &&
                    distance <= attribute.maxAtkRange)
                    Chessboard.GetCell(itPosition).SetBackgroundColor(AttackableColor);
            }
    }

    public void SwapUnit(Cell targetCell)
    {
        Unit temp = targetCell.UnitOnCell;
        targetCell.UnitOnCell = UnitOnCell;
        UnitOnCell = temp;

        if (targetCell.UnitOnCell != null)
        {
            //更新单位所在的cell
            targetCell.UnitOnCell.curCell = targetCell;
            //更新单位的HP和Group位置
            targetCell.UnitOnCell.UpdateAttributePosition();
        }
        if (UnitOnCell != null)
        {
            UnitOnCell.curCell = this;
            this.UnitOnCell.UpdateAttributePosition();
        }
    }

    /// <summary>
    /// 返回召唤是否成功
    /// </summary>
    /// <param name="unitID"></param>
    /// <param name="group"></param>
    /// <returns></returns>


    /// <summary>
    /// 这个格子是否可以召唤。任何可以召唤的格子
    /// </summary>
    /// <returns></returns>
    public bool IsCanSummonPlace()
    {
        if (UnitOnCell != null)                     // 格子上已有单位
            return false;
        else
            return true;
    }
}