using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public abstract class PolicyCard : Card
    {
        public Player Owner;
        public PolicyCard(Player player)
            : base(player.GameCore)
        {
            GameCore = player.GameCore;
            Owner = player;
            player.policyCard.Add(this);
            GameCore.IDP.CID.ApplyID(this);
        }
        public override void Discard()
        {
            base.Discard();
            Owner.policyCard.Remove(this);
        }

        public override CardType GetCardType()
        {
            return CardType.PolicyCard;
        }
        public abstract bool Fuction(UseInput uInput = null); //策略牌使用时执行的操作

        public virtual bool Use(UseInput uInput = null) //任何一个策略牌的使用方法
        {
            if (Owner.bDot < cost)
            {
                return false;
            }
            Owner.policyCard.Remove(this);
            GameCore.IDP.CID.Del(this);
            if (Fuction(uInput))
            {
                Owner.bDot -= cost;
                return true;
            }
            else
            {
                return false;
            }
        }
        public class UseInput//新输入继承此类
        {

        }
    }
}
