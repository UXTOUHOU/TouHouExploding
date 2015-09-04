using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace NetWork
{
    [DataContract]
    public class Notice : Community//通知，如无要求（Ack）无须回复。所有通知都需要继承于此类
    {
        public Notice()
        {
            netAttribute = Community.NetAttributes.Notice;
        }
    }
}
