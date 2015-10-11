using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public interface NeedRespond
    {
        ThingsToAsk thingsToAsk { get; set; }
        bool AddToNeedRespondLIst();
        bool InputRespond(ClientRespond clientRespond);
        
    }
    public class ThingsToAsk
    {

    }
    public class ClientRespond
    {

    }
}
