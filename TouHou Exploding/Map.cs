using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouHou_Exploding
{
    public class Map
    {
        private Core _core;
        private Region[,] _regionList;//第一个索引值为列(8)，第二个为行(12)
        public Map(Core core)//第一个参数为其所在核心
        {
            _core = core;
            _regionList = new Region[8,12];
            for (int x = 1; x <= 8; x++)//初始化区块
            {
                for (int y = 1; y <= 12; y++)
                {
                    int[] a=new int[2]{x,y};
                    _regionList[x, y] = new Region(_core.idProvider, a);
                }
            }
            for (int x = 1; x <= 8; x++)//设定召唤区块
            {
                for (int y = 1; y <= 2; y++)
                {
                    _regionList[x, y].specialHere = Region.Special.Birth;
                    _regionList[x, y].owner = _core.team[0];
                }
                for (int y = 11; y <= 12; y++)
                {
                    _regionList[x, y].specialHere = Region.Special.Birth;
                }
            }
            for (int x = 1; x <= 8; x++)//设定基地
            {
                for (int y = 1; y <= 2; y++)
                {
                    _regionList[x, y].specialHere = Region.Special.Birth;
                    _regionList[x, y].owner = _core.team[0];
                }
                for (int y = 11; y <= 12; y++)
                {
                    _regionList[x, y].specialHere = Region.Special.Birth;
                }
            }
        }

        public class Region:IDProvider.IID
        {
            public int id { get; set; }
            public int[] locate { get; set; }
            public Special specialHere { get; set; }
            public Team owner { get; set; }
            public Terrain terrainHere { get; set; }
            public Statue stateHere { get; set; }
            public Unit unitHere { get; set; }
            public Region(IDProvider idProvider,int[] _locate)//第一参数为ID提供者，第二参数为坐标
            {
                idProvider.RID.ApplyID(this);
                locate = _locate;
                specialHere = Special.Common;
            }
            
            public enum Special { Common,Base,Birth,Custom }//一般/基地/召唤地/自定义
        }
        public class Terrain
        {
            public string name;
            enum TerrainTpye { Plain,Hill,River,Sea,Custom }

        }
        public class Statue
        {
            public string name;
            enum StateTpye { None,Custom }
        }
    }
}
