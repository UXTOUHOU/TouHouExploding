using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public class SpellCard : Card
    {
        public Player Owner;
        public SpellCard(Player player)
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
            return CardType.SpellCard;
        }
    }
}
