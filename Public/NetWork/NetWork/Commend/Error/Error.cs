using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace NetWork
{
    [DataContract]
    public abstract class Error
    {
        [DataMember]
        public int EID;//错误ID
        [DataMember]
        public string ErrorLab;//错误标签
        [DataMember]
        public string ErrorMsg;//错误提示（给玩家显示的部分）
        [DataMember]
        public readonly string ErrorType;//错误类型（自动生成）
        public Error()
        {
            ErrorType = this.GetType().ToString();
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
