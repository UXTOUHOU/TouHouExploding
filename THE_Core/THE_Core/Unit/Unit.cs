using System;
using System.Collections.Generic;

namespace THE_Core
{
    public abstract class Unit : IID
    {
        public int Id { get; set; }

        public UnitBase UnitBase
        {
            get;
            private set;
        }

        public Game GameCore;
        public Team Team;
        public Player Owner;
        public Player Master;
        public ChessboardCell Position { get; set; }

        public Action action { get; set; }
        public State state { get; set; }

        /// <summary>
        /// 该单位的种类ID
        /// </summary>
        public string UnitBaseId { get; set; }
        /// <summary>
        /// 不解释
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 对该角色的描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 角色类型
        /// </summary>
        public UnitType UnitType { get; set; }

        public UnitAttribute HitPoint { get; set; }
        public UnitAttribute Mobility { get; set; }
        public UnitAttribute AttackPower { get; set; }
        public UnitAttribute AttackRange { get; set; }

        /// <summary>
        /// 单位技能
        /// </summary>
        public List<Skill> skill { get; set; }

        public bool IsDead
        {
            get
            {
                if (HitPoint.Current> 0) return false;
                else return true;
            }
        }

        public void InitAttribute(UnitBase unitBase)
        {
            this.UnitBase = unitBase;
            this.UnitBaseId = unitBase.Id;
            this.UnitType = unitBase.Type;
            this.HitPoint = new UnitHitPoint(unitBase.HitPoint);
            this.Mobility = new UnitMobility(unitBase.Mobility);
            this.AttackPower = new UnitAttackPower(unitBase.AttackPower);
            this.AttackRange = new UnitAttackRange(unitBase.AttackRange);
        }
            

        public Unit(UnitBase unitBase, Player owner)
        {
            InitAttribute(unitBase);
            this.GameCore = owner.GameCore;
            this.Team = owner.atTeam;
            this.Owner = owner;

            Owner.unit.Add(this); // TODO : Need to change to Player's function
            this.GameCore.Chessboard.CellList[buildLocate[0], buildLocate[1]].MoveHere(this);
            this.GameCore.IDP.UID.ApplyID(this);
            action = new Action();
            foreach (Skill s in skill)
            {
                s.Register(this);
            }
        }
        /// <summary>
        /// 计算ab单位的距离，无法计算返回-1
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int GetDistance(Unit a, Unit b = null)
        {
            if (b == null) b = this;
            if (a.Position == null || b.Position == null) return -1;
            int distanceX = a.Position.locate.X - b.Position.locate.X;
            int distanceY = a.Position.locate.Y - b.Position.locate.Y;
            if (distanceX < 0) distanceX = -distanceX;
            if (distanceY < 0) distanceY = -distanceY;
            return distanceX + distanceY;
        }
        /// <summary>
        /// 计算该单位到某格子的距离，无法计算返回-1
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public int GetDistance(ChessboardCell r)
        {
            if (this.Position == null) return -1;
            int distanceX = this.Position.locate.X - r.locate.X;
            int distanceY = this.Position.locate.Y - r.locate.Y;
            if (distanceX < 0) distanceX = -distanceX;
            if (distanceY < 0) distanceY = -distanceY;
            return distanceX + distanceY;
        }
        /// <summary>
        /// 激活角色，返回是否成功激活，所有角色中只能同时激活一个
        /// </summary>
        /// <returns></returns>
        public bool Activition()
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
        /// <summary>
        /// 终止某角色的激活，返回原状态是否为激活
        /// </summary>
        /// <returns></returns>
        public bool Unactivition()
        {
            if (action.IsAction == false) return false;

            action.HaveAction = true;
            return true;
        }
        /// <summary>
        /// 攻击指令，如果不能攻击返回假
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool Attack(Unit target)
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
            target.Hurt(AttackPower.Current);
            return true;
        }
        /// <summary>
        /// 检测是否可以攻击某单位，重写这个方法可更改射程判断规则
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual bool CanAttack(Unit target)//
        {
            if (Owner.bDot <= 1) return false;
            if (action.HaveAction == true) return false;
            if (action.HaveAttack == true) return false;
            return GetDistance(target) <= AttackRange.Current;
        }
        /// <summary>
        /// 检测是否可以攻击某位置，重写这个方法可更改射程判断规则
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public virtual bool CanAttack(ChessboardCell region)
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
            return Move(GameCore.Chessboard.CellList[x, y]);
        }
        /// <summary>
        /// 移动到某个位置，不能移动返回假
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public bool Move(ChessboardCell region)
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
        /// <summary>
        /// 检测是否可以移动到某位置，重写这个方法可更改移动判断规则
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public virtual bool CanMove(ChessboardCell region)
        {
            if (Owner.bDot <= 1) return false;
            if (action.HaveAction == true) return false;
            if (action.HaveMove == true) return false;
            if (region.unitHere != null) return false;
            Owner.bDot--;
            return GetDistance(region) <= Mobility.Current;
        }
        /// <summary>
        /// 体力流失，返回剩余血量，死亡返回-1
        /// </summary>
        /// <param name="blood"></param>
        /// <returns></returns>
        public virtual int Hurt(int blood)
        {
            HitPoint.Current -= blood;
            if (HitPoint.Current <= 0)
            {
                Die();
                return -1;
            }
            return HitPoint.Current;
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
        /// <summary>
        /// 记录该单位已经进行过的行动
        /// </summary>
        public class Action
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
        /// <summary>
        /// 属性
        /// </summary>
        public class Attribute
        {
            /// <summary>
            /// 该单位的种类ID
            /// </summary>
            public string typeID { get; set; }
            /// <summary>
            /// 不解释
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 对该角色的描述
            /// </summary>
            public string description { get; set; }
            /// <summary>
            /// 角色类型
            /// </summary>
            public UnitType type { get; set; }
            /// <summary>
            /// 血量
            /// </summary>
            public int blood { get; set; }
            /// <summary>
            /// 机动性
            /// </summary>
            public int mobility { get; set; }
            /// <summary>
            /// 攻击伤害
            /// </summary>
            public int attack { get; set; }
            /// <summary>
            /// 射程
            /// </summary>
            public int range { get; set; }
            /// <summary>
            /// 单位技能
            /// </summary>
            public List<Skill> skill { get; set; }
            /// <summary>
            /// 移动方式
            /// </summary>
            public MoveMethous moveMethous;
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
        /// <summary>
        /// 人物附加状态
        /// </summary>
        public class State
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
