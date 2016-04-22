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

public class Unit : IID,IBattleFieldLocation,ILuaUserData
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
        set { this._row = value; }
    }
    private int _col;
    public int col
    {
        get { return this._col; }
        set { this._col = value; }
    }

    /// <summary>
    /// 当前最大生命值
    /// </summary>
    private int _maxHp;
    public int maxHp
    {
        get { return this._maxHp; }
    }
    /// <summary>
    /// 当前生命值
    /// </summary>
    private int _curHp;
    public int curHp
    {
        get { return this._curHp; }
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

    private int _id;
    public int id
    {
        get
        {
            return this._id;
        }
        set
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 拥有者
    /// </summary>
    private int _ownerId;
    public int ownerId
    {
        get { return this._ownerId; }
    }
    /// <summary>
    /// 控制者
    /// </summary>
    private int _controllerId;
    public int controllerId
    {
        get { return this._controllerId; }
    }

    private int _ref;

    private List<BuffDataDriven> _buffList;

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
    /// <summary>
    /// 更新视图标识
    /// </summary>
    private bool _updateViewFlag;

    /// <summary>
    /// 当前攻击次数
    /// </summary>
    private int _curAttackCount;
    /// <summary>
    /// 每回合最大可攻击次数
    /// </summary>
    private int _maxAttackCount;
    /// <summary>
    /// 每回合可额外攻击的次数
    /// </summary>
    private int _extraAttackCount;

    private int _curCounterAttackCount;
    private int _maxCounterAttackCount;
    private int _extraCounterAttackCount;

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
        this._curHp = UnitAttribute.hp;
        this._maxHp = UnitAttribute.hp;
        //
        this.createUnitPrefab();
        BattleGlobal.Core.chessboard.addChildOnLayer(this._unitGo, BattleConsts.BattleFieldLayer_Unit, targetPosition.y, targetPosition.x);
        //unitSprite = new UnitUI(this, targetPosition);
        this._id = IDProvider.getInstance().applyUnitId(this);
        UnitManager.getInatance().registerUnit(this);
        this._buffList = new List<BuffDataDriven>();
        InterpreterManager.getInstance().registerLightUserData(this);
        this.setUpdateViewFlag(true);
        // 生成技能
        // test
        if ( a == 0 )
        {
            InterpreterManager.getInstance().initSkill("skill1", this);
            a = 1;
        }
        this.initAttributes();
    }

    private void initAttributes()
    {
        this._curAttackCount = 0;
        this._curCounterAttackCount = 0;
        this._maxAttackCount = 1;
        this._maxCounterAttackCount = 1;
        this._extraAttackCount = 0;
        this._extraCounterAttackCount = 0;
    }

    static int a = 0;

    private void createUnitPrefab()
    {
        this._unitGo = ResourceManager.getInstance().createNewInstanceByPrefabName("UnitPrefab");
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
        int[,] offsets = new int[4,2] { {0,-1}, {0,1}, {-1,0}, {1,0} };
        Chessboard battleField = BattleGlobal.Core.chessboard;
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
    /// 应用效果
    /// </summary>
    /// <param name="props"></param>
    /// <returns></returns>
    public IBattleProperties applyEffects(IBattleProperties props)
    {
        int len = this._buffList.Count;
        BuffDataDriven buff;
        for (int i=0;i< len;i++)
        {
            buff = this._buffList[i];
            //if ( buff.canTrigger(props.getCode()) )
            //{
            //    buff.applyTo(props);
            //}
        }
        return props;
    }

    public void getBuffEffectsByCode(int code,List<ISkillEffect> effects)
    {
        int len = this._buffList.Count;
        BuffDataDriven buff;
        for (int i = 0; i < len; i++)
        {
            buff = this._buffList[i];
            buff.getEffectsByCode(code, effects);
        }
    }

    #region 生命值相关
    public void hurt(HurtVO vo)
    {
        int totalDamage = vo.phycicsDamage + vo.spellDamage + vo.hpRemoval;
        Debug.Log("unit in position(" + this._row + "," + this._col + ") curHp=" + this._curHp + " hurt,damage is " + totalDamage);
        if ( this._curHp <= totalDamage )
        {
            // 判断是否有死亡前触发的effect
            // 触发单位死亡事件
        }
        else
        {
            this._curHp -= totalDamage;
            // 更新UI
            this._updateViewFlag = true;
            // 触发受伤事件
            EventVOBase evtVO = BattleObjectFactory.createEventVO(BattleConsts.CODE_TAKE_DAMAGE);
            evtVO.setProperty(BattleConsts.PROPERTY_DAMAGE_ATTACKER, vo.attacker);
            evtVO.setProperty(BattleConsts.PROPERTY_DAMAGE_VICTIM, vo.victim);
            evtVO.setProperty(BattleConsts.PROPERTY_CALC_PHYSICAL_DAMAGE, vo.phycicsDamage);
            evtVO.setProperty(BattleConsts.PROPERTY_CALC_SPELL_DAMAGE, vo.spellDamage);
            evtVO.setProperty(BattleConsts.PROPERTY_CALC_HP_REMOVAL, vo.hpRemoval);
            evtVO.setProperty(BattleConsts.PROPERTY_DAMAGE_REASON, vo.damageReason);
            BattleEventBase evt = BattleObjectFactory.createBattleEvent(BattleConsts.CODE_TAKE_DAMAGE, evtVO);
            ProcessManager.getInstance().raiseEvent(evt);
        }
    }

    public void recover(int recoverValue)
    {

    }
    #endregion

    #region 控制权相关
    public void setOwner(int playerId)
    {
        this._ownerId = playerId;
    }

    public void setController(int playerId)
    {
        this._controllerId = playerId;
        this.setUpdateViewFlag(true);
    }

    public void changeController(int playerId)
    {
        if ( this._controllerId != playerId )
        {
            this._controllerId = playerId;
            this.setUpdateViewFlag(true);
            // 触发控制权变更事件

        }
    }
    #endregion

    #region 位移相关
    /// <summary>
    /// 改变单位位置
    /// </summary>
    /// <param name="offsetRow">行偏移量</param>
    /// <param name="offsetCol">列偏移量</param>
    /// <returns></returns>
    public int translate(int offsetRow,int offsetCol)
    {
        int targetRow = this._row + offsetRow;
        int targetCol = this._col + offsetCol;
        Cell targetCell = BattleGlobal.Core.chessboard.getCellByPos(targetRow, targetCol);
        if ( targetCell.UnitOnCell != null )
        {
            Debug.LogError("Unit translate fail!There is unit on targetCell!");
            return BattleConsts.UNIT_ACTION_FAIL;
        }
        this.curCell = targetCell;
        return BattleConsts.UNIT_ACTION_SUCCESS;
    }
    #endregion

    #region 攻击、反击相关
    /// <summary>
    /// 是否可以执行攻击指令
    /// </summary>
    /// <returns></returns>
    public bool canAttack()
    {
        if ( this._curAttackCount < this._maxAttackCount + this._extraAttackCount )
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 是否可以执行反击指令
    /// </summary>
    /// <returns></returns>
    public bool canCounterAttack()
    {
        if (this._curCounterAttackCount < this._maxCounterAttackCount + this._extraCounterAttackCount)
        {
            return true;
        }
        return false;
    }

    public int doAttack()
    {
        this._curAttackCount++;
        return BattleConsts.UNIT_ACTION_SUCCESS;
    }

    public int doCounterAttack()
    {
        this._curCounterAttackCount++;
        return BattleConsts.UNIT_ACTION_SUCCESS;
    }
    #endregion

    private void updateView()
    {
        this._hpText.text = this._curHp.ToString();
        // todo :暂用
        this._groupSp.sprite = Resources.Load<Sprite>(BattleSceneUtils.getPlayerGroupTextureName(this._controllerId));
    }

    public void addBuff(BuffDataDriven buff)
    {
        // todo : 暂时不加判断
        this._buffList.Add(buff);
    }

    public void reset(int resetType)
    {
        if ( resetType == BattleConsts.RESET_TYPE_SELF_TURN_END )
        {
            this._curAttackCount = 0;
            this._curCounterAttackCount = 0;
        }
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
        if ( this._updateViewFlag )
        {
            this._updateViewFlag = false;
            this.updateView();
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
        this._movingSpeedX = (BattleSceneUtils.getCellPosXByCol(targetCell.location.x) - BattleSceneUtils.getCellPosXByCol(curCell.location.x)) / this._movingTotalTime;
        this._movingSpeedY = (BattleSceneUtils.getCellPosYByRow(targetCell.location.y) - BattleSceneUtils.getCellPosYByRow(curCell.location.y)) / this._movingTotalTime;
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

    public void setUpdateViewFlag(bool value)
    {
        this._updateViewFlag = value;
    }

    //public void applyToBuff

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

    public void setRef(int value)
    {
        this._ref = value;
    }

    public int getRef()
    {
        return this._ref;
    }

    public delegate void OnUnitAttackAnnounceHandler(IBattleProperties props);
    /// <summary>
    /// 攻击宣言前触发
    /// </summary>
    public OnUnitAttackAnnounceHandler OnUnitAttackAnnounce;


}