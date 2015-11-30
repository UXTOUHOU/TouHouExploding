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

        public UnitActionState ActionState { get; set; }

        /// <summary>
        /// 该单位的种类ID
        /// </summary>
        public string UnitBaseId { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 对该单位的描述
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
            this.Mobility = new UnitMobility(unitBase.Mobility, unitBase.MoveMethod);
            this.AttackPower = new UnitAttackPower(unitBase.AttackPower);
            this.AttackRange = new UnitAttackRange(unitBase.AttackRange);
        }
            

        protected Unit(UnitBase unitBase, Player owner)
        {
            InitAttribute(unitBase);
            this.GameCore = owner.GameCore;
            this.Team = owner.atTeam;
            this.Owner = owner;

            Owner.unit.Add(this); // TODO : Need change to Player's function

            this.GameCore.IDP.UID.ApplyID(this);
            ActionState = new UnitActionState();
            foreach (Skill s in skill)
            {
                s.Register(this);
            }
        }

        public static Unit FromUnitBase(UnitBase unitBase, Player owner)
        {
            switch (unitBase.Type)
            {
                case UnitType.Girl:
                    return new UnitGirl(unitBase, owner);
                    break;
                case UnitType.Minion:
                    return new UnitMinion(unitBase, owner);
                    break;
                case UnitType.Servant:
                    return new UnitServant(unitBase, owner);
                    break;
                default:
                    throw new Exception("Undefined Unit Type");
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
        public bool Activate()
        {
            if (ActionState.HaveAction == true) return false;
            //foreach (Unit u in Owner.unit)
            //{
            //    if (u.action.IsAction == true) return false;
            //}
            //↑改为如有激活，自动沉默
            Owner.Unactivition();

            ActionState.IsAction = true;
            return true;
        }
        /// <summary>
        /// 终止某角色的激活，返回原状态是否为激活
        /// </summary>
        /// <returns></returns>
        public bool Deactivate()
        {
            if (ActionState.IsAction == false) return false;

            ActionState.HaveAction = true;
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

            if (ActionState.IsAction == false)//如果没有激活，自动激活
            {
                if (Activate()==false)
                {
                    return false;
                }
            }

                ActionState.HaveAttack = true;
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
            if (ActionState.HaveAction == true) return false;
            if (ActionState.HaveAttack == true) return false;
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
            if (ActionState.HaveAction == true) return false;
            if (ActionState.HaveAttack == true) return false;
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
        public bool MoveTo(int x,int y)
        {
            return MoveTo(GameCore.Chessboard.CellList[x, y]);
        }
        /// <summary>
        /// 移动到某个位置，不能移动返回假
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public bool MoveTo(ChessboardCell region)
        {
            if (CanMove(region) == false)
            {
                return false;
            }

            if (ActionState.IsAction == false)//如果没有激活，自动激活
            {
                if (Activate() == false)
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
            if (ActionState.HaveAction == true) return false;
            if (ActionState.HaveMove == true) return false;
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
            if (Unit_Dying != null)
            {
                Unit_Dying(this);
            }

            Position.unitHere = null;
            Position = null;

            if (Unit_Dead != null)
            {
                Unit_Dead(this);
            }
        }
        public void Reset()
        {
            ActionState.Reset();
            foreach (Skill s in skill)
            {
                s.Reset();
            }
        }

        #region Delegate & Event
        public delegate void Unit_Summoned_Handle(object sender);
        /// <summary>
        /// Trigger when Unit just summoned
        /// </summary>
        public Unit_Summoned_Handle Unit_Summoned;

        public delegate void Unit_Summoning_Handle(object sender);
        /// <summary>
        /// Trigger before Unit summoned
        /// </summary>
        public Unit_Summoning_Handle Unit_Summoning;


        public delegate void Unit_Dead_Handle(object sender);
        /// <summary>
        /// Trigger when Unit just dead
        /// </summary>
        public Unit_Dead_Handle Unit_Dead;


        public delegate void Unit_Dying_Handle(object sender);
        /// <summary>
        /// Trigger before Unit dead
        /// </summary>
        public Unit_Dying_Handle Unit_Dying;

        public delegate void Unit_Moved_Handle(object sender);
        /// <summary>
        /// Trigger when Unit just moved
        /// </summary>
        public Unit_Moved_Handle Unit_Moved;

        public delegate void Unit_Moving_Handle(object sender);
        /// <summary>
        /// Trigger before Unit move
        /// </summary>
        public Unit_Moving_Handle Unit_Moving;

        public delegate void Unit_Attacked_Handle(object sender);
        /// <summary>
        /// Trigger when Unit just attack (another Unit or Object)
        /// </summary>
        public Unit_Attacked_Handle Unit_Attacked;

        public delegate void Unit_Attacking_Handle(object sender);
        /// <summary>
        /// Trigger before Unit attack (another Unit or Object)
        /// </summary>
        public Unit_Attacking_Handle Unit_Attacking;

        public delegate void Unit_Casted_Skill(object sender);
        /// <summary>
        /// Trigger when Unit just casted a skill
        /// </summary>
        public Unit_Casted_Skill Unit_Casted;

        public delegate void Unit_Casting_Skill(object sender);
        /// <summary>
        /// Trigger before Unit cast skill
        /// </summary>
        public Unit_Casting_Skill Unit_Casting;

        public delegate void Unit_Damaged_Handle(object sender);
        /// <summary>
        /// Trigger when Unit was just damaged (by another Unit)
        /// </summary>
        public Unit_Damaged_Handle Unit_Damaged;

        public delegate void Unit_Damaging_Handle(object sender);
        /// <summary>
        /// Trigger before Unit damaged (by another Unit)
        /// </summary>
        public Unit_Damaging_Handle Unit_Damaging;

        public delegate void Unit_Master_Changed_Handle(object sender);
        /// <summary>
        /// Trigger when Unit is temporary controlled by another player
        /// </summary>
        public Unit_Master_Changed_Handle Unit_Master_Changed;

        public delegate void Unit_Owner_Changed_Handle(object sender);
        /// <summary>
        /// Trigger when Unit is permanent controlled by another player
        /// </summary>
        public Unit_Owner_Changed_Handle Unit_Owner_Changed;

        #endregion
    }
}
