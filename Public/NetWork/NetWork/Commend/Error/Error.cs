using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace NetWork
{
    [DataContract]
    public class Error
    {
        [DataMember]
        public int EID;//错误ID
        [DataMember]
        public string ErrorLab;//错误标签
        [DataMember]
        public string ErrorMsg;//错误提示（给玩家显示的部分）
        [DataMember]
        public string ExtraInfo;//针对本次错误信息的提示（可无）
        
        public string ErrorType//错误类型（自动生成）
        {
            get
            {
                return errorType;
            }
        }
        [DataMember]
        protected string errorType;
        public Error()
        {
            errorType = this.GetType().ToString();
        }
        public Error GetError()//Error在最后加入时一顶要用此方法的返回值！
        {
            int eid = EID;
            string eMsg = ErrorMsg;
            string eInfo = ExtraInfo;
            string eLab = ErrorLab;
            return new Error() { errorType = typeof(Error).ToString(), EID = eid, ErrorMsg = eMsg, ExtraInfo = eInfo, ErrorLab = eLab };
        }
        public static NoneError Correct()//返回无错误
        {
            return new NoneError();
        }
        public static CustomError CustomError(string eMsg)//返回自定义错误
        {
            return new CustomError(eMsg);
        }
    }

}
