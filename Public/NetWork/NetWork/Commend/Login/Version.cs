using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace NetWork
{
    [DataContract]
    public class GetVersion : Enquire 
    {
        [DataMember]
        public override string NetID
        {
            get
            {
                return "11001";
            }
        }
        [DataMember]
        public string MyVersion;//这里填客户端问询的版本号
        public GetVersion(string myVersion)
        {
            MyVersion = myVersion;
        }
    }
    public class GetVersionR : Respond 
    {
        [DataMember]
        public override string NetID
        {
            get
            {
                return "11001R";//R代表回复
            }
        }
        [DataMember]
        public string EnquireVersion;//这里填询问的版本号
        [DataMember]
        public bool CanUse;//这里填所询问的版本号是否还能使用
        [DataMember]
        public string NowVersion;//这里是当前最新的版本号
        public GetVersionR(GetVersion toRespond, bool canUse, string nowVersion=null)
            : base(toRespond)
        {
            CanUse = canUse;
            NowVersion = nowVersion;
            EnquireVersion = toRespond.MyVersion;
        }
    }
   
}

