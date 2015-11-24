using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace THE_Core
{
    /// <summary>
    /// 记录该单位已经进行过的行动
    /// </summary>
    public class UnitActionState
    {
        public bool IsAction = false;
        public bool HaveAction = false;
        public bool HaveAttack = false;
        public bool HaveMove = false;
        public void Reset()
        {
            IsAction = false;
            HaveAction = false;
            HaveAttack = false;
            HaveMove = false;
        }

    }
}
