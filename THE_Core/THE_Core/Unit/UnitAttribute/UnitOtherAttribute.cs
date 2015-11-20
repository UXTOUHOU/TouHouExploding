using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public class UnitOtherAttribute : UnitAttribute
    {
        public UnitOtherAttribute(string Key, int Value) : base(Key, Value, UnitAttributeType.Other)
        {

        }

        public UnitOtherAttribute(string Key, string Name, int Value) : base(Key, Name, Value, UnitAttributeType.Other)
        {

        }
    }
}
