using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public class Game
    {
        //public GameConfig gameConfig { get; set; }//游戏设置
        Random random = new Random();//全局随机器
        public IDProvider IDP { get; set; }//ID分配机
        public NeedRespond needRespond { get; set; }//服务器需要挂起询问的对象
        #region 公开对象
        public Chessboard Chessboard
        {
            get
            {
                return _chessboard;
            }
        }//游戏地图
        public Process NowProcess //游戏进度
        {
            get
            {
                return nowProcess;
            }
        }
        private Process nowProcess = Process.RoomPreparing;

        public List<Team> Teams
        {
            get
            {
                return _teams;
            }
        }//游戏所有的队伍
        public int TeamCount
        {
            get
            {
                return _teams.Count;
            }
        }

        public List<Player> Players
        {
            get
            {
                return _players;
            }
        }//所有玩家
        public int PlayerCount
        {
            get
            {
                return _players.Count;
            }
        }
        public Team NeutralTeam;
        public Player NeutralPlayer;

        public Player CurrentPlayer { get; set; }//当前操作的玩家
        public List<SummonCard> Characters { get; set; }//赛场召唤区
        public List<SummonCard> WaitingCharacters { get; set; }//卡池
        public EndReport GameEndReport
        {
            get
            {
                if (NowProcess != Process.RoomEnding) return null;
                return _endReport;
            }
        }

        #endregion

        
        public int Round = 1;
        public DayNight CurrentDayNight//获取当前的白天/黑夜状态
        {
            get
            {
                if ((Round / 2) % 2 == 0)
                {
                    return DayNight.Day;
                }
                else
                {
                    return DayNight.Night;
                }
            }
        }

        #region 私有对象

        private List<Team> _teams = new List<Team>();
        private List<Player> _players = new List<Player>();

        private Chessboard _chessboard;
        private UnitSpool _minionSpool;

        private EndReport _endReport;

        #endregion
        public Game(params Team[] Teams)
        {
            IDP = new IDProvider();
            Characters = new List<SummonCard>();
            foreach (Team aTeam in Teams)
            {
                _teams.Add(aTeam);
            }
            InitTeam();
            InitPlayer();

            InitChessboard();
            InitMinionSpool();
        }

        /// <summary>
        /// 生成游戏棋盘
        /// </summary>
        private void InitChessboard()
        {
            if (_chessboard == null)//如果没有生成地图，注意，如果没有指定队伍或玩家，地图也会重新生成
            {
                _chessboard = new Chessboard(this);
            }
        }

        /// <summary>
        /// 初始化单位池
        /// </summary>
        private void InitMinionSpool()
        {
            if (_minionSpool == null)
            {
                _minionSpool = new UnitSpool(this);
            }
        }


        /// <summary>
        /// 生成默认队伍和玩家
        /// </summary>
        private void InitTeam()
        {
            if (_teams.Count == 0 || _players.Count == 0)
            {
                _teams.Add(new Team(this));
                _teams.Add(new Team(this));
                _teams[0].Add(new Player(Teams[0]));
                _teams[1].Add(new Player(Teams[1]));
            }
            NeutralTeam = new Team(this);
            NeutralPlayer = new Player(NeutralTeam);
        }

        /// <summary>
        /// 安排玩家顺序
        /// </summary>
        private void InitPlayer()
        {
            int maxTeamPlayerCount = 0;
            foreach (Team aTeam in _teams)
            {
                if (aTeam.playerList.Count > maxTeamPlayerCount)
                {
                    maxTeamPlayerCount = aTeam.playerList.Count;
                }
            }

            Players.Add(NeutralPlayer);
            for (int i = 0; i < maxTeamPlayerCount; i++)
            {
                foreach (Team aTeam in _teams)
                {
                    if (aTeam.playerList.Count > i)
                    {
                        _players.Add(aTeam.playerList[i]);
                    }
                }
            }
        }

        private void GameStart()
        {
            _minionSpool.RefreshSpool();

            if (WaitingCharacters == null)//如果没有指定卡堆，生成卡堆
            {
                MakeCardList();
            }
        }
        public bool MakeCardList()
        {
            if (nowProcess != Process.RoomPreparing) return false;

            List<SummonCard> addList = new List<SummonCard>
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

            WaitingCharacters = new List<SummonCard>();
            foreach (SummonCard c in addList)
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
            Team winner = null;
            int temp = 0;
            foreach (Team t in Teams)
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
            if (temp == Teams.Count())
            {
                var endRpt = new EndReport() { state = EndReport.State.Draw };
                _endReport = endRpt;
                GameEnd();
                return endRpt;
            }
            if (temp == Teams.Count() - 1)
            {
                var endRpt = new EndReport() { Winner = winner, state = EndReport.State.SomeoneWin };
                _endReport = endRpt;
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
            if (CurrentPlayer == null) CurrentPlayer = Players[0];//判断是不是第一轮
            else
            {
                int index = Players.IndexOf(CurrentPlayer);//判断是不是一轮结束并指定操作人员
                if (index == Players.Count - 1)
                {
                    RoundOver();
                }
                else
                {
                    CurrentPlayer = Players[index + 1];
                }
            }
        }
        private void Preparing()
        {
            CurrentPlayer.Reset();
        }
        private void Action()
        {

        }
        private void Ending()
        {
            CurrentPlayer.Unactivition();

        }
        private void RoundOver()//所有玩家轮过一次
        {
            Round++;
            CurrentPlayer = Players[0];

            RoundStart();
        }
        private void IncreaseCharacters(int number = 6)//补齐召唤区少女至所需数目
        {
            while (Characters.Count() < number && WaitingCharacters.Count != 0)
            {
                SummonCard toAdd = WaitingCharacters[random.Next(WaitingCharacters.Count)];
                Characters.Add(toAdd);
                WaitingCharacters.Remove(toAdd);
            }
        }



        public Process NextStep()//进入下一个过程 返回下一个过程
        {
            switch (NowProcess)
            {
                case Process.RoomPreparing: GameStart(); nowProcess = Process.RoundStarting; Starting(); break;
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
            public State state;
            public enum State
            {
                SomeoneWin, Draw//,SomeoneExit//一方获胜，平局，一方退出
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
        public enum DayNight { Day, Night }
    }


}
