using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace TouHou_Exploding
{
    public abstract class Card : IDProvider.IID
    {
        public Core GameCore;
        
        public int cost { get; set; }
        public int id { get; set; }
        public virtual string name { get; set; }
        public virtual string description { get; set; }//对该卡牌的描述
        public abstract CardType GetCardType();
        public enum CardType { PolicyCard, CommonCharacter, Hero } 
        public Card(Core core)
        {
            GameCore = core;
            GameCore.IDP.CID.ApplyID(this);
        }
        public virtual void Discard()//丢弃卡牌
        {
            GameCore.IDP.CID.Del(this);
        }
    }
    public abstract class PolicyCard : Card
    {
        public Player Owner;
        public PolicyCard(Player player)
            : base(player.GameCore)
        {
            GameCore = player.GameCore;
            Owner = player;
            player.policyCard.Add(this);
            GameCore.IDP.CID.ApplyID(this);
    }
        public override void Discard()
        {
            base.Discard();
            Owner.policyCard.Remove(this);
        }

        public override CardType GetCardType()
        {
            return CardType.PolicyCard;
        }
        public abstract bool Fuction(UseInput uInput = null); //策略牌使用时执行的操作

        public virtual bool Use(UseInput uInput = null) //任何一个策略牌的使用方法
        {
            if (Owner.bDot < cost)
            {
                return false;
            }
            Owner.policyCard.Remove(this);
            GameCore.IDP.CID.Del(this);
            if(Fuction(uInput))
            {
                Owner.bDot -= cost;
                return true;
            }
            else
            {
                return false;
            }
        } 
        public class UseInput//新输入继承此类
        {

        } 
    }
    public abstract class Character : Card
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
        public Unit.Attribute unitAttribute { get; set; }//召唤出的角色属性
        public enum Type{ Common,Hero,Servant }//Common召唤出的为普通单位，Hero为少女单位，Servent为基本效果产生的单位
        //public enum Preset { 毛玉, 天狗, 妖精, 永远亭的兔子, 自爆人形, 河童重工, 博丽灵梦, 魂魄妖梦, 十六夜咲夜, Custom }
        public Unit ToBattle(int[] locate, Player owner)//上战场，把卡片转换为单位。已经召唤过的话会返回空
        {
            if (owner.action.HaveCall == true) return null;
            if (!GameCore.RoomMap.RegionList[locate[0],locate[1]].owner.Equals(owner.atTeam)) return null;//判断是否有权限
            owner.action.HaveCall = true;
            Unit x =new Unit(this,locate,owner);
            return x;
        }
        public Character(Core core)
            :base(core)
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
        protected abstract void SetAttribute();//继承类必须实现这个方法，在其中写该角色的数值
        public override CardType GetCardType()
        {
            return CardType.CommonCharacter;
        }
    }

    public abstract class Hero:Character
    {
        public Hero(Core core)
            :base(core)
        {

        }
        public override CardType GetCardType()
        {
            return CardType.Hero;
        }
    }
    

}
