using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public class UnitHitPoint : UnitAttribute
    {
        public UnitHitPoint(int Value) : base("HitPoint", "Hit Point", Value, UnitAttributeType.HitPoint)
        {
        }
    }
}
