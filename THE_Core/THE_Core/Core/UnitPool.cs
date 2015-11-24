using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public class UnitPool
    {
        private Game _game;
        private List<UnitBase> _avaliableUnits;
        private List<Unit> _selectedUnits = new List<Unit>();
        private Random _random = new Random();

        public int MaxUnit
        {
            get;
            private set;
        }

        public UnitPool(Game game, int MaxUnitNumber = 6)
        {
            this._game = game;
            this.MaxUnit = MaxUnitNumber;
            InitUnitPool();
        }

        private void InitUnitPool()
        {
            SetAllUnitList();
            _avaliableUnits = GenerateAvaliableUnitBaseList();
        }

        public void FillPool()
        {
            while (_selectedUnits.Count < this.MaxUnit)
            {
                int selectedIndex = _random.Next(0, _avaliableUnits.Count - 1);
                Unit aUnit = Unit.FromUnitBase(_avaliableUnits[selectedIndex], _game.NeutralPlayer);
                _selectedUnits.Add(aUnit);
            }
        }

        public void RefreshPool()
        {
            _selectedUnits.Clear();
            FillPool();
        }

        #region 静态部分

        public static List<UnitBase> AllUnitBase = new List<UnitBase>();

        private static void SetAllUnitList()
        {
            // TODO: Generate from database;
            throw new NotImplementedException();

            StringReader sr = new StringReader(Properties.Resources.THE_Data);
            XmlSerializer xs = new XmlSerializer(typeof(THE_Data));
            THE_Data data = (THE_Data)xs.Deserialize(sr);

            AllUnitBase.Clear();

            foreach (THE_Data.UnitRow ur in data.Units)
            {
                UnitBase unitBase = UnitBase.FromUnitRow(ur);
                AllUnitBase.Add(unitBase);
            }

        }

        private static List<UnitBase> GenerateAvaliableUnitBaseList()
        {
            List<UnitBase> result = new List<UnitBase>();
            var avaliableUnitBaseQuery = from UnitBase aUnitBase in AllUnitBase
                                         where aUnitBase.Avaliable
                                         select aUnitBase;
            result = avaliableUnitBaseQuery.ToList<UnitBase>();
            return result;
        }
        #endregion
    }
}
