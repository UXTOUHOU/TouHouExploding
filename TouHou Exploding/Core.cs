using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouHou_Exploding
{
    class Core
    {
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
    class Player
    {
        public int ID { get; set; }
        public string name { get; set; }
        public Type PlayerType { get; set; }
        public Team AtTeam { get; set; }
        public Statue PlayerStatue { get; set; }
        public int BDot { get; set; }
        public enum Type { Player, AI, Watcher, Custom }
        public class Statue
        {

        }
    }
    class Team
    {

    }
}
