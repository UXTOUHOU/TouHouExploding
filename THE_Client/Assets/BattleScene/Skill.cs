using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public abstract class Skill
{
    public Unit unit = null;            // 技能所属的单位
                                        //public List<Unit> affectedUnit;	// 受技能影响的单位
    protected bool usable = true;       // 技能是否可用
    protected bool cancelable = true;   // 是否可以取消



    /// <summary>
    /// 获取技能的可用状态
    /// </summary>
    /// <returns></returns>
    public virtual bool IsUsable()
    {
        return usable;
    }

    /// <summary>
    /// 获取技能是否可以取消
    /// </summary>
    /// <returns></returns>
    public virtual bool IsCancelable()
    {
        return cancelable;
    }

    /// <summary>
    /// 单位被召唤时，初始化技能时调用。主动技能可以为空，被动技能应再初始化时添加到事件响应的队列中。
    /// </summary>
    public virtual void InitSkill()
    {
    }

    /// <summary>
    /// 新回合时刷新技能的状态
    /// </summary>
    /// <param name="group"></param>
    public virtual void OnNewRound(EGroupType group)
    {
        if (unit.GroupType != group) return;
        usable = true;
    }

    /// <summary>
    /// 技能发动产生的效果，执行技能应调用RunSkillThread。这个函数包括目标的判断
    /// </summary>
    public abstract void SkillEffect(Cell cell);

    /// <summary>
    /// 执行技能时调用
    /// </summary>
    public void RunSkillThread()
    {
        BattleProcess.ChangeState(PlayerState.RunningSkill);
        new Thread(new ThreadStart(StartSkill)).Start();
    }

    /// <summary>
    /// 技能发动
    /// </summary>
    private void StartSkill()
    {
        SkillEffect(null);
        BattleProcess.ChangeState(PlayerState.SkillEnd);
    }

    /// <summary>
    /// 技能结束
    /// </summary>
    protected virtual void OnSkillEnd()
    {
        throw new NotImplementedException();
    }

    public Skill(Unit master)
    {
        unit = master;

        InitSkill();
    }
}

public abstract class ActiveSkill : Skill
{
    public ActiveSkill(Unit master) : base(master)
    {

    }

    public virtual void OnChangeCell(ChessboardPosition position)
    {
    }
}

public abstract class PassiveSkill : Skill
{


    public PassiveSkill(Unit master) : base(master)
    {
    }
}

public class Skill_1_1 : ActiveSkill
{
    public override void SkillEffect(Cell cell)
    {
        //SetSkillRange();
        //SetSkillRangeVisible();
        //SetTargetRange();
        //SetTargetRangeVisible();
        Unit target = SkillOperate.SelectUnit();
        SkillOperate.ChessboardDialog("2");
        target.NormalHurt(2);
        usable = false;
    }

    public Skill_1_1(Unit master) : base(master)
    {

    }
}

public class Skill_1_2 : PassiveSkill
{
    public override void InitSkill()
    {
        // 召唤时检查所有已存在的单位，包括自身
        SkillOperate.ForEachUnitOnChessboard(this);
        // 有单位位置变换时，更新所有单位的状态
        SkillOperate.AddSkillCallBackTime(this, SkillOperate.EUseSkillTime.AfterUnitMove);
        // 召唤新的单位时时，更新所有单位的状态
        SkillOperate.AddSkillCallBackTime(this, SkillOperate.EUseSkillTime.AfterUnitSummon);
    }

    public override void SkillEffect(Cell cell)
    {
        if (cell == null) return;
        if (cell.UnitOnCell == null) return;

        Unit targetUnit = cell.UnitOnCell;
        if (targetUnit.GroupType != this.unit.GroupType) return;

        if (this.unit.Distance(targetUnit) <= 2)                        // 范围2内的单位
        {
            UnitState state = new UnitState(this,
                EUnitState.Damage_Decrease_Point,
                1);                                                     // 所受伤害减少1
            targetUnit.AddState(state);
        }
        else
        {
            targetUnit.RemoveSkillState(this);
        }
    }

    public Skill_1_2(Unit master) : base(master)
    {

    }
}