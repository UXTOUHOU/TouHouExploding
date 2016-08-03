using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace NetWork
{
    /// <summary>
    /// 询问。所有询问都需要继承于此类
    /// </summary>
    [DataContract]
    public class Enquire : Community
    {
        [DataMember]
        public override string NetID
        {
            get
            {
                return "00002";
            }
        }


        /// <summary>
        /// 如果收到Respond执行委托中的方法。注：委托使用.NET设计规范
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void RespondedEventHandler(object sender, RespondedEventArgs e);
        /// <summary>
        /// Acked事件，收到Ack后需要调用什么方法别客气往里面放
        /// </summary>
        public event RespondedEventHandler Responded;
        /// <summary>
        /// 这里是方法会感兴趣的数值——Respond！回复内容！
        /// </summary>
        public class RespondedEventArgs : EventArgs
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
