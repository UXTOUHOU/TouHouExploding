using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace NetWork
{
    [DataContract]
    public interface IUser
    {
        [DataMember]
        public string User;
        [DataMember]
        public string Password;
        [DataMember]
        public string NickName;
    }
}
