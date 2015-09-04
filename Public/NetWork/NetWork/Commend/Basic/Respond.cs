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
        [DataMember]
        public override string NetID
        {
            get
            {
                return "00003";
            }
        }

        public string RespondID//该指令回复的ID
        {
            get
            {
                return respondID;
            }
        }
        [DataMember]
        private string respondID;

        [DataMember]
        public Error Error = null;//一般为null，如果遇到错误为错误对象
        
        public bool IsError//检测是否有错误
        {
            get
            {
                if (Error == null || Error.EID == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public void Correct()//设置某回复为正确
        {
            Error = null;
        }
        public void SetError(Error e)//设置错误
        {
            Error = e;
        }
        


        public Respond(Enquire toRespond)//从ID创建回复对象
        {
            netAttribute = Community.NetAttributes.Respond;
            respondID = toRespond.CommunityID;
        }
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
