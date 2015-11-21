using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public class Player : IID
    {
        public Game GameCore;
        public int Id { get; set; }
        public string name { get; set; }
        public Type playerType { get; set; }
        public Team atTeam { get; set; }
        public int blood {
            get
            {
                return atTeam.blood;
            }
        }
        public State playerstate { get; set; }
        public int bDot { get; set; }
        public Action action;
        /// <summary>
        /// 玩家手中的策略牌
        /// </summary>
        public List<PolicyCard> policyCard { get; set; }
        /// <summary>
        /// 玩家场上的单位
        /// </summary>
        public List<Unit> unit { get; set; }
        /// <summary>
        /// 击毁区
        /// </summary>
        public List<Unit> deadCard { get; set; }
        public enum Type { Player, AI, Watcher, Custom }
        public Player(Team team)
        {
            GameCore = team.GameCore;
            GameCore.IDP.PID.ApplyID(this);
            team.Add(this);
            action = new Action();
            policyCard = new List<PolicyCard>();
            unit = new List<Unit>();
            deadCard = new List<Unit>();
        }
        public bool Unactivition()
        {
            bool result = false;
            foreach (Unit u in unit)
            {
                if (u.Unactivition()) result = true;
            }
            return result;
        }
        public void Reset()
        {
            if(bDot<8) bDot = 8;
            action.Reset();
            foreach(Unit u in unit)
            {
                u.Reset();
            }
        }
        public Unit Call(SummonCard character,int[] locate)
        {
            return character.ToBattle(locate, this);
        }
        public void Surrender()
        {
            action.HaveSurrender = true;
            GameCore.CheckWin();
        }
        public bool HaveFailed()
        {
            if (action.HaveSurrender == true) return true;
            if (deadCard.Count >= 6) return true;
            if(atTeam.blood <= 0) return true;
            return false;
        }
        public void BaseAttack()
        {
            atTeam.BeAttacked();
        }
        public class State
        {

        }
        public class Action
        {
            public bool HaveCall = false;
            public bool HaveSurrender = false;
            public void Reset()
            {
                HaveCall = false;
            }
        }
        /*
        public class PlayerSave
        {
            public int id { get; set; }
            public string name { get; set; }
            public Player.Type type { get; set; }//人物类型
            public int RoomTeam { set; get; }//队伍，只有0 1
            public List<PolicyCard> policyCard { get; set; }//玩家手中的策略牌
        }
        */
    }
}
