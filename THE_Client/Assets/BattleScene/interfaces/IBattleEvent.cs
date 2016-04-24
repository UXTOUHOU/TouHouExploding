using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface IBattleEvent
{

    /// <summary>
    /// 事件类型
    /// </summary>
    /// <returns></returns>
    int getEventCode();

    /// <summary>
    /// 获取该事件将触发的效果
    /// </summary>
    /// <returns></returns>
    List<ISkillEffect> getTriggerEffects();

    IBattleVO getEventVO();
}