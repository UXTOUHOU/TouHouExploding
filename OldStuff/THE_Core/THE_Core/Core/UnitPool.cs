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

        /// <summary>
        /// 将单位池填满
        /// </summary>
        public void Fill()
        {
            while (_selectedUnits.Count < this.MaxUnit)
            {
                AddRandomUnit();
            }
        }

        /// <summary>
        /// 将随机单位加入单位池
        /// </summary>
        public void AddRandomUnit()
        {
            int selectedIndex = _random.Next(0, _avaliableUnits.Count - 1);
            Unit aUnit = Unit.FromUnitBase(_avaliableUnits[selectedIndex], _game.NeutralPlayer);
            _selectedUnits.Add(aUnit);
        }

        /// <summary>
        /// 将数个随机单位加入单位池
        /// </summary>
        /// <param name="number">数量</param>
        /// <param name="overFill">是否可以超过单位池上限</param>
        public void AddRandomUnit(int number, bool overFill = false)
        {
            for (int i = 0; i < number; i++)
            {
                AddRandomUnit();
                if (!overFill && _selectedUnits.Count >= this.MaxUnit)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 将特定单位加入单位池
        /// </summary>
        /// <param name="unitBase">要加入的单位</param>
        public void AddUnit(UnitBase unitBase)
        {
            Unit aUnit = Unit.FromUnitBase(unitBase, _game.NeutralPlayer);
            _selectedUnits.Add(aUnit);
        }

        /// <summary>
        /// 刷新单位池。将清除原先单位池中的单位，并以随机生成的新单位填满单位池。
        /// </summary>
        public void RefreshPool()
        {
            _selectedUnits.Clear();
            Fill();
        }

        #region 静态部分

        public static List<UnitBase> AllUnitBase = new List<UnitBase>();

        private static void SetAllUnitList()
        {
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
