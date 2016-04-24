using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface IBattleVO
{
    void setProperty(int propId, object value);
    object getProperty(int propId);
    IBattleVO clone();
}
