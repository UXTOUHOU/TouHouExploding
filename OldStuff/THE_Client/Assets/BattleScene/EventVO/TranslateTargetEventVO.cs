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

    public override void setProperty(BattleConsts.Property propId, object value)
    {
        switch (propId)
        {
            case BattleConsts.Property.TranslateTarget:
                this.target = (Unit)value;
                break;
            case BattleConsts.Property.TranslateOriginalRow:
                this.originalRow = (int)value;
                break;
            case BattleConsts.Property.TranslateOriginalCol:
                this.originalCol = (int)value;
                break;
            case BattleConsts.Property.TranslateOffsetRow:
                this.offsetRow = (int)value;
                break;
            case BattleConsts.Property.TranslateOffsetCol:
                this.offsetCol = (int)value;
                break;
            case BattleConsts.Property.TranslateTargetRow:
                this.targetRow = (int)value;
                break;
            case BattleConsts.Property.TranslateTargetCol:
                this.targetCol = (int)value;
                break;
        }
    }

    public override object getProperty(BattleConsts.Property propId)
    {
        switch (propId)
        {
            case BattleConsts.Property.TranslateTarget:
                return this.target;
            case BattleConsts.Property.TranslateOriginalRow:
                return this.originalRow;
            case BattleConsts.Property.TranslateOriginalCol:
                return this.originalCol;
            case BattleConsts.Property.TranslateOffsetRow:
                return this.offsetRow;
            case BattleConsts.Property.TranslateOffsetCol:
                return this.offsetCol;
            case BattleConsts.Property.TranslateTargetRow:
                return this.targetRow;
            case BattleConsts.Property.TranslateTargetCol:
                return this.targetCol;
            default:
                throw new NotImplementedException();
        }
    }

    public override BattleConsts.Code getEventCode()
    {
        return BattleConsts.Code.Translate;
    }
}

