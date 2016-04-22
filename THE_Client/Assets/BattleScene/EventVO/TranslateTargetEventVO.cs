using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TranslateTargetEventVO : EventVOBase
{
    public Unit target;
    public int originalRow;
    public int originalCol;
    public int offsetRow;
    public int offsetCol;
    public int targetRow;
    public int targetCol;
    public int translateReason;
    public List<int> paths;

    public override void setProperty(int propId, object value)
    {
        switch (propId)
        {
            case BattleConsts.PROPERTY_TRANSLATE_TARGET:
                this.target = (Unit)value;
                break;
            case BattleConsts.PROPERTY_TRANSLATE_ORIGINAL_ROW:
                this.originalRow = (int)value;
                break;
            case BattleConsts.PROPERTY_TRANSLATE_ORIGINAL_COL:
                this.originalCol = (int)value;
                break;
            case BattleConsts.PROPERTY_TRANSLATE_OFFSET_ROW:
                this.offsetRow = (int)value;
                break;
            case BattleConsts.PROPERTY_TRANSLATE_OFFSET_COL:
                this.offsetCol = (int)value;
                break;
            case BattleConsts.PROPERTY_TRANSLATE_TARGET_ROW:
                this.targetRow = (int)value;
                break;
            case BattleConsts.PROPERTY_TRANSLATE_TARGET_COL:
                this.targetCol = (int)value;
                break;
        }
    }

    public override object getProperty(int propId)
    {
        switch (propId)
        {
            case BattleConsts.PROPERTY_TRANSLATE_TARGET:
                return this.target;
            case BattleConsts.PROPERTY_TRANSLATE_ORIGINAL_ROW:
                return this.originalRow;
            case BattleConsts.PROPERTY_TRANSLATE_ORIGINAL_COL:
                return this.originalCol;
            case BattleConsts.PROPERTY_TRANSLATE_OFFSET_ROW:
                return this.offsetRow;
            case BattleConsts.PROPERTY_TRANSLATE_OFFSET_COL:
                return this.offsetCol;
            case BattleConsts.PROPERTY_TRANSLATE_TARGET_ROW:
                return this.targetRow;
            case BattleConsts.PROPERTY_TRANSLATE_TARGET_COL:
                return this.targetCol;
            default:
                throw new NotImplementedException();
        }
    }

    public override int getEventCode()
    {
        return BattleConsts.CODE_TRANSLATE;
    }
}

