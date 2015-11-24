using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public class UnitMobility : UnitAttribute
    {
        public UnitMoveMethod MoveMethod
        {
            get;
            private set;
        }
        public UnitMobility(int Value, UnitMoveMethod MoveMethod) : base("Mobility", Value, UnitAttributeType.Mobility)
        {
            this.MoveMethod = MoveMethod;
        }
        public void ChangeMoveMethod(UnitMoveMethod MoveMethod)
        {
            this.MoveMethod = MoveMethod;
        }
    }

    public enum UnitMoveMethod
    {
        Walk,
        Fly,
        Teleport
    }
}
