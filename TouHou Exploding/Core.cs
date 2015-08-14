using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouHou_Exploding
{
    public class Core
    {
        public GameConfig gameConfig { get; set; }//游戏设置
        public IDProvider idProvider { get; set; }//ID分配机
        public Map map { set; get; }//游戏地图
        public Process process { get; set; }//游戏进度
        public Team[] team { get; set; }//游戏所有的队伍
        public List<Player> player { get; set; }//所有参与的玩家
        public Player nowPlayer { get; set; }//当前操作的玩家
        public Core(GameConfig setting)
        {
            if(setting.mod==GameConfig.GameMod.Common)
            {
                
            }
        }
        public Process NextStep()//返回下一个过程
        {
            return Process.RoomClosing;
        }
        public class GameConfig
        {
            public GameMod mod { get; set; }//游戏模式：普通
            public int playerNum { get; set; }//游戏人数：2
            public Map.MapSave battleMap { get; set; }//空即为不指定地图，自动生成（暂不支持提前指定地图
            public List<PolicyCard> policyCards { get; set; }//本局系统提供的策略牌，空为自动生成
            public List<Character> characters { get; set; }//本局系统提供的人物卡，空为自动生成
            public enum GameMod { Common, Master, Custom }
            public GameConfig()
            {
                mod = GameMod.Common;
                playerNum = 2;
            }
        }
        public enum Process { RoomClosing,RoomLoading, RoomWaiting, RoomPreparing, RoundStarting, RoundPreparing, RoundAction, RoundEnding, RoomEnding }//房间关闭，房间读取，房间等待，房间准备，回合开始，回合准备，回合行动，回合结束，房间结算
        public void Start()
        {

        }
    }
    public class Player:IDProvider.IID
    {
        public int id { get; set; }
        public string name { get; set; }
        public Type playerType { get; set; }
        public Team atTeam { get; set; }
        public int baseBlood { get; set; }
        public Statue playerStatue { get; set; }
        public int bDot { get; set; }
        public List<PolicyCard> policyCard { get; set; }//玩家手中的策略牌
        public List<Character> character { get; set; }//玩家的召唤去
        public List<Unit> unit { get; set; }//玩家场上的单位
        public List<Unit> deadCard { get; set; }//击毁区
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
            player.atTeam = this;
        }
    }
}
