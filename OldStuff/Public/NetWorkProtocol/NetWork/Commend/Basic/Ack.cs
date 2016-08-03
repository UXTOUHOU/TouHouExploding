using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace NetWork
{
    /// <summary>
    /// 通知，无须回复。所有通知都需要继承于此类
    /// </summary>
    [DataContract]
    public sealed class Ack : Community
    {
        [DataMember]
        public override string NetID
        {
            get
            {
                return "00004";
            }
        }
        public override string NetContent
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// 该指令回复的ID
        /// </summary>
        public string AckID
        {
            get
            {
                return ackID;
            }
        }
        [DataMember]
        private string ackID;
        /// <summary>
        /// 从ID创建回复对象
        /// </summary>
        /// <param name="toAckID"></param>
        public Ack(string toAckID)
        {

            netAttribute = Community.NetAttributes.Respond;
            ackID = toAckID;
        }
        /// <summary>
        /// 从ID创建回复对象
        /// </summary>
        /// <param name="toAck"></param>
        public Ack(Community toAck)//
        {
            netAttribute = Community.NetAttributes.Respond;
            ackID = toAck.CommunityID;
        }

        /// <summary>
        /// 从对象创建回复
        /// </summary>
        /// <param name="toAck"></param>
        /// <returns></returns>
        public static Ack GetAck(Community toAck)
        {
             return new Ack(toAck.CommunityID);
        }
    }
}
