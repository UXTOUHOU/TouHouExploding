using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouHou_Exploding
{
    public class MissTest : Character
    {
        public MissTest(Core core)
            : base(core)
        {

        }
        protected override void SetAttribute()
        {
            
            unitAttribute = new Unit.Attribute()
            {
                name = "测试小妹",
                blood = 7,
                mobility = 3,
                range = 2,
                attack = 4,
            };
        }

    }
}
