using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public class UnitSpool
    {
        private Game _game;
        private List<Unit> _avaliableUnits;
        private List<Unit> _selectedUnits = new List<Unit>();
        private Random _random = new Random();

        public int MaxUnit
        {
            get;
            private set;
        }

        public UnitSpool(Game game, int MaxUnitNumber = 6)
        {
            this._game = game;
            this.MaxUnit = MaxUnitNumber;
            InitUnitsSpool();
        }

        private void InitUnitsSpool()
        {
            SetAllUnitList();
            _avaliableUnits = GenerateAvaliableUnitList();
        }

        public void FillSpool()
        {
            while (_selectedUnits.Count < this.MaxUnit)
            {
                int selectedIndex = _random.Next(0, _avaliableUnits.Count - 1);
                _selectedUnits.Add(_avaliableUnits[selectedIndex]);
            }
        }

        public void RefreshSpool()
        {
            _selectedUnits.Clear();
            FillSpool();
        }

        #region 静态部分

        public static List<Unit> AllUnits = new List<Unit>();

        private static void SetAllUnitList()
        {
            // TODO: Generate from database;
            throw new NotImplementedException();
        }

        private static List<Unit> GenerateAvaliableUnitList()
        {
            List<Unit> result = new List<Unit>();
            foreach (Unit aUnit in AllUnits)
            {
                result.Add(aUnit);
            }
            return result;
        }
        #endregion
    }
}
