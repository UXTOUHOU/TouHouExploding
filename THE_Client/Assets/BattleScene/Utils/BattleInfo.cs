using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class BattleInfo
{
    public Unit unitSelected;

    public Unit attacker;

    public Unit defender;
    /// <summary>
    /// 时点处理是否完成
    /// </summary>
    public bool isProcessingComplete;

    public void reset()
    {
        this.unitSelected = null;
        this.attacker = null;
        this.defender = null;
    }
}

