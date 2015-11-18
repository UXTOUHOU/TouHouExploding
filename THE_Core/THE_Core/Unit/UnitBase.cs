using System;
using System.Collections.Generic;

namespace THE_Core
{
    public class UnitBase : IDProvider.IID
    {
        public Character card
        {
            get
            {
                return _card;
            }
        }
        private Character _card;
        public int Id { get; set; }
        public Core GameCore;
        public Player Owner;
        public ChessboardCell Position { get; set; }

        public Action action { get; set; }
        public State state { get; set; }

        public string typeID { get; set; }//该单位的种类ID
        public string name { get; set; }//不解释
        public string description { get; set; }//对该角色的描述
        public Character.Type type { get; set; }//角色类型

        public UnitAttribute HP { get; set; }
        public UnitAttribute Mobility { get; set; }
        public UnitAttribute AttackPower { get; set; }
        public UnitAttribute AttackRange { get; set; }

        public List<Skill> skill { get; set; }//单位技能

        public bool IsDead
        {
            get
            {
                if (HP.Current> 0) return false;
                else return true;
            }
        }

        public void InitAttribute(Character transCare)
        {
            throw new NotImplementedException();
        }
            

        public UnitBase(Character transCard, int[] buildLocate, Player owner)
        {
            InitAttribute(transCard);
            _card = transCard;
            GameCore = _card.GameCore;
            Owner = owner;
            Owner.unit.Add(this);
            _card.GameCore.RoomMap.RegionList[buildLocate[0], buildLocate[1]].MoveHere(this);
            _card.GameCore.IDP.UID.ApplyID(this);
            action = new Action();
            foreach (Skill s in skill)
            {
                s.Register(this);
            }
        }
        public int GetDistance(UnitBase a, UnitBase b = null)//计算ab单位的距离，无法计算返回-1
        {
            if (b == null) b = this;
            if (a.Position == null || b.Position == null) return -1;
            int distanceX = a.Position.locate.X - b.Position.locate.X;
            int distanceY = a.Position.locate.Y - b.Position.locate.Y;
            if (distanceX < 0) distanceX = -distanceX;
            if (distanceY < 0) distanceY = -distanceY;
            return distanceX + distanceY;
        }
        public int GetDistance(ChessboardCell r)//计算该单位到某格子的距离，无法计算返回-1
        {
            if (this.Position == null) return -1;
            int distanceX = this.Position.locate.X - r.locate.X;
            int distanceY = this.Position.locate.Y - r.locate.Y;
            if (distanceX < 0) distanceX = -distanceX;
            if (distanceY < 0) distanceY = -distanceY;
            return distanceX + distanceY;
        }
        public bool Activition()//激活角色，返回是否成功激活，所有角色中只能同时激活一个
        {
            if (action.HaveAction == true) return false;
            //foreach (Unit u in Owner.unit)
            //{
            //    if (u.action.IsAction == true) return false;
            //}
            //↑改为如有激活，自动沉默
            Owner.Unactivition();

            action.IsAction = true;
            return true;
        }
        public bool Unactivition()//终止某角色的激活，返回原状态是否为激活
        {
            if (action.IsAction == false) return false;

            action.HaveAction = true;
            return true;
        }
        public bool Attack(UnitBase target)//攻击指令，如果不能攻击返回假
        {
            if (CanAttack(target) == false) return false;

            if (action.IsAction == false)//如果没有激活，自动激活
            {
                if (Activition()==false)
                {
                    return false;
                }
            }

                action.HaveAttack = true;
            target.Hurt(attribute.attack);
            return true;
        }
        public virtual bool CanAttack(UnitBase target)//检测是否可以攻击某单位，重写这个方法可更改射程判断规则
        {
            if (Owner.bDot <= 1) return false;
            if (action.HaveAction == true) return false;
            if (action.HaveAttack == true) return false;
            return GetDistance(target) <= AttackRange.Current;
        }
        public virtual bool CanAttack(ChessboardCell region)//检测是否可以攻击某位置，重写这个方法可更改射程判断规则
        {
            if (Owner.bDot <= 1) return false;
            if (action.HaveAction == true) return false;
            if (action.HaveAttack == true) return false;
            return GetDistance(region) <= AttackRange.Current;
        }

        public virtual bool AttackBase(Team beAttacked)
        {
            if (CanAttackBase(beAttacked))
            {
                beAttacked.BeAttacked();
                return true;
            }
            else
            {
                return false;
            }
        }
        public virtual bool AttackBase(Player beAttacked)
        {
            return AttackBase(beAttacked.atTeam);
        }
        public virtual bool CanAttackBase(Team beAttacked)
        {
            if (beAttacked.blood <= 0) return false;
            foreach(ChessboardCell r in beAttacked.Base)
            {
                if (CanAttack(r)) return true;
            }
            return false;
        }
        public bool Move(int x,int y)
        {
            return Move(GameCore.RoomMap.RegionList[x, y]);
        }
        public bool Move(ChessboardCell region)//移动到某个位置，不能移动返回假
        {
            if (CanMove(region) == false)
            {
                return false;
            }

            if (action.IsAction == false)//如果没有激活，自动激活
            {
                if (Activition() == false)
                {
                    return false;
                }
            }

            Owner.bDot--;
            return region.MoveHere(this);
        }
        public virtual bool CanMove(ChessboardCell region)//检测是否可以移动到某位置，重写这个方法可更改移动判断规则
        {
            if (Owner.bDot <= 1) return false;
            if (action.HaveAction == true) return false;
            if (action.HaveMove == true) return false;
            if (region.unitHere != null) return false;
            Owner.bDot--;
            return GetDistance(region) <= Mobility.Current;
        }
        public virtual int Hurt(int blood)//体力流失，返回剩余血量，死亡返回-1
        {
            HP.Current -= blood;
            if (HP.Current <= 0)
            {
                Die();
                return -1;
            }
            return HP.Current;
        }
        public virtual void Die()
        {
            Position.unitHere = null;
            Position = null;
            //_card.GameCore.IDP.UID.Del(this);
            //↑单位死亡还是别除名了。。。要是哪天那帮没节操的策划编个复活技能……
            Owner.unit.Remove(this);
            if (card.GetCardType() == Card.CardType.Hero)
            {
                Owner.deadCard.Add(this);

            }
        }
        public void Reset()
        {
            action.Reset();
            foreach (Skill s in skill)
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
            public string typeID { get; set; }//该单位的种类ID
            public string name { get; set; }//不解释
            public string description { get; set; }//对该角色的描述
            public Character.Type type { get; set; }//角色类型
            public int blood { get; set; }//血量
            public int mobility { get; set; }//机动性
            public int attack { get; set; }//攻击伤害
            public int range { get; set; }//射程
            public List<Skill> skill { get; set; }//单位技能
            public MoveMethous moveMethous;//移动方式
            public Attribute()
            {
                skill = new List<Skill>();
            }
            public Attribute Clone()
            {
                return (Attribute)this.MemberwiseClone();
            }
            public enum MoveMethous { Walk, Fly, Transport }
        }
        public class State//人物附加状态
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
