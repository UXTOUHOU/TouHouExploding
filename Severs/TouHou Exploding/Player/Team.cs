using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public class Team : IDProvider.IID
    {
        public Core GameCore;
        public int id { get; set; }
        public string name { get; set; }
        public int blood { get; set; }//基地血量
        public List<Player> playerList { get; set; }
        public List<Region> OwnRegion { get; set; }
        public List<Region> Base
        {
            get
            {
                List<Region> R = new List<Region>();
                foreach(Region r in OwnRegion)
                {
                    if (r.specialHere == Region.Special.Base) R.Add(r);
                }
                return R;
            }
        }

        public Team(Core core)
        {
            GameCore = core;
            GameCore.IDP.TID.ApplyID(this);
            playerList = new List<Player>();
            OwnRegion = new List<Region>();
            blood = 4;
        }
        public void Add(Player player)
        {
            if (player.atTeam != null) player.atTeam.Leave(player);
            playerList.Add(player);
            player.atTeam = this;
        }
        public void BeAttacked()
        {
            blood--;
            GameCore.CheckWin();
        }
        public bool IsFailed()//检查该队伍是不是全员失败
        {
            if (blood <= 0) return true;
            foreach (Player p in playerList)
            {
                if(p.HaveFailed() == false)
                    return false;
            }
            return true;
        }
        public void Leave(Player player)
        {
            player.atTeam=null;
            this.playerList.Remove(player);
        }
    }
}
