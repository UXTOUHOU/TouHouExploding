using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class DamageResult : IBattleResult
{
	public Unit attacker;

	public Unit victim;

	public BattleConsts.DamageReason damageReason;

	public int physicalDamage;

	public int spellDamage;

	public int hpRemoval;

	public void execute()
	{
		// 计算伤害
		// todo :以后改为工厂模式通用接口
		HurtVO vo = new HurtVO();
		vo.attacker = this.attacker;
		vo.victim = this.victim;
		vo.damageReason = this.damageReason;
		// 物理伤害
		PhysicsDamageVO phyVO = new PhysicsDamageVO();
		phyVO.damage = this.physicalDamage;
		phyVO.damageBaseOutgoing = attacker.physicalDamageBaseOutgoing + victim.physicalDamageBaseOutgoing;
		phyVO.damagePercentage = attacker.physicalDamagePercentage + victim.physicalDamagePercentage;
		phyVO.damageExtraOutgoing = attacker.physicalDamageExtraOutgoing + victim.physicalDamageExtraOutgoing;
		vo.phycicsDamage = BattleFieldsUtils.getFinalPhysicsDamage(phyVO);
		// 魔法伤害
		SpellDamageVO spellVO = new SpellDamageVO();
		spellVO.damage = this.spellDamage;
		spellVO.damageBaseOutgoing = attacker.spellDamageBaseOutgoing + victim.spellDamageBaseOutgoing;
		spellVO.damagePercentage = attacker.spellDamagePercentage + victim.spellDamagePercentage;
		spellVO.damageExtraOutgoing = attacker.spellDamageExtraOutgoing + victim.spellDamageExtraOutgoing;
		vo.spellDamage = BattleFieldsUtils.getFinalSpellDamage(spellVO);
		// 生命流失
		HpRemovalVO hpRemovalVO = new HpRemovalVO();
		hpRemovalVO.damage = this.hpRemoval;
		vo.hpRemoval = BattleFieldsUtils.getFinalHpRemoval(hpRemovalVO);
		// hurt
		this.victim.hurt(vo);
	}

	public int getType()
	{
		throw new NotImplementedException();
	}
}

