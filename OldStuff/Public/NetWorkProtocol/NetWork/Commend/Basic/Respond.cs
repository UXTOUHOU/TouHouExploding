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
    public class Respond : Community
    {
        [DataMember]
        public override string NetID
        {
            get
            {
                return "00003";
            }
        }

        /// <summary>
        /// 该指令回复的ID
        /// </summary>
        public string RespondID
        {
            get
            {
                return respondID;
            }
        }
        [DataMember]
        private string respondID;

        /// <summary>
        /// 一般为null，如果遇到错误为错误对象
        /// </summary>
        [DataMember]
        public Error Error = null;

        /// <summary>
        /// 检测是否有错误
        /// </summary>
        public bool IsError
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

        /// <summary>
        /// 设置某回复为正确
        /// </summary>
        public void Correct()
        {
            Error = null;
        }
        /// <summary>
        /// 设置错误，返回格式是否正确，不正确不加载（必须为Error类本身，不能是其子类）
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool SetError(Error e)
        {
            if (e.GetType().ToString() != typeof(Error).ToString())
            {
                return false;
            }
            Error = e;
            return true;
        }



        /// <summary>
        /// 从ID创建回复对象
        /// </summary>
        /// <param name="toRespond"></param>
        public Respond(Enquire toRespond)
        {
            netAttribute = Community.NetAttributes.Respond;
            respondID = toRespond.CommunityID;
        }
        /// <summary>
        /// 从ID创建回复对象
        /// </summary>
        /// <param name="toRespondID"></param>
        public Respond(string toRespondID)
        {
            netAttribute = Community.NetAttributes.Respond;
            respondID = toRespondID;
        }
        /// <summary>
        /// 从对象创建回复
        /// </summary>
        /// <param name="toRespond"></param>
        /// <returns></returns>
        public static Respond GetRespond(Enquire toRespond)
        {
            return new Respond(toRespond.CommunityID);
        }
    }
}
