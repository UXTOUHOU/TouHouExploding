using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public class UnitAttackRange : UnitAttribute
    {
        public UnitAttackRange(int Value) : base("AttackRange", "Attack Range", Value, UnitAttributeType.AttackRange)
        {
        }
    }
}
