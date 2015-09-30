using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouHou_Exploding
{
    public class Region : IDProvider.IID
    {
        public int id { get; set; }//ID
        public int[] locate { get; set; }//坐标
        public Special specialHere { get; set; }//地区特别属性
        public Team owner { get; set; }//归属
        public Terrain terrainHere { get; set; }//地形
        public Statue stateHere { get; set; }//地区状态
        public Unit unitHere { get; set; }//在此位置的单位
        public Region(IDProvider.IDList idList, int[] _locate)//第一参数为ID提供者，第二参数为坐标
        {
            idList.ApplyID(this);
            locate = _locate;
            specialHere = Special.Common;
        }
        public bool MoveHere(Unit unit)//移动到此格。成功返回真，失败返回假
        {
            if (unitHere != null) return false;
            if (unit.at != null)
            {
                unit.at.unitHere = null;
            }
            unitHere = unit;
            unit.at = this;
            return true;
        }
        public enum Special { Common, Base, Birth, Custom }//一般/基地/召唤地/自定义

        public class Statue
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


