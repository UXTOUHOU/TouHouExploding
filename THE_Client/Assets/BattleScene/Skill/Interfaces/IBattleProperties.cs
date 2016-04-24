using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IBattleProperties : IBattleVO
{
    int getCode();
    void addPropertyValue(int propId, object value);
}
