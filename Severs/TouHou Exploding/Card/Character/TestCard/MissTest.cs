using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
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
                description = "说真的，我不认为有人有机会看到这段说明。",
                blood = 7,
                mobility = 3,
                range = 2,
                attack = 4,
            };
        }
        //public class 目标诺森德:Skill
        //{
        //    public 目标诺森德(Unit unit=null)
        //        :base(unit)
        //    {
                
        //    }
        //    public override bool CanUse(InputUse input)
        //    {
        //        if (base.CanUse() == false) return false;
        //        InputUse_目标诺森德 _input = (InputUse_目标诺森德)input;
        //        return master.GetDistance(_input.beAttacked) <= master.attribute.range;
        //    }
        //    public override bool Fuction(InputUse inputUse = null)
        //    {
        //        InputUse_目标诺森德 input = (InputUse_目标诺森德)inputUse;

                
        //    }

        //    public override void SetAttribute()
        //    {
        //        name = "目标，诺森德！";
        //        description = "（2B）造成3点伤害。";
        //        cost = 2;
        //    }
        //    public class InputUse_目标诺森德 : InputUse
        //    {
        //        public Unit beAttacked;
        //        public InputUse_目标诺森德(Unit _beAttacked)
        //        {
        //            beAttacked = _beAttacked;
        //        }
        //    }
        //}
     }
}

