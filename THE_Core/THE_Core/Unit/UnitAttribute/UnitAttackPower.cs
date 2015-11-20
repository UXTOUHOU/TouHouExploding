using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public class UnitAttackPower: UnitAttribute
    {
        public UnitAttackPower(int Value) : base("AttackPower", "Attack Power", Value, UnitAttributeType.AttackPower)
        {
        }
    }
}
