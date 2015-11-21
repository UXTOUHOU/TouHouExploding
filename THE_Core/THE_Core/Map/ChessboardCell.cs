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
        public Position locate { get; set; }
        /// <summary>
        /// 地区特别属性
        /// </summary>
        public Special specialHere { get; set; }
        /// <summary>
        /// 归属
        /// </summary>
        public Team owner
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
        public Terrain terrainHere { get; set; }
        /// <summary>
        /// 地区状态
        /// </summary>
        public State stateHere { get; set; }
        /// <summary>
        /// 在此位置的单位
        /// </summary>
        public Unit unitHere { get; set; }
        /// <summary>
        /// 第一参数为ID提供者，第二参数为坐标
        /// </summary>
        /// <param name="idList"></param>
        /// <param name="_locate"></param>
        public ChessboardCell(IDList idList, Position _locate)
        {
            idList.ApplyID(this);
            locate = _locate;
            specialHere = Special.Common;
        }
        /// <summary>
        /// 移动到此格。
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>成功返回真，失败返回假</returns>
        public bool MoveHere(Unit unit)
        {
            if (unitHere != null) return false;
            if (unit.Position != null)
            {
                unit.Position.unitHere = null;
            }
            unitHere = unit;
            unit.Position = this;
            return true;
        }
        /// <summary>
        /// 一般/基地/召唤地/自定义
        /// </summary>
        public enum Special { Common, Base, Birth, Custom }

        public class State
        {
            public string name;
            public enum Type { None, Custom }
        }
    }
    public class Terrain
    {
        public string name;
        public enum Type { Plain, Hill, River, Sea, Custom }

    }
}


