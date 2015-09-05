using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace NetWork
{
    [DataContract]
    public class KeepContact : Notice
    {
        [DataMember]
        public override string NetID
        {
            get
            {
                return "01001";
            }
        }
    }
    [DataContract]
    public class StartKeepContacted : Notice
    {
        [DataMember]
        public override string NetID
        {
            get
            {
                return "02001";
            }
        }
        [DataMember]
        public bool IsKeepContacted;
        [DataMember]
        public int IntervalTime = 15;//客户端发送01001的间隔时间，单位秒，默认15，如果IskeepContacted为假，该值无效
        public StartKeepContacted(bool isKeepContacted)//输入是否持续保持联系
        {
            IsKeepContacted = isKeepContacted;
        }
    }
    [DataContract]
    public class IsContacted : Notice
    {
        [DataMember]
        public override string NetID
        {
            get
            {
                return "02003";
            }
        }
        public IsContacted()
        {
            NeedAck = true;
        }
    }
}
