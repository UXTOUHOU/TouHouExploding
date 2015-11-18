using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public abstract class SummonCard : Card
    {
        public override string name
        {
            get
            {
                return unitAttribute.name;
            }
            set
            {
                unitAttribute.name = value;
            }
        }
        public override string typeID
        {
            get
            {
                return unitAttribute.typeID;
            }

            set
            {
                unitAttribute.typeID = value;
            }
        }

        public override string description
        {
            get
            {
                return base.description;
            }

            set
            {
                base.description = value;
            }
        }
        public Type type { get; set; }
        public UnitBase.Attribute unitAttribute { get; set; }//召唤出的角色属性
        public enum Type { Minion, Girl, Servant }//Common召唤出的为普通单位，Hero为少女单位，Servent为基本效果产生的单位
        //public enum Preset { 毛玉, 天狗, 妖精, 永远亭的兔子, 自爆人形, 河童重工, 博丽灵梦, 魂魄妖梦, 十六夜咲夜, Custom }
        public UnitBase ToBattle(int[] locate, Player owner)//上战场，把卡片转换为单位。已经召唤过的话会返回空
        {
            if (owner.action.HaveCall == true) return null;
            if (!GameCore.Chessboard.RegionList[locate[0], locate[1]].owner.Equals(owner.atTeam)) return null;//判断是否有权限
            if (GameCore.Chessboard.GetRegion(locate).unitHere != null) return null;
            owner.action.HaveCall = true;
            UnitBase x = new UnitBase(this, locate, owner);
            return x;
        }
        public SummonCard(Game core)
            : base(core)
        {
            GameCore = core;
            unitAttribute = new UnitBase.Attribute();
            SetAttribute();
        }
        public override void Discard()
        {
            base.Discard();
            if (GameCore.Characters.Contains(this))//判断是否处于卡堆
                GameCore.Characters.Remove(this);
            if (GameCore.WaitingCharacters.Contains(this))//判断是否处于召唤区
                GameCore.WaitingCharacters.Remove(this);
        }
        protected abstract void SetAttribute();//继承类必须实现这个方法，在其中写该角色的数值
        public override CardType GetCardType()
        {
            return CardType.SummonCard;
        }
    }
}
