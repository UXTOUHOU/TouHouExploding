using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public enum EGroupType
{
    BlueSide,
    RedSide
}

public enum EUnitState
{
    Damage_Decrease_Point                   // 减少受到的伤害：点数
}

public class UnitState
{
    public Skill skill;                     // 产生该状态的技能
    public bool skillSuperposable;          // 技能是否可叠加	
    public EUnitState stateType;            // 状态类型
    public int value;                       // 效果程度的数值

    public UnitState(Skill _skill, EUnitState _state, int _value, bool _superposable = false)
    {
        skill = _skill;
        stateType = _state;
        value = _value;
        skillSuperposable = _superposable;
    }
}

public class Unit
{
    private Cell _curCell;                // 单位所在的cell
    public Cell curCell
    {
        set
        {
            this._curCell = value;
            if ( value != null )
            {
                this._row = value.location.y;
                this._col = value.location.x;
            }
        }
        get { return this._curCell; }
    }
    public CardAttribute UnitAttribute;     // 单位当前属性


    public bool Movable = true;             // 是否可以进行移动操作
    public bool Attackable = true;          // 是否可以进行攻击操作

    public Skill Skill_1;
    public Skill Skill_2;
    public Skill Skill_3;

    private List<UnitState> listState = new List<UnitState>();      // 单位当前的状态列表
    private EGroupType groupType;           // 单位阵营
    private UnitUI unitSprite;              // 单位的UI
    private GameObject _unitGo;
    private Image _unitSp;
    private Image _groupSp;
    private Text _hpText;
    private int _row;
    public int row
    {
        get {  return this._row; }
    }
    private int _col;
    public int col
    {
        get { return this._col; }
    }

    public int HP
    {
        get
        {
            return UnitAttribute.hp;
        }

        set
        {
            UnitAttribute.hp = value;
            if (value <= 0)
                UnitDeath();
        }
    }

    public EGroupType GroupType
    {
        get
        {
            return groupType;
        }

        set
        {
            if (groupType != value)
                UnitManager.GroupChanged = true;
            groupType = value;
        }
    }

    //以下是每帧执行相关
    /// <summary>
    /// 当前是否正在移动
    /// </summary>
    private bool _isMoving;
    /// <summary>
    /// 当前是否正在等待移动至下一格的指令
    /// </summary>
    private bool _isWaitingToMove;
    private int _movingPathIndex;
    private int[] _movingPath;
    private float _movingTime;
    private float _movingTotalTime;
    private Vector3 _movingSpeed;
    private float _movingSpeedX;
    private float _movingSpeedY;

    public Unit(int unitID, ChessboardPosition targetPosition)
    {
        this._curCell = Chessboard.GetCell(targetPosition);

        Skill_1 = null;
        Skill_2 = null;
        Skill_3 = null;

        switch (unitID)
        {
            case 1:
                Skill_1 = new Skill_1_1(this);
                Skill_2 = new Skill_1_2(this);
                break;
        }

        // Test
        UnitAttribute = new CardAttribute();
        //
        this.createUnitPrefab();
        BattleSceneMain.getInstance().chessboard.addChildOnLayer(this._unitGo, BattleConsts.BattleFieldLayer_Unit, targetPosition.y, targetPosition.x);
        //unitSprite = new UnitUI(this, targetPosition);
        UnitManager.getInatance().registerUnit(this);
    }

    private void createUnitPrefab()
    {
        this._unitGo = ResourceManager.getInstance().loadPrefab("Prefabs/UnitPrefab");
        this._unitSp = this._unitGo.transform.FindChild("UnitImage").GetComponent<Image>();
        this._groupSp = this._unitGo.transform.FindChild("GroupImage").GetComponent<Image>();
        this._hpText = this._unitGo.transform.FindChild("HpText").GetComponent<Text>();
        // 设置对应的缩放
        this._groupSp.GetComponent<RectTransform>().localScale = new Vector3(1f / BattleGlobal.Scale, 1f / BattleGlobal.Scale, 1);
        this._hpText.GetComponent<RectTransform>().localScale = new Vector3(1f / BattleGlobal.Scale, 1f / BattleGlobal.Scale, 1);
    }

