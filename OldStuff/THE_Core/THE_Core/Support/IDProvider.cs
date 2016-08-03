using System;

namespace THE_Core
{
    /// <summary>
    /// 用于分发临时ID，被分发对象需继承接口IID，未分配时为-1
    /// </summary>
    public class IDProvider
    {
        //以下为动态ID分配
        /// <summary>
        /// (CardID)-区别卡牌(召唤区少女也算卡牌)
        /// </summary>
        public IDList CID { get; set; }
        /// <summary>
        /// (UnitID)-区别单位-由卡牌/技能召唤出的单位ID
        /// </summary>
        public IDList UID { get; set; }
        /// <summary>
        /// (PlayerID)-区别玩家
        /// </summary>
        public IDList PID { get; set; }
        /// <summary>
        /// (RegionID)-区别不同地区方块
        /// </summary>
        public IDList RID { get; set; }
        /// <summary>
        /// (TeamID)-区别不同队
        /// </summary>
        public IDList TID { get; set; }
        public IDProvider()
        {
            CID = new IDList(typeof(Card));
            UID = new IDList(typeof(Unit));
            PID = new IDList(typeof(Player));
            RID = new IDList(typeof(ChessboardCell));
            TID = new IDList(typeof(Team));
        }   
    }
    public interface IID
    {
        int Id { get; set; }
    }
}
