using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace NetWork
{
    public sealed class UserInformation
    {
        [DataMember]
        public string UserID { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string NickName { get; set; }
    }
}
