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

    public object getProperty(int propId)
    {
        switch ( propId )
        {
            case BattleConsts.PROPERTY_TRANSLATE_TARGET:
                return this.target;
            case BattleConsts.PROPERTY_TRANSLATE_OFFSET_ROW:
                return this.offsetRow;
            case BattleConsts.PROPERTY_TRANSLATE_OFFSET_COL:
                return this.offsetCol;
        }
        return null;
    }

    public void setProperty(int propId, object value)
    {
        throw new NotImplementedException();
    }
}
