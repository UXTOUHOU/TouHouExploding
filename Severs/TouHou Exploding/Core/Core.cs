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
        public Character character { get; set; }//赛场召唤区
        public Core(GameConfig setting)
        {
            if(setting.mod==GameConfig.GameMod.Common)
            {
                if (setting.battleMap == null)
                {
                    map = new Map(this);
                }
                if (setting.policyCards == null)
                {

                }
            }
        }
        public Process NextStep()//返回下一个过程
        {
            return Process.RoomClosing;
        }
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
        public enum Process { RoomClosing,RoomLoading, RoomWaiting, RoomPreparing, RoundStarting, RoundPreparing, RoundAction, RoundEnding, RoomEnding }//房间关闭，房间读取，房间等待，房间准备，回合开始，回合准备，回合行动，回合结束，房间结算
        public void Start()
        {

        }
    }
    
    
}
