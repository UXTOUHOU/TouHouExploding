using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace NetWork
{
    [DataContract]
    public class Login : Enquire
    {
        [DataMember]
        public override string NetID
        {
            get
            {
                return "11002";
            }
        }
        [DataMember]
        public string User;
        [DataMember]
        public string Password;
        public Login(string user, string password)
        {
            User = user;
            Password = password;
        }
    }
    public class LoginR : Respond
    {
        [DataMember]
        public override string NetID
        {
            get
            {
                return "11002R";//R代表回复
            }
        }
        [DataMember]
        public IUser UserInfo;
        public LoginR(GetVersion toRespond,IUser userInfo)//返回成功 生成类的方法
            : base(toRespond)
        {
            UserInfo = userInfo;
        }
        public LoginR(GetVersion toRespond, Error error)//返回失败 生成类的方法
            : base(toRespond)
        {
            Error = error;
        }
    }

}

