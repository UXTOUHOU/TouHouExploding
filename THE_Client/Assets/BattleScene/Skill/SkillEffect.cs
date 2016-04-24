using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SkillEffect : ISkillEffect
{
    private int _ref;

    private int _code;

    private int _condition;

    private int _target;

    private int _cost;

    private int _operation;

    public SkillEffect()
    {
        this._ref = 0;
        this._code = 0;
        this._condition = 0;
        this._target = 0;
        this._cost = 0;
        this._operation = 0;
    }

    public IBattleProperties applyTo(IBattleProperties props)
    {
        throw new NotImplementedException();
    }

    public int getCode()
    {
        return this._code;
    }

    public int getCondition()
    {
        return this._condition;
    }

    public int getCost()
    {
        return this._cost;
    }

    public int getOperation()
    {
        return this._operation;
    }

    public int getRef()
    {
        return this._ref;
    }

    public int getTarget()
    {
        return this._target;
    }

    public void setCode(int value)
    {
        this._code = value;
    }

    public void setCondition(int value)
    {
        this._condition = value;
    }

    public void setCost(int value)
    {
        this._cost = value;
    }

    public void setOperation(int value)
    {
        this._operation = value;
    }

    public void setRef(int value)
    {
        this._ref = value;
    }

    public void setTarget(int value)
    {
        this._target = value;
    }
}

