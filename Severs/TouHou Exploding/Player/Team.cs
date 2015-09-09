using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouHou_Exploding
{
    public class Team : IDProvider.IID
    {
        private Core _core;
        public int id { get; set; }
        public string name { get; set; }
        public List<Player> playerList { get; set; }
        public Team(Core core)
        {
            _core = core;
            _core.IDP.TID.ApplyID(this);
        }
        public void Add(Player player)
        {
            playerList.Add(player);
            player.atTeam = this;
        }
    }
}
