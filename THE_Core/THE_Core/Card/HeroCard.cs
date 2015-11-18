using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public abstract class HeroCard : SummonCard
    {
        public HeroCard(Game core)
            : base(core)
        {

        }
        public override CardType GetCardType()
        {
            return CardType.HeroCard;
        }
    }
}
