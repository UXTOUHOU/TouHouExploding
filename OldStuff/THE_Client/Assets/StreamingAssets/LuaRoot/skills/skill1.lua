function skill1.cost()
	return 0;
end

function skill1.initSkill(unit)
	--local unit = PropertyLib.getSkillCaster(skill);
	--主动技能模板
	--local e1 = EffectLib.createEffect();
	--PropertyLib.setEffectCode(CODE_ACTIVE);
	--PropertyLib.setEffectCost(skill1.cost);
	--PropertyLib.setSkillActiveEffect(e1);
	--被动特效模板
	local buff = EffectLib.createBuff();
	local e2 = EffectLib.createEffect();
	PropertyLib.setEffectCode(e2,CODE_TAKE_DAMAGE);
	PropertyLib.setEffectCondition(e2,skill1.condition);
	PropertyLib.setEffectOperation(e2,skill1.operation);
	EffectLib.addEffectToBuff(e2,buff);
	EffectLib.addBuffToUnit(buff,unit);
end

function skill1.condition(vo)
	local reason = PropertyLib.getVOProperty(vo,PROPERTY_DAMAGE_REASON);
	if reason ~= DAMAGE_REASON_ATTACK then
		return false;
	end
	return true;
end

function skill1.target1(e)
	--local victim = lib.getProperty(e,PROPERTY_ATTACKER);
	--lib.setOperationInfo(victim);
end

function skill1.operation(vo)
	local attacker = PropertyLib.getVOProperty(vo,PROPERTY_DAMAGE_ATTACKER);
	local victim = PropertyLib.getVOProperty(vo,PROPERTY_DAMAGE_VICTIM);
	local row0,col0 = BattleFieldLib.getUnitLocation(attacker);
	local row1,col1 = BattleFieldLib.getUnitLocation(victim);
	local row2,col2;
	local dRow = Utility.normalize(row1-row0);
	local dCol = Utility.normalize(col1-col0);
	local crashUnit = false;
	for i=1,2 do
		row2 = row1 + dRow * i;
		col2 = col1 + dCol * i;
		if BattleFieldLib.hasUnitOnCell(row2,col2) then
			crashUnit = true;
			local unit = BattleFieldLib.getUnitOnCell(row2,col2);
			if  i ~= 1 then
				translateTable =
				{
					unit = victim,
					dRow = dRow,
					dCol = dCol,
				}
				EffectLib.applyTranslate(translateTable);
			end
			damageTable = 
			{
				attacker = attacker,
				victim = victim,
				hpRemoval = 5,
			}
			EffectLib.applyDamage(damageTable);
			damageTable = 
			{
				attacker = attacker,
				victim = unit,
				hpRemoval = 5,
			}
			EffectLib.applyDamage(damageTable);
			break;
		end
	end
	if crashUnit==false then
		translateTable =
		{
			target = victim,
			offsetRow = dRow * 2,
			offsetCol = dCol * 2,
		}
		EffectLib.applyTranslate(translateTable);
	end
end