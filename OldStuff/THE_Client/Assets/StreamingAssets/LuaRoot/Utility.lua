Utility = {};
EffectLib = require "EffectLib.cs";
PropertyLib = require "PropertyLib.cs";
BattleFieldLib = require "BattleFieldLib.cs";

function Utility.normalize(num)
	if num > 0 then 
		return 1;
	end
	if num < 0 then 
		return -1;
	end
	return 0;
	--return num > 0 ? 1 : num < 0 ? -1 : 0;
end