    private int[] _moveRange;
    /// <summary>
    /// 广搜，确定可移动到达的位置
    /// </summary>
    public int[] getAvailableMoveRange()
    {
        int len = BattleConsts.MapMaxRow * BattleConsts.MapMaxCol;
        int rowLimit = BattleConsts.MapMaxRow;
        int colLimit = BattleConsts.MapMaxCol;
        this._moveRange = new int[len];
        int i, j, index,tmpRow,tmpCol,stackLen,tmpPos;
        int[] stack = new int[len];
        for (i=0;i<len;i++)
        {
            this._moveRange[i] = -1;
        }
        index = 0;
        stackLen = 0;
        stack[stackLen++] = this._row * colLimit + this._col;
        // 当前位置花费步数为0
        this._moveRange[stack[0]] = 0;
        Cell cell;
        int[,] offsets = new int[4,2] { {0,-1 }, {0,1}, {-1,0 }, {1,0 } };
        Chessboard battleField = BattleSceneMain.getInstance().chessboard;
        while ( index < stackLen )
        {
            if (this._moveRange[stack[index]] >= this.UnitAttribute.motilityCurrent )
            {
                break;
            }
            for (i=0;i<4;i++)
            {
                tmpRow = stack[index] / colLimit + offsets[i,0];
                tmpCol = stack[index] % colLimit + offsets[i,1];
                tmpPos = tmpRow * colLimit + tmpCol;
                cell = battleField.getCellByPos(tmpRow,tmpCol);
                // 判断cell是否为可移动的位置,地形判断随后添加
                if ( cell != null && this._moveRange[tmpPos] == -1 && cell.UnitOnCell == null )
                {
                    this._moveRange[tmpPos] = this._moveRange[stack[index]] + 1;
                    stack[stackLen++] = tmpPos;
                }
            }
            index++;
        }
        return (int[])(this._moveRange.Clone());
    }



    /// <summary>
    /// 给单位添加一个状态
    /// </summary>
    /// <param name="state"></param>
    public void AddState(UnitState state)
    {
        if (state.skillSuperposable == false)
        {
            // 不可叠加的状态，遇到同名技能直接退出
            foreach (var unitState in listState)
                if (state.skill.ToString().Equals(unitState.skill.ToString()))
                    return;
        }
        listState.Add(state);
    }

    /// <summary>
    /// 去掉某个技能造成的所有状态
    /// </summary>
    /// <param name="skill"></param>
    public void RemoveSkillState(Skill skill)
    {
        if (skill == null) return;
        foreach (var state in listState)
            if (ReferenceEquals(state.skill, skill))
                listState.Remove(state);
    }

    /// <summary>
    /// 单位死亡时调用
    /// </summary>
    public void UnitDeath()
    {
        if (this._curCell != null)
            this._curCell.UnitOnCell = null;

        unitSprite.RemoveUnitSprite();
        // 删除技能产生的状态（没有删除技能产生的其他效果）
        foreach (Unit unit in UnitManager.UnitList)
        {
            unit.RemoveSkillState(Skill_1);
            unit.RemoveSkillState(Skill_2);
            unit.RemoveSkillState(Skill_3);
        }
    }

