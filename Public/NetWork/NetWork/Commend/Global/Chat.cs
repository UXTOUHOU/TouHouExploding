using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace NetWork
{
    [DataContract]
    public class SendChat : Notice//客户端向服务端发送聊天内容,暂未启用
    {
        [DataMember]
        public override string NetID
        {
            get
            {
                return "01002";
            }
        }

        [DataMember]
        public string Attributes;//聊天的属性，公告/世界/服务器/房间/团队/私人
        [DataMember]
        public string Content;//聊天的内容
    }
    [DataContract]
    public class ChatCome : Notice//客户端接受聊天内容，暂未启用
    {
        [DataMember]
        public override string NetID
        {
            get
            {
                return "02002";
            }
        }
        [DataMember]
        public string From;//发送者
        [DataMember]
        public string Attributes;//聊天的属性，公告/世界/服务器/房间/团队/私人
        [DataMember]
        public string Content;//聊天的内容
    }
}
