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
        /// <summary>
        /// 召唤出的角色属性
        /// </summary>
        public Unit.Attribute unitAttribute { get; set; }
        /// <summary>
        /// 上战场，把卡片转换为单位。已经召唤过的话会返回空
        /// </summary>
        /// <param name="locate"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public Unit ToBattle(int[] locate, Player owner)
        {
            if (owner.action.HaveCall == true) return null;
            if (!GameCore.Chessboard.CellList[locate[0], locate[1]].owner.Equals(owner.atTeam)) return null;//判断是否有权限
            if (GameCore.Chessboard.GetRegion(locate).unitHere != null) return null;
            owner.action.HaveCall = true;
            Unit x = new Unit(this, locate, owner);
            return x;
        }
        public SummonCard(Game core)
            : base(core)
        {
            GameCore = core;
            unitAttribute = new Unit.Attribute();
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
        /// <summary>
        /// 继承类必须实现这个方法，在其中写该角色的数值
        /// </summary>
        protected abstract void SetAttribute();
        public override CardType GetCardType()
        {
            return CardType.SummonCard;
        }
    }
}
