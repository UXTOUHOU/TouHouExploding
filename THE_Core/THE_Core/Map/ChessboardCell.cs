using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public class ChessboardCell : IID
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 坐标
        /// </summary>
        public Position Location { get; set; }
        /// <summary>
        /// 地区特别属性
        /// </summary>
        public ChessboardCellType CellType { get; set; }
        /// <summary>
        /// 归属
        /// </summary>
        public Team Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
                value.OwnRegion.Add(this);
            }
        }
        //GameCore.RoomTeam[1].OwnRegion.Add(temp[x, y]);
        private Team _owner;
        /// <summary>
        /// 地形
        /// </summary>
        public ChessboardCellTerrainType TerrainType { get; set; }

        /// <summary>
        /// 地区状态
        /// </summary>
        public ChessboardCellState CellState { get; set; }

        /// <summary>
        /// 在此位置的单位
        /// </summary>
        public Unit Unit { get; set; }
        public bool HasUnit
        {
            get
            {
                return (this.Unit != null);
            }
        }


        /// <summary>
        /// 第一参数为ID提供者，第二参数为坐标
        /// </summary>
        /// <param name="idList"></param>
        /// <param name="location"></param>
        public ChessboardCell(IDList idList, Position location)
        {
            idList.ApplyID(this);
            Location = location;
            CellType = ChessboardCellType.Common;
        }

        /// <summary>
        /// 放置单位到此格。
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>成功返回真，失败返回假</returns>
        public bool PlaceUnit(Unit unit)
        {
            if (Unit != null) return false;
            if (unit.Position != null)
            {
                unit.Position.Unit = null;
            }
            Unit = unit;
            unit.Position = this;
            return true;
        }
    }

    /// <summary>
    /// 一般/基地/召唤地/自定义
    /// </summary>
    public enum ChessboardCellType
    {
        Common, Base, Birth, Custom 
    }

    /// <summary>
    /// 
    /// </summary>
    public class ChessboardCellState
    {
        public string name;

        public enum Type
        {
            None,
            Burn,
            Frozen,
            Flood,
            Poison,
            Mist,
            Custom 
        }
    }

    public class ChessboardCellTerrainType
    {
        public string name;
        public enum Type { Plain, Hill, River, Sea, Custom }

    }
}


