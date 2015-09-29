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
        
        public Type type { get; set; }
        public Unit.Attribute unit { get; set; }//召唤出的角色属性
        public enum Type{ Common,Hero,Servant }//Common召唤出的为普通单位，Hero为少女单位，Servent为基本效果产生的单位
        //public enum Preset { 毛玉, 天狗, 妖精, 永远亭的兔子, 自爆人形, 河童重工, 博丽灵梦, 魂魄妖梦, 十六夜咲夜, Custom }
        public Unit ToBattle(int[] locate, Player owner)//上战场，把卡片转换为单位。已经召唤过的话会返回空
        {
            if (owner.action.HaveCall == true) return null;
            owner.action.HaveCall = true;
            Unit x =new Unit(this,locate,owner);
            return x;
        }
        public Character(Core core)
            :base(core)
        {
            GameCore = core;
            unit = new Unit.Attribute();
            SetAttribute();
        }
        public override void Discard()//还没写完
        {
            base.Discard();
            if (GameCore.Characters.Contains(this))//判断是否处于卡堆
                GameCore.Characters.Remove(this);
            if (GameCore.WaitingCharacters.Contains(this))//判断是否处于召唤区
                GameCore.WaitingCharacters.Remove(this);
        }
        public abstract void SetAttribute();//继承类必须实现这个方法，在其中写该角色的数值
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
    public class Unit : IDProvider.IID
    {
        public Character card
        {
            get
            {
                return _card;
            }
        }
        private Character _card;
        public int id { get; set; }
        public Core GameCore;
        public Player Owner;
        public Region at { get; set; }
        public Attribute attribute { get; set; }
        public Action action { get; set; }
        public Statue statue { get; set; }
        public Unit(Character transCard, int[] buildLocate, Player owner)
        {
            attribute = transCard.unit.Clone();
            _card = transCard;
            GameCore = _card.GameCore;
            Owner = owner;
            Owner.unit.Add(this);
            _card.GameCore.RoomMap.RegionList[buildLocate[0], buildLocate[1]].MoveHere(this);
            _card.GameCore.IDP.UID.ApplyID(this);
            action = new Action();
        }
        public int GetDistance(Unit a, Unit b = null)//计算ab单位的距离，无法计算返回-1
        {
            if (b == null) b = this;
            if (a.at == null || b.at == null) return -1;
            int distanceX = a.at.locate[0] - b.at.locate[0];
            int distanceY = a.at.locate[1] - b.at.locate[1];
            if (distanceX < 0) distanceX = -distanceX;
            if (distanceY < 0) distanceY = -distanceY;
            return distanceX + distanceY;
        }
        public int GetDistance(Region r)//计算该单位到某格子的距离，无法计算返回-1
        {
            if (this.at == null) return -1;
            int distanceX = this.at.locate[0] - r.locate[0];
            int distanceY = this.at.locate[1] - r.locate[1];
            if (distanceX < 0) distanceX = -distanceX;
            if (distanceY < 0) distanceY = -distanceY;
            return distanceX + distanceY;
        }
        public bool Activition()//激活角色，返回是否成功激活，所有角色中只能同时激活一个
        {
            if (action.HaveAction == true) return false;
            foreach (Unit u in Owner.unit)
            {
                if (u.action.IsAction == true) return false;
            }

            action.IsAction = true;
            return true;
        }
        public bool Unactivition()//终止某角色的激活，返回原状态是否为激活
        {
            if (action.IsAction == false) return false;

            action.HaveAction = true;
            return true;
        }
        public bool Attack(Unit target)//攻击指令，如果不能攻击返回假
       {
            if (CanAttack(target) == false) return false;
            action.HaveAttack = true;
            target.Hurt(attribute.attack);
            return true;
        }
       public virtual bool CanAttack(Unit target)//检测是否可以攻击某单位，重写这个方法可更改射程判断规则
        {
            if (Owner.bDot <= 1) return false;
            if(action.IsAction == false) return false;
            if (action.HaveAction == true) return false;
            if (action.HaveAttack == false) return false;
            return GetDistance(target) <= attribute.range;
        }
       public bool Move(Region region)//移动到某个位置，不能移动返回假
        {
            if(CanMove(region)==false)
            {
                return false;
            }
            Owner.bDot--;
            return region.MoveHere(this);
        }
        public virtual bool CanMove(Region region)//检测是否可以移动到某位置，重写这个方法可更改移动判断规则
        {
            if (Owner.bDot<=1) return false;
            if (action.IsAction == false) return false;
            if (action.HaveAction == true) return false;
            if (action.HaveMove == false) return false;
            Owner.bDot--;
            return GetDistance(region) <= attribute.mobility;
        }
       public virtual int Hurt(int blood)//体力流失，返回剩余血量，死亡返回-1
       {
           attribute.blood -= blood;
           if(attribute.blood<=0)
           {
               Die();
               return -1;
           }
           return attribute.blood;
       }
       public virtual void Die()
       {
            at.unitHere = null;
            at = null;
            _card.GameCore.IDP.UID.Del(this);
            Owner.unit.Remove(this);
            if (card.GetCardType() == Card.CardType.Hero)
            {
                Owner.deadCard.Add(this);

            }
       } 
        public void Reset()
        {
            action.Reset();
            foreach(Skill s in attribute.skill)
            {
                s.Reset();
            }
        }
        public class Action//记录该单位已经进行过的行动
        {
            public bool IsAction = false;
            public bool HaveAction = false;
            public bool HaveAttack = false;
            public bool HaveMove = false;
            public void Reset()
            {
                IsAction = false;
                HaveAction = false;
                HaveAttack = false;
                HaveMove = false;
            }
        }
       public class Attribute//属性
       {
           public string name { get; set; }//不解释
           public Character.Type type { get; set; }//角色类型
           public int blood { get; set; }//血量
           public int mobility { get; set; }//机动性
           public int attack { get; set; }//攻击伤害
           public int range { get; set; }//射程
           public List<Skill> skill { get; set; }//单位技能
           public MoveMethous moveMethous;//移动方式
           public Attribute Clone()
           {
               return (Attribute)this.MemberwiseClone();
           }
           public enum MoveMethous{ Walk,Fly,Transport }
       }
       public class Statue//人物附加状态
       {

       }
    }
    //public class HeroUnit : Unit
    //{
    //    public HeroUnit(HeroCard heroCard,int[] locate)
    //        : base(heroCard,locate)
    //    {

    //    }
    //    public override void Die()
    //    {
            
    //        base.Die();
    //    }
    //}

}
