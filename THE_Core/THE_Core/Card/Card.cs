
namespace THE_Core
{
    public abstract class Card : IDProvider.IID
    {
        public Game GameCore;
        public int Id { get; set; }//该卡牌每场自动分配的ID

        public virtual string typeID { get; set; }//该卡牌所属种类的ID
        public int cost { get; set; }
        public virtual string name { get; set; }
        public virtual string description { get; set; }//对该卡牌的描述
        public abstract CardType GetCardType();

        public Card(Game core)
        {
            GameCore = core;
            GameCore.IDP.CID.ApplyID(this);
        }
        public virtual void Discard()//丢弃卡牌
        {
            GameCore.IDP.CID.Del(this);
        }
    }

    public enum CardType { PolicyCard, SummonCard, HeroCard, SpellCard }
}
