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
        private List<UnitBase> _avaliableUnits;
        private List<UnitBase> _selectedUnits = new List<UnitBase>();
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

        public static List<UnitBase> AllUnits = new List<UnitBase>();

        private static void SetAllUnitList()
        {
            // TODO: Generate from database;
            throw new NotImplementedException();
        }

        private static List<UnitBase> GenerateAvaliableUnitList()
        {
            List<UnitBase> result = new List<UnitBase>();
            foreach (UnitBase aUnit in AllUnits)
            {
                result.Add(aUnit);
            }
            return result;
        }
        #endregion
    }
}
