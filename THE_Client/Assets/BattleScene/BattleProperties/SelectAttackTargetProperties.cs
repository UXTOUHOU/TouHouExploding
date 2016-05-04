using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class SelectAttackTargetProperties : IBattleProperties
{
	public Unit attacker;

	public Unit defender;

	public int minDisExtra;

	public int maxDisExtra;

	private BattleConsts.Code _code;

	public SelectAttackTargetProperties()
	{
		this._code = BattleConsts.Code.SelectAttackTarget;
		this.minDisExtra = 0;
		this.maxDisExtra = 0;
	}

	//public 
	public void addPropertyValue(BattleConsts.Property propId, object value)
	{
		switch (propId)
		{
		case BattleConsts.Property.MinAttackDisExtra:
			this.minDisExtra += (int)value;
			break;
		case BattleConsts.Property.MaxAttackDisExtra:
			this.maxDisExtra += (int)value;
			break;
		default:
			throw new NotImplementedException();
		}
	}

	public BattleConsts.Code getCode()
	{
		return this._code;
	}

	public object getProperty(BattleConsts.Property propId)
	{
		switch (propId)
		{
		case BattleConsts.Property.AttackAttacker:
			return this.attacker;
		case BattleConsts.Property.AttackDefender:
			return this.defender;
		case BattleConsts.Property.MinAttackDisExtra:
			return this.minDisExtra;
		case BattleConsts.Property.MaxAttackDisExtra:
			return this.maxDisExtra;
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

