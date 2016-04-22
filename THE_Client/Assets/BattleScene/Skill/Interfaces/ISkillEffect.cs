using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface ISkillEffect : ILuaUserData
{
    /// <summary>
    /// 效果触发时点
    /// </summary>
    /// <returns></returns>
    int getCode();
    /// <summary>
    /// 效果触发时点
    /// </summary>
    /// <param name="value"></param>
    void setCode(int value);

    int getCondition();
    void setCondition(int value);

    int getOperation();
    void setOperation(int value);

    int getTarget();

    void setTarget(int value);

    int getCost();

    void setCost(int value);

    IBattleProperties applyTo(IBattleProperties props);
}