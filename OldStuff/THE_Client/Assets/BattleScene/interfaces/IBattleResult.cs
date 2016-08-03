using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 产生的结果接口
/// </summary>
public interface IBattleResult
{
    int getType();
    void execute();
}