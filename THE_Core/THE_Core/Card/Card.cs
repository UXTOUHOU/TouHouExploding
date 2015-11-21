
namespace THE_Core
{
    public abstract class Card : IID
    {
        public Game GameCore;
        /// <summary>
        /// 该卡牌每场自动分配的ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 该卡牌所属种类的ID
        /// </summary>
        public virtual string typeID { get; set; }
        public int cost { get; set; }
        public virtual string name { get; set; }
        /// <summary>
        /// 对该卡牌的描述
        /// </summary>
        public virtual string description { get; set; }
        public abstract CardType GetCardType();

        public Card(Game core)
        {
            GameCore = core;
            GameCore.IDP.CID.ApplyID(this);
        }
        /// <summary>
        /// 丢弃卡牌
        /// </summary>
        public virtual void Discard()
        {
            GameCore.IDP.CID.Del(this);
        }
    }

    public enum CardType { PolicyCard, SummonCard, HeroCard, SpellCard }
}
