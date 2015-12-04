using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public class Chessboard
    {
        public Game GameCore;
        /// <summary>
        /// 第一个索引值为列(8)，第二个为行(12)
        /// </summary>
        public ChessboardCell[,] CellList;
        /// <summary>
        /// 自动读取一张地图，并将区块注册至区域表
        /// </summary>
        /// <param name="core"></param>
        /// <param name="map"></param>
        public Chessboard(Game core, ChessboardCell[,] map)
        {
            GameCore = core;
            CellList = map;
            for (int x = 0; x < map.GetLength(0); x++)//注册区块
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    GameCore.IDP.RID.SetID(CellList[x, y]);
                }
            }
        }
        /// <summary>
        /// 自动生成一张地图 第一个参数为其所在核心，会自动分配地区ID并将区块注册至区域表
        /// </summary>
        /// <param name="core"></param>
        public Chessboard(Game core)//
        {
            GameCore = core;
            CellList = MakeMap();
            for (int x = 0; x < CellList.GetLength(0); x++)//注册区块
            {
                for (int y = 0; y < CellList.GetLength(1); y++)
                {
                    GameCore.IDP.RID.SetID(CellList[x, y]);
                }
            }
        }
        public ChessboardCell GetRegion(int[] locate)
        {
            return CellList[locate[0], locate[1]];
        }
        /// <summary>
        /// 直接生成地图不注册区域
        /// </summary>
        /// <param name="mapType"></param>
        /// <returns></returns>
        private ChessboardCell[,] MakeMap(MapType mapType = MapType.Common)
        {
            var idList = new IDList("");
            var temp = new ChessboardCell[8, 12];
            for (int x = 0; x < temp.GetLength(0); x++)//初始化区块
            {
                for (int y = 0; y < temp.GetLength(1); y++)
                {
                    Position a = new Position(x, y);
                    temp[x, y] = new ChessboardCell(idList, a);
                }
            }
            for (int x = 0; x < temp.GetLength(0); x++)//设定召唤区块
            {
                for (int y = 0; y < 2; y++)
                {
                    temp[x, y].CellType = ChessboardCellType.Birth;
                    temp[x, y].Owner = GameCore.Teams[0];
                }
                for (int y = temp.GetLength(1) - 2; y < temp.GetLength(1); y++)
                {
                    temp[x, y].CellType = ChessboardCellType.Birth;
                    temp[x, y].Owner = GameCore.Teams[1];
                }
            }
            if (temp.GetLength(0) % 2 == 0) //设定基地，自动基地判断大小
            {
                for (int x = temp.GetLength(0) / 2 - 1; x <= temp.GetLength(0) / 2; x++) //偶数
                {
                    for (int y = 0; y <= 0; y++)
                    {
                        temp[x, y].CellType = ChessboardCellType.Base;
                    }
                    for (int y = temp.GetLength(1) - 1; y <= temp.GetLength(1) - 1; y++)
                    {
                        temp[x, y].CellType = ChessboardCellType.Base;
                    }
                }
            }
            else
            {
                for (int x = temp.GetLength(0) / 2 - 1; x <= temp.GetLength(0) / 2 + 1; x++) //奇数
                {
                    for (int y = 0; y <= 0; y++)
                    {
                        temp[x, y].CellType = ChessboardCellType.Base;
                    }
                    for (int y = temp.GetLength(1) - 1; y <= temp.GetLength(1) - 1; y++)
                    {
                        temp[x, y].CellType = ChessboardCellType.Base;
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
                /// <summary>
                /// ID
                /// </summary>
                public int id { get; set; }
                /// <summary>
                /// 坐标
                /// </summary>
                public int[] locate { get; set; }
                /// <summary>
                /// 地区特别属性
                /// </summary>
                public ChessboardCellType CellTypeHere { get; set; }
                /// <summary>
                /// 归属
                /// </summary>
                public int ownerID { get; set; }
                /// <summary>
                /// 地形
                /// </summary>
                public ChessboardCellTerrainType.Type terrainHere { get; set; }
                /// <summary>
                /// 地区状态
                /// </summary>
                public int stateHereID { get; set; }
                /// <summary>
                /// 在此位置的单位
                /// </summary>
                public int unitHereID { get; set; }
            }
            public Chessboard ToMap(Game core)//未完成
            {
                return new Chessboard(core);
            }
        }
    }
}
