using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouHou_Exploding
{
    class Map
    {
        private Region[] RegionList;
        public class Region
        {
            public int ID { get; set; }
            public int[] Locate { get; set; }
            public Special SpecialHere { get; set; }
            public Team Owner { get; set; }
            public Terrain TerrainHere { get; set; }
            public Statue StateHere { get; set; }
            public Unit UnitHere { get; set; }
            public Region(int id)
            {

            }
            public enum Special { Common,Base,Birth,Custom }//一般/基地/召唤地/自定义
        }
        class Terrain
        {
            public string name;
            enum TerrainTpye { Plain,Hill,River,Sea,Custom }

        }
        class Statue
        {
            public string name;
            enum StateTpye { None,Custom }
        }
    }
}
