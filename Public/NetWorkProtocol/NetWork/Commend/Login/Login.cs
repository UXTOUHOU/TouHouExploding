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
    [DataContract]
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
        public UserInformation UserInfo;
        /// <summary>
        /// 返回成功 生成类的方法
        /// </summary>
        /// <param name="toRespond"></param>
        /// <param name="userInfo"></param>
        public LoginR(Login toRespond, UserInformation userInfo)
            : base(toRespond)
        {
            UserInfo = userInfo;
        }
        /// <summary>
        /// 返回失败 生成类的方法
        /// </summary>
        /// <param name="toRespond"></param>
        /// <param name="error"></param>
        public LoginR(Login toRespond, Error error)
            : base(toRespond)
        {
            Error = error;
        }
    }

}

