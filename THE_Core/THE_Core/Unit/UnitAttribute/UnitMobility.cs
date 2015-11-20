using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public class UnitMobility : UnitAttribute
    {
        public UnitMobility(int Value) : base("Mobility", Value, UnitAttributeType.Mobility)
        {

        }
    }
}
