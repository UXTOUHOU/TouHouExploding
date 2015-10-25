using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace NetWork
{
    /// <summary>
    /// 一般不用
    /// </summary>
    [DataContract]
    public class NoneError : Error
    {
        public NoneError()
        {
            EID = 0;
            ErrorLab = null;
            ErrorMsg = null;
        }
    }
    /// <summary>
    /// 未归类错误
    /// </summary>
    [DataContract]
    public class UnclassedError : Error
    {
        public UnclassedError()
        {
            EID = 100000;
            ErrorLab = "未归类错误";
            ErrorMsg = "抱歉啦QAQ...出现了某些错误。";
        }
    }
    /// <summary>
    /// 自定义错误
    /// </summary>
    [DataContract]
    public class CustomError : Error
    {
        public CustomError(string eMsg)
        {
            EID = 100001;
            ErrorLab = "错误";
            ErrorMsg = eMsg;
        }
    }
    /// <summary>
    /// 登陆错误的基类
    /// </summary>
    [DataContract]
    public class LoginError : Error
    {
        public LoginError()
        {
            EID = 200000;
            ErrorLab = "登陆错误";
            ErrorMsg = "啊啊啊啊救命啊！登陆出现问题啦！！！";
        }
    }
    /// <summary>
    /// 用户名错误
    /// </summary>
    [DataContract]
    public class UserError : LoginError
    {
        public UserError()
        {
            EID = 200001;
            ErrorMsg = "嘟嘟噜~少年瞪大眼睛看看用户名是不是错啦~(⊙v⊙)";
        }
    }

    /// <summary>
    /// 密码错误
    /// </summary>
    [DataContract]
    public class PwdError : LoginError
    {
        public PwdError()
        {
            EID = 200002;
            ErrorMsg = "咳咳……密码不对哦，不许进！";
        }
    }
    /// <summary>
    /// 封号
    /// </summary>
    [DataContract]
    public class BannedError : LoginError
    {
        public BannedError()
        {
            EID = 200003;
            ErrorMsg = "你·个·大·坏·蛋！被~封~号~了~吧~活！该！哈哈哈哈哈哈";
        }
    }
    /// <summary>
    /// 拒绝登陆
    /// </summary>
    [DataContract]
    public class RefusedError : LoginError
    {
        public RefusedError()
        {
            EID = 200004;
            ErrorMsg = "服务器姐姐sama心情不爽，先别打扰她了……每个月总会有那么几天的嘛求体谅【喝茶";
        }
    }
}
