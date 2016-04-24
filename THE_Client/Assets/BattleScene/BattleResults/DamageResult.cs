using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class DamageResult : IBattleResult
{
    public Unit attacker;

    public Unit victim;

    public int damageReason;

    public int physicalDamage;

    public int spellDamage;

    public int hpRemoval;

    public void execute()
    {
        // 攻击方的buff加成属性
        CalcDamageAttackerProperties attackerProps = new CalcDamageAttackerProperties();
        attackerProps.attacker = this.attacker;
        attackerProps.victim = this.victim;
        attackerProps.damageReason = this.damageReason;
        attackerProps.physicalDamage = this.physicalDamage;
        attackerProps.spellDamage = this.spellDamage;
        attackerProps.hpRemoval = this.hpRemoval;
        this.attacker.applyEffects(attackerProps);
        // 受伤害方的buff加成属性
        CalcDamageVictimProperties victimProps = new CalcDamageVictimProperties();
        victimProps.attacker = this.attacker;
        victimProps.victim = this.victim;
        victimProps.damageReason = this.damageReason;
        victimProps.physicalDamage = this.physicalDamage;
        victimProps.spellDamage = this.spellDamage;
        victimProps.hpRemoval = this.hpRemoval;
        this.victim.applyEffects(attackerProps);
        // 计算伤害
        // todo :以后改为工厂模式通用接口
        HurtVO vo = new HurtVO();
        vo.attacker = this.attacker;
        vo.victim = this.victim;
        vo.damageReason = this.damageReason;
        // 物理伤害
        PhysicsDamageVO phyVO = new PhysicsDamageVO();
        phyVO.damage = this.physicalDamage;
        phyVO.damageBaseOutgoing = attackerProps.physicalDamageBaseOutgoing + victimProps.physicalDamageBaseOutgoing;
        phyVO.damagePercentage = attackerProps.physicalDamagePercentage + victimProps.physicalDamagePercentage;
        phyVO.damageExtraOutgoing = attackerProps.physicalDamageExtraOutgoing + victimProps.physicalDamageExtraOutgoing;
        vo.phycicsDamage = BattleFieldsUntils.getFinalPhysicsDamage(phyVO);
        // 魔法伤害
        SpellDamageVO spellVO = new SpellDamageVO();
        spellVO.damage = this.spellDamage;
        spellVO.damageBaseOutgoing = attackerProps.spellDamageBaseOutgoing + victimProps.spellDamageBaseOutgoing;
        spellVO.damagePercentage = attackerProps.spellDamagePercentage + victimProps.spellDamagePercentage;
        spellVO.damageExtraOutgoing = attackerProps.spellDamageExtraOutgoing + victimProps.spellDamageExtraOutgoing;
        vo.spellDamage = BattleFieldsUntils.getFinalSpellDamage(spellVO);
        // 生命流失
        HpRemovalVO hpRemovalVO = new HpRemovalVO();
        hpRemovalVO.damage = this.hpRemoval;
        vo.hpRemoval = BattleFieldsUntils.getFinalHpRemoval(hpRemovalVO);
        // hurt
        this.victim.hurt(vo);
    }

    public int getType()
    {
        throw new NotImplementedException();
    }
}

