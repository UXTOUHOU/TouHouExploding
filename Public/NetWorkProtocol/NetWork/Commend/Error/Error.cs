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
        /// <summary>
        /// 错误ID
        /// </summary>
        [DataMember]
        public int EID;
        /// <summary>
        /// 错误标签
        /// </summary>
        [DataMember]
        public string ErrorLab;
        /// <summary>
        /// 错误提示（给玩家显示的部分）
        /// </summary>
        [DataMember]
        public string ErrorMsg;
        /// <summary>
        /// 针对本次错误信息的提示（可无）
        /// </summary>
        [DataMember]
        public string ExtraInfo;

        /// <summary>
        /// 错误类型（自动生成）
        /// </summary>
        public string ErrorType
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
        /// <summary>
        /// Error在最后加入时一顶要用此方法的返回值！
        /// </summary>
        /// <returns></returns>
        public Error GetError()
        {
            int eid = EID;
            string eMsg = ErrorMsg;
            string eInfo = ExtraInfo;
            string eLab = ErrorLab;
            return new Error() { errorType = typeof(Error).ToString(), EID = eid, ErrorMsg = eMsg, ExtraInfo = eInfo, ErrorLab = eLab };
        }
        /// <summary>
        /// 返回无错误
        /// </summary>
        /// <returns></returns>
        public static NoneError Correct()
        {
            return new NoneError();
        }
        /// <summary>
        /// 返回自定义错误
        /// </summary>
        /// <param name="eMsg"></param>
        /// <returns></returns>
        public static CustomError CustomError(string eMsg)
        {
            return new CustomError(eMsg);
        }
    }

}
