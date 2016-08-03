using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public class SummonSpellEffect : SpellEffect
    {
        private Game _gameCore;
        private Player _player;
        private Skill _skill;
        private UnitBase _unitBase;
        private ChessboardCell _cbCell;

        public SummonSpellEffect(Game gameCore, Player player, Skill skill, UnitBase unitBase, ChessboardCell cbCell)
        {
            this._gameCore = gameCore;
            this._player = player;
            this._skill = skill;
            this._unitBase = unitBase;
            this._cbCell = cbCell;
        }

        public override void Spell()
        {
            try
            {
                Unit aUnit = Unit.FromUnitBase(_unitBase, _player);
                aUnit.SummonToLocation(_cbCell);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
