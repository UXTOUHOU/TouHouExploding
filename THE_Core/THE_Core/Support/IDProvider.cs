using System;

namespace THE_Core
{
    public class IDProvider//用于分发临时ID，被分发对象需继承接口IID，未分配时为-1
    {
        //以下为动态ID分配
        public IDList CID { get; set; }//(CardID)-区别卡牌(召唤区少女也算卡牌)
        public IDList UID { get; set; }//(UnitID)-区别单位-由卡牌/技能召唤出的单位ID
        public IDList PID { get; set; }//(PlayerID)-区别玩家
        public IDList RID { get; set; }//(RegionID)-区别不同地区方块
        public IDList TID { get; set; }//(TeamID)-区别不同队
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
