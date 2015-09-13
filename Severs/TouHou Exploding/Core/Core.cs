using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouHou_Exploding
{
    public class Core
    {
        //public GameConfig gameConfig { get; set; }//游戏设置
        public IDProvider IDP { get; set; }//ID分配机
        public Map RoomMap { set; get; }//游戏地图
        public Process NowProcess //游戏进度
        {
            get
            {
                return nowProcess;
            }
            set
            {
                nowProcess = value;
                switch (nowProcess)
                {
                    case Process.RoomPreparing: break;
                    case Process.RoundStarting: Starting(); break;
                    case Process.RoundPreparing: Preparing(); break;
                    case Process.RoundAction: Action(); break;
                    case Process.RoundEnding: Ending(); break;
                }
            }
        }
        private Process nowProcess;
        public Team[] RoomTeam { get; set; }//游戏所有的队伍
        //public List<Player> player { get; set; }//所有参与的玩家
        public Player PlayerA { get; set; }//玩家A
        public Player PlayerB { get; set; }//玩家B
        public Player NowPlayer { get; set; }//当前操作的玩家
        public List<Character> Characters { get; set; }//赛场召唤区
        //public List<Character> WaitingCharacter { get; set; }//赛场卡牌
        public Time NowTime;
        public Core(Player A=null, Player B=null)
        {
            IDP = new IDProvider();
            RoomTeam = new Team[2];
            RoomTeam[0] = new Team(this);
            RoomTeam[1] = new Team(this);
            RoomMap = new Map(this);
            NowProcess = Process.RoomPreparing;
            RoomTeam = new Team[2];//初始化队伍
            if (A == null) A = new Player();
            if (B == null) B = new Player();
            PlayerA = A;//初始化玩家
            PlayerB = B;
            A.atTeam = RoomTeam[0];
            B.atTeam = RoomTeam[1];
        }
        private void Starting()
        {
            
        }
        private void Preparing()
        {

        }
        private void Action()
        {

        }
        private void Ending()
        {

        }

        public Process NextStep()//进入下一个过程 返回下一个过程
        {
            switch (NowProcess)
            {
                case Process.RoomPreparing: NowProcess = Process.RoundStarting;  break;
                case Process.RoundStarting: NowProcess = Process.RoundPreparing; break;
                case Process.RoundPreparing: NowProcess = Process.RoundAction; break;
                case Process.RoundAction: NowProcess = Process.RoundEnding; break;
                case Process.RoundEnding: NowProcess = Process.RoundStarting; break;
            }
            return NowProcess;
        }
        /*
        public class GameConfig
        {
            public GameMod mod { get; set; }//游戏模式：普通
            public Map.MapSave battleMap { get; set; }//空即为不指定地图，自动生成（暂不支持提前指定地图
            public List<PolicyCard> policyCards { get; set; }//本局系统提供的策略牌，空为玩家自带
            public List<Character> characters { get; set; }//本局系统提供的人物卡，空为自动生成
            public List<Player> player { get; set; }//玩家设定
            public enum GameMod { Common, Master, Custom }
            public GameConfig()
            {
                mod = GameMod.Common;
            }
        }
        */
        public enum Process { RoomPreparing, RoundStarting, RoundPreparing, RoundAction, RoundEnding, RoomEnding }//房间准备，回合开始，回合准备，回合行动，回合结束，房间结算
        public enum Time { Day, Night }
    }
    
    
}
