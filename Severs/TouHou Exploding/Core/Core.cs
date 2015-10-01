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
        Random random = new Random();//全局随机器
        public IDProvider IDP { get; set; }//ID分配机
        public Map RoomMap { set; get; }//游戏地图
        public Process NowProcess //游戏进度
        {
            get
            {
                return nowProcess;
            }
        }
        private Process nowProcess = Process.RoomPreparing;
        public Team[] RoomTeam { get; set; }//游戏所有的队伍
        //public List<Player> player { get; set; }//所有参与的玩家
        public List<Player> Players { get; set; }//所有
        public Player NowPlayer { get; set; }//当前操作的玩家
        public List<Character> Characters { get; set; }//赛场召唤区
        public List<Character> WaitingCharacters { get; set; }//卡池
        public EndReport GameEndReport
        {
            get
            {
                if (NowProcess != Process.RoomEnding) return null;
                return _endRpt;
            }
        }
        private EndReport _endRpt;
        public int Round = 1;
        public Time NowTime//获取当前的白天/黑夜状态
        {
            get
            {
                if ((Round / 2) % 2 == 0)
                {
                    return Time.Day;
                }
                else
                {
                    return Time.Night;
                }
            }
        }
        public Core()//输入参数，Team和Player要是想指定要同时输入
        {
            IDP = new IDProvider();
            Characters = new List<Character>();
        }

        private void GameStart()
        {
            if (RoomTeam == null || Players == null)//如果没有，生成队伍
            {
                RoomTeam = new Team[2];//初始化队伍
                RoomTeam[0] = new Team(this);
                RoomTeam[1] = new Team(this);
                Players = new List<Player>();
                Players.Add(new Player(RoomTeam[0]));//初始化玩家
                Players.Add(new Player(RoomTeam[1]));
                RoomMap = new Map(this);
            }
            
            if (RoomMap == null)//如果没有生成地图，注意，如果没有指定队伍或玩家，地图也会重新生成
            {
                RoomMap = new Map(this);
            }
   
            if(WaitingCharacters == null)//如果没有指定卡堆，生成卡堆
            {
                MakeCardList();
            }


        }
        public bool MakeCardList()
        {
            if (nowProcess != Process.RoomPreparing) return false;

            List<Character> addList = new List<Character>
            {
                new MissTest(this),
                new MissTest(this),
                new MissTest(this),
                new MissTest(this),
                new MissTest(this),
                new MissTest(this),
                new MissTest(this),
                new MissTest(this),
                new MissTest(this),
                new MissTest(this),
                new MissTest(this)
            };

            WaitingCharacters = new List<Character>();
            foreach (Character c in addList)
            {
                WaitingCharacters.Add(c);
            }
            return true;
        }
        private void GameEnd()//游戏结局执行方法
        {
            if (NowProcess == Process.RoomPreparing) return;
            nowProcess = Process.RoomEnding;
        }
        public EndReport CheckWin()//未完成，检查是否有人胜利，输出战报信息暂未结束获胜输出null
        {
            if (NowProcess == Process.RoomPreparing) return null;
            Team winner=null;
            int temp = 0;
            foreach(Team t in RoomTeam)
            {
                if (t.IsFailed() == true)
                {
                    temp++;
                }
                else
                {
                    winner = t;
                }
            }
            if (temp == RoomTeam.Count())
            {
                var endRpt = new EndReport() { statue = EndReport.Statue.Draw };
                _endRpt = endRpt;
                GameEnd();
                return endRpt;
            }
            if (temp == RoomTeam.Count() - 1)
            {
                var endRpt = new EndReport() { Winner = winner, statue=EndReport.Statue.SomeoneWin };
                _endRpt = endRpt;
                GameEnd();
                return endRpt;
            }
            return null;
        }

        private void RoundStart()
        {
            
        }
        private void Starting()
        {
            IncreaseCharacters();//补充召唤区
            if (NowPlayer == null) NowPlayer = Players[0];//判断是不是第一轮
            else
            {
                int index = Players.IndexOf(NowPlayer);//判断是不是一轮结束并指定操作人员
                if(index  == Players.Count - 1)
                {
                    RoundOver();
                }
                else
                {
                    NowPlayer = Players[index + 1];
                }
            }
        }
        private void Preparing()
        {
            NowPlayer.Reset();
        }
        private void Action()
        {

        }
        private void Ending()
        {
            NowPlayer.Unactivition();
            
        }
        private void RoundOver()//所有玩家轮过一次
        {
            Round++;
            NowPlayer = Players[0];

            RoundStart();
        }
        private void IncreaseCharacters(int number=6)//补齐召唤区少女至所需数目
        {
            while (Characters.Count() < number&&WaitingCharacters.Count!=0)
            {
                Character toAdd = WaitingCharacters[random.Next(WaitingCharacters.Count)];
                Characters.Add(toAdd);
                WaitingCharacters.Remove(toAdd);
            }
        }



        public Process NextStep()//进入下一个过程 返回下一个过程
        {
            switch (NowProcess)
            {
                case Process.RoomPreparing: GameStart(); nowProcess = Process.RoundStarting;  Starting(); break;
                case Process.RoundStarting: nowProcess = Process.RoundPreparing; Preparing(); break;
                case Process.RoundPreparing: nowProcess = Process.RoundAction; Action(); break;
                case Process.RoundAction: nowProcess = Process.RoundEnding; Ending(); break;
                case Process.RoundEnding: nowProcess = Process.RoundStarting; Starting(); break;
            }
            return NowProcess;
        }
        
        public class EndReport//输出完结战报
        {
            public Team Winner;
            public Statue statue;
            public enum Statue
            {
                SomeoneWin,Draw//,SomeoneExit//一方获胜，平局，一方退出
            }
        }

        //public class GameConfig
        //{
        //    public GameMod mod { get; set; }//游戏模式默认：普通
        //    public Map.MapSave battleMap { get; set; }//空即为不指定地图，自动生成（暂不支持提前指定地图
        //    public enum GameMod { Common, Master, Custom }
        //    public GameConfig()
        //    {
                
        //    }
        //}
        
        public enum Process { RoomPreparing, RoundStarting, RoundPreparing, RoundAction, RoundEnding, RoomEnding }//房间准备，回合开始，回合准备，回合行动，回合结束，房间结算
        public enum Time { Day, Night }
    }
    
    
}
