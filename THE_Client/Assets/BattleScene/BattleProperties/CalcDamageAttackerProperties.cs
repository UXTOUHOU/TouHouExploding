using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CalcDamageAttackerProperties : IBattleProperties
{
	public Unit attacker;

	public Unit victim;

	public BattleConsts.DamageReason damageReason;

	public int physicalDamage;

	public int physicalDamageBaseOutgoing;

	public int physicalDamagePercentage;

	public int physicalDamageExtraOutgoing;

	public int spellDamage;

	public int spellDamageBaseOutgoing;

	public int spellDamagePercentage;

	public int spellDamageExtraOutgoing;

	public int totalDamageReduction;

	public int physicalDamageReduction;

	public int spellDamageReduction;

	public int hpRemoval;

	public void addPropertyValue(BattleConsts.Property propId, object value)
	{
		switch (propId)
		{
			case BattleConsts.Property.PhysicalDamage:
				this.physicalDamage += (int)value;
				break;
			case BattleConsts.Property.PhysicalDamageBaseOutgoing:
				this.physicalDamageBaseOutgoing += (int)value;
				break;
			case BattleConsts.Property.PhysicalDamagePercentage:
				this.physicalDamageExtraOutgoing += (int)value;
				break;
			case BattleConsts.Property.PhysicalDamageExtraOutgoing:
				this.physicalDamageExtraOutgoing += (int)value;
				break;
			case BattleConsts.Property.SpellDamage:
				this.spellDamage += (int)value;
				break;
			case BattleConsts.Property.SpellDamageBaseOutgoing:
				this.spellDamageBaseOutgoing += (int)value;
				break;
			case BattleConsts.Property.SpellDamagePercentage:
				this.spellDamagePercentage += (int)value;
				break;
			case BattleConsts.Property.SpellDamageExtraOutgoing:
				this.spellDamageExtraOutgoing += (int)value;
				break;
			case BattleConsts.Property.TotalDamageReduction:
				this.totalDamageReduction += (int)value;
				break;
			case BattleConsts.Property.PhysicalDamageReduction:
				this.physicalDamageReduction += (int)value;
				break;
			case BattleConsts.Property.SpellDamageReduction:
				this.spellDamageReduction += (int)value;
				break;
			default:
				throw new NotImplementedException();
		}
	}

	public BattleConsts.Code getCode()
	{
		return BattleConsts.Code.CalcDamageAttacker;
	}

	public object getProperty(BattleConsts.Property propId)
	{
		switch (propId)
		{
			case BattleConsts.Property.DamageReason:
				return this.damageReason;
			case BattleConsts.Property.DamageAttacker:
				return this.attacker;
			case BattleConsts.Property.DamageVictim:
				return this.victim;
			case BattleConsts.Property.PhysicalDamage:
				return this.physicalDamage;
			case BattleConsts.Property.PhysicalDamageBaseOutgoing:
				return this.physicalDamageBaseOutgoing;
			case BattleConsts.Property.PhysicalDamagePercentage:
				return this.physicalDamageExtraOutgoing;
			case BattleConsts.Property.PhysicalDamageExtraOutgoing:
				return this.physicalDamageExtraOutgoing;
			case BattleConsts.Property.SpellDamage:
				return this.spellDamage;
			case BattleConsts.Property.SpellDamageBaseOutgoing:
				return this.spellDamageBaseOutgoing;
			case BattleConsts.Property.SpellDamagePercentage:
				return this.spellDamagePercentage;
			case BattleConsts.Property.SpellDamageExtraOutgoing:
				return this.spellDamageExtraOutgoing;
			case BattleConsts.Property.TotalDamageReduction:
				return this.totalDamageReduction;
			case BattleConsts.Property.PhysicalDamageReduction:
				return this.physicalDamageReduction;
			case BattleConsts.Property.SpellDamageReduction:
				return this.spellDamageReduction;
			case BattleConsts.Property.HpRemoval:
				return this.hpRemoval;
			default:
				throw new NotImplementedException();
		}
	}

	public void setProperty(BattleConsts.Property propId, object value)
	{
		throw new NotImplementedException();
	}

	public IBattleVO clone()
	{
		return null;
	}
}

