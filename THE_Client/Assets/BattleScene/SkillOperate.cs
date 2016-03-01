using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System;

public class SkillOperate
{
    public static bool WaitSelectCell = false;
    public static Cell CellSkillTarget;
    public static Mutex CellSkillTargetMutex = new Mutex();

    /// <summary>
    /// 等待玩家选择一个单位
    /// </summary>
    /// <returns></returns>
    public static Unit SelectUnit()
    {
        Unit unit = null;
        WaitSelectCell = true;
        while (unit == null)
        {
            CellSkillTargetMutex.WaitOne();
            if (CellSkillTarget != null)
                unit = CellSkillTarget.UnitOnCell;
            CellSkillTargetMutex.ReleaseMutex();
            Thread.Sleep(1);
        }
        WaitSelectCell = false;
        return unit;
    }

    /// <summary>
    /// 选择直线方向的单位
    /// </summary>
    /// <param name="central"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public static Unit SelectBeelineUnit(Cell central, int distance)
    {
        Chessboard.SetBackgroundColor(delegate (Cell cell) { return central.Position.Distance(cell.Position) < 4; },
            Cell.MovableColor);
        Unit unit = null;
        do
        {
            unit = SelectUnit();
        } while (!(unit.curCell.Position.Distance(central.Position) <= distance));
        Chessboard.ClearBackground();
        return unit;
    }

    public static void NormalHurt(Unit target, int damage)
    {
        Debug.Assert(damage > 0, "damage <= 0");
        target.NormalHurt(damage);
    }

    public static bool ChangeDialogAttribute = false;
    private static string dialogMessage;
    private static bool dialogVisible = false;
    public static void MainThreadChessboardDialog()
    {
        MutexDialog.WaitOne();
        Chessboard.SetDialogString(dialogMessage);
        Chessboard.SetChessboardDialogVisible(dialogVisible);
        ChangeDialogAttribute = false;
        MutexDialog.ReleaseMutex();
    }

    public static Mutex MutexDialog = new Mutex();
    public static bool ClickDialogButton = false;
    public static bool DialogReturn = false;
    public static bool ChessboardDialog(string message)
    {
        ChangeDialogAttribute = true;
        dialogMessage = message;
        dialogVisible = true;
        ClickDialogButton = false;
        bool res = false;
        while (true)
        {
            MutexDialog.WaitOne();
            if (ClickDialogButton)
            {
                res = DialogReturn;
                MutexDialog.ReleaseMutex();
                break;
            }
            MutexDialog.ReleaseMutex();
            Thread.Sleep(1);
        }
        return res;
    }

    /// <summary>
    /// 技能发动时间
    /// </summary>
    public enum EUseSkillTime
    {
        AfterUnitMove,      // 单位移动后
        AfterUnitSummon,    // 召唤单位后
    }

    private static List<Skill> afterUnitMove = new List<Skill>();
    private static List<Skill> afterUnitSummon = new List<Skill>();

    /// <summary>
    /// 添加游戏进行到某个阶段需要发动的技能
    /// </summary>
    /// <param name="skill">需要发动的技能</param>
    /// <param name="time">发动的时间</param>
    public static void AddSkillCallBackTime(Skill skill, EUseSkillTime time)
    {
        switch (time)
        {
            case EUseSkillTime.AfterUnitMove:
                afterUnitMove.Add(skill);
                break;
            case EUseSkillTime.AfterUnitSummon:
                afterUnitSummon.Add(skill);
                break;
            default:
                throw new System.NotImplementedException();
        }
    }

    /// <summary>
    /// 对所有棋盘上的单位发动技能
    /// </summary>
    /// <param name="skill"></param>
    public static void ForEachUnitOnChessboard(Skill skill)
    {
        foreach (var unit in UnitManager.UnitList)
            skill.SkillEffect(unit.curCell);
    }

    /// <summary>
    /// 召唤单位，在任何可放置单位的位置
    /// </summary>
    /// <param name="unitID"></param>
    /// <param name="group"></param>
    /// <returns></returns>
    public static void SummonUnit(Cell cell, int unitID, EGroupType group)
    {
        if (!cell.IsCanSummonPlace()) return;       // 无法召唤的位置

        Unit newUnit = new Unit(unitID, cell.Position);
        cell.UnitOnCell = newUnit;
        cell.UnitOnCell.GroupType = group;
        cell.UnitOnCell.curCell = cell;
        // AfterSummonUnit事件
        foreach (var skill in afterUnitSummon)
            skill.SkillEffect(cell);
    }
}