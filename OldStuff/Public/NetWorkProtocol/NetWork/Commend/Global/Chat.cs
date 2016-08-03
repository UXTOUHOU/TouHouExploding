using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace NetWork
{
    /// <summary>
    /// 客户端向服务端发送聊天内容,暂未启用
    /// </summary>
    [DataContract]
    public class SendChat : Notice
    {
        [DataMember]
        public override string NetID
        {
            get
            {
                return "01002";
            }
        }

        /// <summary>
        /// 聊天的属性，公告/世界/服务器/房间/团队/私人
        /// </summary>
        [DataMember]
        public string Attributes;
        /// <summary>
        /// 聊天的内容
        /// </summary>
        [DataMember]
        public string Content;//
    }
    /// <summary>
    /// 客户端接受聊天内容，暂未启用
    /// </summary>
    [DataContract]
    public class ChatCome : Notice
    {
        [DataMember]
        public override string NetID
        {
            get
            {
                return "02002";
            }
        }
        /// <summary>
        /// 发送者
        /// </summary>
        [DataMember]
        public string From;
        /// <summary>
        /// 聊天的属性，公告/世界/服务器/房间/团队/私人
        /// </summary>
        [DataMember]
        public string Attributes;
        /// <summary>
        /// 聊天的内容
        /// </summary>
        [DataMember]
        public string Content;
    }
}
