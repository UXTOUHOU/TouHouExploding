﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public class Team : IID
    {
        public Game GameCore;
        public int Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 基地血量
        /// </summary>
        public int blood { get; set; }
        public List<Player> playerList { get; set; }
        public List<ChessboardCell> OwnRegion { get; set; }
        public List<ChessboardCell> Base
        {
            get
            {
                List<ChessboardCell> R = new List<ChessboardCell>();
                foreach(ChessboardCell r in OwnRegion)
                {
                    if (r.CellType == ChessboardCell.ChessboardCellType.Base) R.Add(r);
                }
                return R;
            }
        }

        public Team(Game core)
        {
            GameCore = core;
            GameCore.IDP.TID.ApplyID(this);
            playerList = new List<Player>();
            OwnRegion = new List<ChessboardCell>();
            blood = 4;
        }
        public void Add(Player player)
        {
            if (player.atTeam != null) player.atTeam.Leave(player);
            playerList.Add(player);
            player.atTeam = this;
        }
        public void BeAttacked()
        {
            blood--;
            GameCore.CheckWin();
        }
        /// <summary>
        /// 检查该队伍是不是全员失败
        /// </summary>
        /// <returns></returns>
        public bool IsFailed()
        {
            if (blood <= 0) return true;
            foreach (Player p in playerList)
            {
                if(p.HaveFailed() == false)
                    return false;
            }
            return true;
        }
        public void Leave(Player player)
        {
            player.atTeam=null;
            this.playerList.Remove(player);
        }
    }
}
