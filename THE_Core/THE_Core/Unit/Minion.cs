using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public class Minion : UnitBase
    {
        public Minion(SummonCard transCard, int[] buildLocate, Player owner) : base(transCard, buildLocate, owner)
        {
        }
    }
}
