using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TranslateTargetVO : IBattleVO
{
    public Unit target;
    public int offsetRow;
    public int offsetCol;

    public IBattleVO clone()
    {
        throw new NotImplementedException();
    }

    public object getProperty(BattleConsts.Property propId)
    {
        switch ( propId )
        {
            case BattleConsts.Property.TranslateTarget:
                return this.target;
            case BattleConsts.Property.TranslateOffsetRow:
                return this.offsetRow;
            case BattleConsts.Property.TranslateOffsetCol:
                return this.offsetCol;
        }
        return null;
    }

    public void setProperty(BattleConsts.Property propId, object value)
    {
        throw new NotImplementedException();
    }
}
