using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class BattleFieldsUtils
{
    public static int getManhattanDis(IBattleFieldLocation a,IBattleFieldLocation b)
    {
        return Math.Abs(a.row - b.row) + Math.Abs(a.col - b.col);
    }

    public static int getFinalPhysicsDamage(PhysicsDamageVO vo)
    {
        int baseValue = vo.damage;
        int baseBonus = vo.damageBaseOutgoing;
        int basePercentage = vo.damagePercentage;
        int baseExtraValue = vo.damageExtraOutgoing;
        float finalValue = (float)baseValue + baseBonus;
        if ( finalValue <= 0 )
        {
            return 0;
        }
        else
        {
            if ( basePercentage <= -100 )
            {
                return 0;
            }
            else
            {
                finalValue = finalValue * (1 + 1.0f * basePercentage / 100f);
            }
            finalValue += baseExtraValue;
        }
        return (int)finalValue;
    }

    public static int getFinalSpellDamage(SpellDamageVO vo)
    {
        int baseValue = vo.damage;
        int baseBonus = vo.damageBaseOutgoing;
        int basePercentage = vo.damagePercentage;
        int baseExtraValue = vo.damageExtraOutgoing;
        float finalValue = (float)baseValue + baseBonus;
        if (finalValue <= 0)
        {
            return 0;
        }
        else
        {
            if (basePercentage <= -100)
            {
                return 0;
            }
            else
            {
                finalValue = finalValue * (1 + 1.0f * basePercentage / 100f);
            }
            finalValue += baseExtraValue;
        }
        return (int)finalValue;
    }

    public static int getFinalHpRemoval(HpRemovalVO vo)
    {
        return vo.damage;
    }
    
    /// <summary>
    /// 是否在攻击范围内
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defender"></param>
    /// <returns></returns>
    public static bool isInAttackRange(Unit attacker,Unit defender)
    {
        int manhattanDis = BattleFieldsUtils.getManhattanDis(attacker,defender);
        int minAttackDis = attacker.UnitAttribute.minAttackRangeCurrent;
        int maxAttackDis = attacker.UnitAttribute.maxAttackRangeCurrent;
        return minAttackDis <= manhattanDis && manhattanDis <= maxAttackDis;
    }
}

