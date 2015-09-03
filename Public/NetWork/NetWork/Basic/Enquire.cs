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
        public delegate void RespondedEventHandler(object sender, RespondedEventArgs e);//如果收到Respond执行委托中的方法。注：委托使用.NET设计规范
        public event RespondedEventHandler Responded;//Acked事件，收到Ack后需要调用什么方法别客气往里面放
        public class RespondedEventArgs : EventArgs//这里是方法会感兴趣的数值——Respond！回复内容！
        {
            public Respond Content;
            public RespondedEventArgs(Respond respond)
            {
                Content = respond;
            }
        }
        //以上是Responded委托部分，Ack的判定标准是调用RespondArrive方法

        public void RespondArrive(RespondedEventArgs e)
        {
            if (Responded != null)
            {
                Responded(this, e);
            }
        }

        public Enquire()
        {
            netAttribute = Community.NetAttributes.Enquire;
        }
    }
}
