using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouHou_Exploding
{
    public class Core
    {
        public IDProvider idProvider { get; set; }
        public Map map { set; get; }
        public Team[] team { get; set; }
        public Player[] player { get; set; }
        public Core(GameConfig Setting)
        {

        }
        public class GameConfig
        {
            public GameMod Mod { get; set; }
            public int PlayerNum { get; set; }
            public Map BattleMap { get; set; }
            public Card[] Cards { get; set; }
            public enum GameMod { Common, Master, Custom }
            public GameConfig()
            {
                Mod = GameMod.Common;
                PlayerNum = 2;

            }
        }
        public void Start()
        {

        }
    }
    public class Player:IDProvider.IID
    {
        public int id { get; set; }
        public string name { get; set; }
        public Type PlayerType { get; set; }
        public Team AtTeam { get; set; }
        public Statue PlayerStatue { get; set; }
        public int BDot { get; set; }
        public enum Type { Player, AI, Watcher, Custom }
        public Player()
        {
            
        }
        public class Statue
        {

        }
    }
    public class Team:IDProvider.IID
    {
        private Core _core;
        public int id { get; set; }
        public string name { get; set; }
        public List<Player> playerList { get; set; }
        public Team(Core core)
        {
            _core = core;
            _core.idProvider.TID.ApplyID(this);
        }
        public void Add(Player player)
        {
            playerList.Add(player);
            player.AtTeam = this;
        }
    }
}
