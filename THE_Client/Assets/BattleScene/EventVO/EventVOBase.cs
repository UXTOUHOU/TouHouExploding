using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EventVOBase : IBattleVO
{
    virtual public int getEventCode()
    {
        return 0;
    }

    virtual public IBattleVO clone()
    {
        throw new NotImplementedException();
    }

    virtual public object getProperty(int propId)
    {
        throw new NotImplementedException();
    }

    virtual public void setProperty(int propId, object value)
    {
        throw new NotImplementedException();
    }
}

