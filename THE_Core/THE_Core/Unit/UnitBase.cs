using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace THE_Core
{
    [DebuggerDisplay("Id = {Id}, Name= {Name}")]
    public class UnitBase
    {
        public string Id
        {
            get;
            private set;
        }
        public string Name
        {
            get;
            private set;
        }
        public string Description
        {
            get;
            private set;
        }
        public string Provenance
        {
            get;
            private set;
        }
        public UnitType Type
        {
            get;
            private set;
        }
        public int HitPoint
        {
            get;
            private set;
        }
        public int Mobility
        {
            get;
            private set;
        }
        public int AttackPower
        {
            get;
            private set;
        }
        public int AttackRange
        {
            get;
            private set;
        }

        public bool Avaliable
        {
            get;
            private set;
        }

        public List<Skill> SkillList;

        private UnitBase(string id, string name, string description, string provenance, UnitType type, int hitPoint, int mobility, int attackPower, int attackRange, bool avaliable)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Type = type;
            this.HitPoint = hitPoint;
            this.Mobility = mobility;
            this.AttackPower = attackPower;
            this.AttackRange = attackRange;
            this.Avaliable = avaliable;
        }

        public static UnitBase FromUnitRow(THE_Data.UnitRow unitRow)
        {
            UnitType type = (UnitType)Enum.Parse(typeof(UnitType), unitRow.Type);
            return new UnitBase(unitRow.Id, unitRow.Name, unitRow.Description, unitRow.Provenance, type, unitRow.HitPoint, unitRow.Mobility, unitRow.AttackPower, unitRow.AttackRange, unitRow.Avaliable);
        }
    }
    public enum UnitType { Minion, Girl, Servant }//Common召唤出的为普通单位，Hero为少女单位，Servent为基本效果产生的单位
}
