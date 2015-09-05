using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace NetWork
{
    [DataContract]
    public sealed class Room
    {
        [DataMember]
        public int ID;
        [DataMember]
        public string Name;
        public bool NeedPwd//获得房间是否需要密码
        {
            get
            {
                if (Pwd == null || Pwd == "")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        [DataMember]
        public string Pwd = null;//房间进入密码设为null或""视为无密码
        [DataMember]
        public Mods Mod = Mods.Common;
        [DataMember]
        public List<Player> Players;
        public enum Mods
        {
            Common
        }
    }
}
