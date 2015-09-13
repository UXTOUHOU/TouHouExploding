using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouHou_Exploding
{
    public class Player : IDProvider.IID
    {
        public int id { get; set; }
        public string name { get; set; }
        public Type playerType { get; set; }
        public Team atTeam { get; set; }
        public int baseBlood { get; set; }
        public Statue playerStatue { get; set; }
        public int bDot { get; set; }
        public List<PolicyCard> policyCard { get; set; }//玩家手中的策略牌
        public List<Unit> unit { get; set; }//玩家场上的单位
        public List<Unit> deadCard { get; set; }//击毁区
        public enum Type { Player, AI, Watcher, Custom }
        public Player()
        {

        }
        public class Statue
        {

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
