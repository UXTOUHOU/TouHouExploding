using UnityEngine;
using System.Collections;

public class CardAttribute
{
    public int ID;              // 卡片编号;
    public int hp;              // HP
    public int motility;        // 机动性
    public int attack;          // 攻击力
    public int minAtkRange;     // 攻击范围最小值
    public int maxAtkRange;     // 攻击范围最大值
    /// <summary>
    /// 基础机动
    /// </summary>
    public int motilityBase;
    /// <summary>
    /// 额外机动
    /// </summary>
    public int motilityExtra;
    /// <summary>
    /// 总机动的额外百分比
    /// </summary>
    public float motilityTotalPercent;
    public int motilityCurrent
    {
        get { return Mathf.FloorToInt((1+motilityTotalPercent)*(this.motilityBase + this.motilityExtra)); }
    }

    public int minAttackRangeBase;
    public int minAttackRangeExtra;
    public int minAttackRangeCurrent
    {
        get { return this.minAttackRangeBase + this.minAttackRangeExtra; }
    }
    public int maxAttackRangeBase;
    public int maxAttackRangeExtra;
    public int maxAttackRangeCurrent
    {
        get { return this.maxAttackRangeBase + this.maxAttackRangeExtra; }
    }

    /// <summary>
    /// 配置的最大生命值
    /// </summary>
    private int _defaultMaxHp;
    /// <summary>
    /// 配置的最大生命值
    /// </summary>
    public int DefaultMaxHp
    {
        get { return this._defaultMaxHp; }
    }

    public CardAttribute()
    {
        // Test
        ID = 1;
        hp = 10;
        attack = 3;
        motility = 3;
        minAtkRange = 0;
        maxAtkRange = 3;
        //
        this._defaultMaxHp = 10;
        this.motilityBase = 3;
        this.minAttackRangeBase = 0;
        this.maxAttackRangeBase = 3;
        this.minAttackRangeExtra = 0;
        this.maxAttackRangeExtra = 0;
    }

    public CardAttribute(int cardID)
    {
        // 从文件中读取
        throw new System.NotImplementedException();
    }
}