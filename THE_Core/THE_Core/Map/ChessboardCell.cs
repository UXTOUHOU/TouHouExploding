using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public class ChessboardCell : IID
    {
        public int Id { get; set; }//ID
        public Position locate { get; set; }//坐标
        public Special specialHere { get; set; }//地区特别属性
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
            }//归属
        //GameCore.RoomTeam[1].OwnRegion.Add(temp[x, y]);
        private Team _owner;
        public Terrain terrainHere { get; set; }//地形
        public State stateHere { get; set; }//地区状态
        public Unit unitHere { get; set; }//在此位置的单位
        public ChessboardCell(IDList idList, Position _locate)//第一参数为ID提供者，第二参数为坐标
        {
            idList.ApplyID(this);
            locate = _locate;
            specialHere = Special.Common;
        }
        public bool MoveHere(Unit unit)//移动到此格。成功返回真，失败返回假
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
        public enum Special { Common, Base, Birth, Custom }//一般/基地/召唤地/自定义

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


