using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EventVOBase : IBattleVO
{
    virtual public BattleConsts.Code getEventCode()
    {
        return 0;
    }

    virtual public IBattleVO clone()
    {
        throw new NotImplementedException();
    }

    virtual public object getProperty(BattleConsts.Property propId)
    {
        throw new NotImplementedException();
    }

    virtual public void setProperty(BattleConsts.Property propId, object value)
    {
        throw new NotImplementedException();
    }
}

