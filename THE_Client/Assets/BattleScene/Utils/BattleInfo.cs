using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class BattleInfo
{
    public Unit unitSelected;

    public Unit attacker;

    public Unit defender;
    /// <summary>
    /// 时点处理是否完成
    /// </summary>
    public bool isProcessingComplete;
    /// <summary>
    /// 时点处理完之后的下个状态
    /// </summary>
    public BattleConsts.BattleState nextState;
    /// <summary>
    /// 当前状态下是否可以执行召唤单位的操作
    /// </summary>
    public bool isSummoningOpAvailabel;
    /// <summary>
    /// 将要召唤的单位id
    /// </summary>
    public string summoningUnitId;

    public void reset()
    {
        this.unitSelected = null;
        this.attacker = null;
        this.defender = null;
        this.nextState = 0;
        this.isSummoningOpAvailabel = false;
    }
}

