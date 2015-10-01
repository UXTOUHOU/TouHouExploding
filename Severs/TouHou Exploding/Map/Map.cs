using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouHou_Exploding
{
    public class Map
    {
        public Core GameCore;
        public Region[,] RegionList;//第一个索引值为列(8)，第二个为行(12)
        public Map(Core core, Region[,] map)//自动读取一张地图，并将区块注册至区域表
        {
            GameCore = core;
            RegionList = map;
            for (int x = 0; x < map.GetLength(0); x++)//注册区块
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    GameCore.IDP.RID.SetID(RegionList[x, y]);
                }
            }
        }
        public Map(Core core)//自动生成一张地图 第一个参数为其所在核心，会自动分配地区ID并将区块注册至区域表
        {
            GameCore = core;
            RegionList = MakeMap();
            for (int x = 0; x < RegionList.GetLength(0); x++)//注册区块
            {
                for (int y = 0; y < RegionList.GetLength(1); y++)
                {
                    GameCore.IDP.RID.SetID(RegionList[x, y]);
                }
            }
        }
        public Region GetRegion(int[] locate)
        {
            return RegionList[locate[0], locate[1]];
        }
        private Region[,] MakeMap(MapType mapType = MapType.Common)//直接生成地图不注册区域
        {
            var idList = new IDProvider.IDList("");
            var temp = new Region[8, 12];
            for (int x = 0; x < temp.GetLength(0); x++)//初始化区块
            {
                for (int y = 0; y < temp.GetLength(1); y++)
                {
                    int[] a = new int[2] { x, y };
                    temp[x, y] = new Region(idList, a);
                }
            }
            for (int x = 0; x < temp.GetLength(0); x++)//设定召唤区块
            {
                for (int y = 0; y < 2; y++)
                {
                    temp[x, y].specialHere = Region.Special.Birth;
                    temp[x, y].owner = GameCore.RoomTeam[0];
                }
                for (int y = temp.GetLength(1) - 2; y < temp.GetLength(1); y++)
                {
                    temp[x, y].specialHere = Region.Special.Birth;
                    temp[x, y].owner = GameCore.RoomTeam[1];
                }
            }
            if (temp.GetLength(0) % 2 == 0) //设定基地，自动基地判断大小
            {
                for (int x = temp.GetLength(0) / 2 - 1; x <= temp.GetLength(0) / 2; x++) //偶数
                {
                    for (int y = 0; y <= 0; y++)
                    {
                        temp[x, y].specialHere = Region.Special.Base;
                    }
                    for (int y = temp.GetLength(1) - 1; y <= temp.GetLength(1) - 1; y++)
                    {
                        temp[x, y].specialHere = Region.Special.Base;
                    }
                }
            }
            else
            {
                for (int x = temp.GetLength(0) / 2 - 1; x <= temp.GetLength(0) / 2 + 1; x++) //奇数
                {
                    for (int y = 0; y <= 0; y++)
                    {
                        temp[x, y].specialHere = Region.Special.Base;
                    }
                    for (int y = temp.GetLength(1) - 1; y <= temp.GetLength(1) - 1; y++)
                    {
                        temp[x, y].specialHere = Region.Special.Base;
                    }
                }
            }

            return temp;
        }
        public enum MapType { Common }
        public class MapSave
        {
            public RegionSave[,] map { get; set; }
            public class RegionSave
            {
                public int id { get; set; }//ID
                public int[] locate { get; set; }//坐标
                public Region.Special specialHere { get; set; }//地区特别属性
                public int ownerID { get; set; }//归属
                public Terrain.Type terrainHere { get; set; }//地形
                public int stateHereID { get; set; }//地区状态
                public int unitHereID { get; set; }//在此位置的单位
            }
            public Map ToMap(Core core)//未完成
            {
                return new Map(core);
            }
        }
    }
}
