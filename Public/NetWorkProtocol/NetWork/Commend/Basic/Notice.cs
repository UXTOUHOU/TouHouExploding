using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace NetWork
{
    /// <summary>
    /// 通知，如无要求（Ack）无须回复。所有通知都需要继承于此类
    /// </summary>
    [DataContract]
    public class Notice : Community
    {
        [DataMember]
        public override string NetID
        {
            get
            {
                return "00001";
            }
        }

        public Notice()
        {
            netAttribute = Community.NetAttributes.Notice;
        }
    }
}
