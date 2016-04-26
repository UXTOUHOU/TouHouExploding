using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface IBattleVO
{
    void setProperty(BattleConsts.Property propId, object value);
    object getProperty(BattleConsts.Property propId);
    IBattleVO clone();
}
