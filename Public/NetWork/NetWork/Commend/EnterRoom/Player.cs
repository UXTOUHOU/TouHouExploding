using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace NetWork
{
    [DataContract]
    public sealed class Player
    {
        [DataMember]
        public string ID;
        [DataMember]
        public string NickName;
        [DataMember]
        public bool IsReady = false;
    }
}