    public void NormalHurt(int damage)
    {
        foreach (var state in listState)
        {
            switch (state.stateType)
            {
                case EUnitState.Damage_Decrease_Point:
                    damage = Math.Max(damage - state.value, 0);     // 造成的伤害至少为0
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        HP -= damage;
        UnitManager.HPChanged = true;
    }

    public void update()
    {
        if ( this._isMoving )
        {
            this.move();
        }
    }

    public void startMoving(int[] movingPath)
    {
        if ( this._isMoving )
        {
            return;
        }
        this._movingPath = movingPath;
        this._movingPathIndex = 0;
        // 判断是否原地不动
        if ( this._movingPath.Length == 1 )
        {
            CommandManager.getInstance().runCommand(BattleConsts.CMD_UnitMoveComplete);
        }
        else
        {
            this._isMoving = true;
            this._isWaitingToMove = true;
            this.moveToNextCell();
        }
    }
    /// <summary>
    /// 该方法只在移动中可以进行调用，移动至下一格
    /// </summary>
    public void moveToNextCell()
    {
        if ( !this._isWaitingToMove || !this._isMoving )
        {
            return;
        }
        this._isWaitingToMove = false;
        Cell curCell = BattleGlobal.Core.chessboard.getCellByPos(this._movingPath[this._movingPathIndex]);
        Cell targetCell = BattleGlobal.Core.chessboard.getCellByPos(this._movingPath[this._movingPathIndex+1]);
        // 初始化时间
        this._movingTime = 0;
        this._movingTotalTime = BattleConsts.DefaultMoveTimePerCell;
        // 计算速度
        this._movingSpeedX = (BattleUtils.getCellPosXByCol(targetCell.location.x) - BattleUtils.getCellPosXByCol(curCell.location.x)) / this._movingTotalTime;
        this._movingSpeedY = (BattleUtils.getCellPosYByRow(targetCell.location.y) - BattleUtils.getCellPosYByRow(curCell.location.y)) / this._movingTotalTime;
        //Debug.Log("SpeedX = " + this._movingSpeedX + "  SpeedY = " + this._movingSpeedY);
        this._movingSpeed = new Vector3(this._movingSpeedX, this._movingSpeedY, 0);
        this._isMoving = true;
    }

    private void move()
    {
        this._movingTime += Time.deltaTime;
        if ( this._movingTime >= this._movingTotalTime )
        {
            Cell targetCell = BattleGlobal.Core.chessboard.getCellByPos(this._movingPath[this._movingPathIndex + 1]);
            this._unitGo.transform.position = targetCell.transform.position;
            // 判断是否已经到达终点
            this._movingPathIndex++;
            if ( this._movingPathIndex == this._movingPath.Length - 1 )
            {
                this._isMoving = false;
                CommandManager.getInstance().runCommand(BattleConsts.CMD_UnitMoveComplete);
            }
            else
            {
                // 暂停，等待下一次执行
                this._isWaitingToMove = true;
                CommandManager.getInstance().runCommand(BattleConsts.CMD_UnitStepOnCell);
                //this.moveToNext();
            }
        }
        else
        {
            Vector3 dTranslation = this._movingSpeed * Time.deltaTime;
            this._unitGo.transform.localPosition = this._unitGo.transform.localPosition + dTranslation;
        }
    }

    /// <summary>
    /// 单位所属的格子变更时，更新阵营和HP标志的位置
    /// </summary>
    public void UpdateAttributePosition()
    {
        unitSprite.UpdateHPPosition();
        unitSprite.UpdateGroupPosition();
    }

    public void MoveWithPath(List<ChessboardPosition> ListMovePath)
    {
        if (BattleProcess.CurrentState != PlayerState.WaitMoveAnimateEnd)
            BattleProcess.ChangeState(PlayerState.WaitMoveAnimateEnd);
        var lastPosition = ListMovePath[0];
        Vector3 targetPosition = lastPosition.GetPosition();
        //移动
        unitSprite.UnitImage.transform.position = Vector3.MoveTowards(unitSprite.UnitImage.transform.position,
            targetPosition,
            Chessboard.CellSize / 50F);
        if (unitSprite.UnitImage.transform.position == targetPosition)
        {//一段移动结束
            ListMovePath.RemoveAt(0);
            if (ListMovePath.Count == 0)
            {//移动结束
             //更新Unit的Cell
                var targetCell = Chessboard.GetCell(lastPosition);
                targetCell.SwapUnit(Chessboard.SelectedCell);
                Chessboard.SelectedCell = targetCell;

                targetCell.UnitOnCell.Movable = false;                      // 单位不可再次移动
                Chessboard.UnitMove = false;
                BattleProcess.ChangeState(PlayerState.SelectUnitBehavior);
            }
        }
    }

    public int Distance(Unit unit)
    {
        return this._curCell.Position.Distance(unit.curCell.Position);
    }
}