using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class CounterAttackResult : IBattleResult
{
    /// <summary>
    /// 被反击的单位
    /// </summary>
    public Unit attacker;
    /// <summary>
    /// 反击的单位
    /// </summary>
    public Unit defender;

    public void execute()
    {
        // 能否反击的判断加入到单位的hurt中
        DamageResult dmgRes = new DamageResult();
        dmgRes.attacker = this.defender;
        dmgRes.victim = this.attacker;
        dmgRes.damageReason = BattleConsts.DAMAGE_REASON_COUNTER_ATTACK;
        dmgRes.physicalDamage = this.defender.UnitAttribute.attack;
        ProcessManager.getInstance().addResult(dmgRes);
    }

    public int getType()
    {
        throw new NotImplementedException();
    }
}

