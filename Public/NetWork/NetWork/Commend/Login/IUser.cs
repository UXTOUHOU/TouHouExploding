using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace NetWork
{
    public interface IUser
    {
        [DataMember]
        string User { get; set; }
        [DataMember]
        string Password { get; set; }
        [DataMember]
        string NickName { get; set; }
    }
}
