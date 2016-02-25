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
        get { return Mathf.FloorToInt(motilityTotalPercent*(this.motilityBase + this.motilityExtra)); }
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
    }

    public CardAttribute(int cardID)
    {
        // 从文件中读取
        throw new System.NotImplementedException();
    }
}