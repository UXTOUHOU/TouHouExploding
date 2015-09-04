using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace NetWork
{
    [DataContract]
    public class Respond : Community//通知，无须回复。所有通知都需要继承于此类
    {
        public string RespondID//该指令回复的ID
        {
            get
            {
                return respondID;
            }
        }
        [DataMember]
        private string respondID;
        public Respond(string toRespondID)//从ID创建回复对象
        {
            netAttribute = Community.NetAttributes.Respond;
            respondID = toRespondID;
        }
        public static Respond GetRespond(Enquire toRespond)//从对象创建回复
        {
            return new Respond(toRespond.CommunityID);
        }
    }
}
