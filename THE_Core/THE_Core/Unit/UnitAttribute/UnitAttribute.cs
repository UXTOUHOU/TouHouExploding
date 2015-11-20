using System.Diagnostics;

namespace THE_Core
{
    [DebuggerDisplay("Name={Name}, Value={Current}/{Max}(Default)")]
    public abstract class UnitAttribute
    {
        public string Key
        {
            private set;
            get;
        }
        public string Name;
        public UnitAttributeType AttributeType
        {
            private set;
            get;
        }
        public int Current;
        public int Max;
        public int Default
        {
            private set;
            get;
        }

        public UnitAttribute(string Key, int Value, UnitAttributeType Type = UnitAttributeType.Other)
        {
            this.Key = Key;
            this.Name = Key;
            AttributeType = UnitAttributeType.Other;
            this.Current = Value;
            this.Max = Value;
            this.Default = Value;
        }

        public UnitAttribute(string Key, string Name, int Value, UnitAttributeType Type = UnitAttributeType.Other)
        {
            this.Key = Key;
            this.Name = Name;
            AttributeType = UnitAttributeType.Other;
            this.Current = Value;
            this.Max = Value;
            this.Default = Value;
        }

        public enum UnitAttributeType
        {
            HitPoint,
            AttackPower,
            Mobility,
            AttackRange,
            Other
        }

    }
}
