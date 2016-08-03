using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class BattleEventBase : IBattleEvent
{
    protected EventVOBase _eventVO;
    public BattleEventBase(EventVOBase vo)
    {
        this._eventVO = vo;
    }

    /// <summary>
    /// 事件类型（时点）
    /// </summary>
    /// <returns></returns>
    virtual public BattleConsts.Code getEventCode()
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// 事件名称
    /// </summary>
    /// <returns></returns>
    virtual public string getEventName()
    {
        return "";
    }

    public IBattleVO getEventVO()
    {
        return this._eventVO;
    }

    virtual public List<ISkillEffect> getTriggerEffects()
    {
        throw new NotImplementedException();
    }

    virtual public object getEventVOProperty(BattleConsts.Property propId)
    {
        return this._eventVO != null ? this._eventVO.getProperty(propId) : null;
    }
}
