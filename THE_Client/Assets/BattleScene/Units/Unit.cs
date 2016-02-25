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
    public Cell CurrentCell;                // 单位所在的cell
    public CardAttribute UnitAttribute;     // 单位当前属性


    public bool Movable = true;             // 是否可以进行移动操作
    public bool Attackable = true;          // 是否可以进行攻击操作


    public Skill Skill_1;
    public Skill Skill_2;
    public Skill Skill_3;

    private List<UnitState> listState = new List<UnitState>();      // 单位当前的状态列表
    private EGroupType groupType;           // 单位阵营
    private UnitUI unitSprite;              // 单位的UI

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

    public Unit(int unitID, ChessboardPosition targetPosition)
    {
        CurrentCell = Chessboard.GetCell(targetPosition);

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
        unitSprite = new UnitUI(this, targetPosition);
        UnitManager.UnitList.Add(this);
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
        if (CurrentCell != null)
            CurrentCell.UnitOnCell = null;

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

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
        return this.CurrentCell.Position.Distance(unit.CurrentCell.Position);
    }
}