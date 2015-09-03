using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace NetWork
{
    public class Enquire : Community//询问。所有询问都需要继承于此类
    {
        public Enquire()
        {
            netAttribute = Community.NetAttributes.Enquire;
        }
    }
}
