using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IBattleProperties : IBattleVO
{
	BattleConsts.Code getCode();
    //void addPropertyValue(BattleConsts.Property propId, object value);
}
