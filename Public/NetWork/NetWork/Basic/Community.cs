using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace NetWork
{
    [DataContract]
    public abstract class Community//一切被发送的json都要继承自Community基类
    {
        public delegate void AckedEventHandler(object sender,AckedEventArgs e);//如果收到Ack执行委托中的方法，请注意，如果NeedAck为false这一部分无效。注：委托使用.NET设计规范
        public event AckedEventHandler Acked;//Acked事件，收到Ack后需要调用什么方法别客气往里面放
        public class AckedEventArgs : EventArgs//按说这里应该放上面方法会感兴趣的数值，然而我并没有想好有什么可感兴趣的，就先这样吧。
        {

        }
        //以上是Ack委托部分，Ack的判定标准是调用AckedArrive方法



        [DataMember]
        public string NetContent//字符串（命令）模式的内容。如果不是命令模式，读取无效
        {
            set
            {
                if (value == "" || value == null)
                {
                    netContent = null;
                    netMod = NetMods.Object;
                }
                else
                {
                    netContent = value;
                    netMod = NetMods.String;
                }
            }
            get
            {
                if (NetMod != NetMods.String) return null;
                return netContent;
            }
        }
        private string netContent;

        public bool NeedAck = false;//设定接受到是否需要立刻回复已收到的包

        public string CommunityID//只读
        {
            get
            {
                return communityID;
            }
        }
        [DataMember]
        private string communityID;//此通讯的ID，生成方式为时间+10位随机数

        [DataMember]
        private string className;//存储发送的类的名字，同时发送的也是它
        public string ClassName//发送的类的名字，只读
        {
            get
            {
                return className;
            }
        }

        public NetAttributes NetAttribute//只读
        {
            get
            {
                return netAttribute;
            }
        }
        [DataMember]
        protected NetAttributes netAttribute;//数据包的属性（仅限自己和子类访问）

        [DataMember]
        public NetMods NetMod//数据包的传送模式，会自动设置
        {
            get
            {
                return netMod;
            }
        }
        private NetMods netMod = NetMods.Object;




        public Community()
        {
            className = this.GetType().ToString();//自动设置类名
            communityID = getRamdomID();
        }


        public string ToJson()//把自己转换为Json
        {
            return JsonHelper.GetJson(this);
        }


        public void AckArrive(AckedEventArgs e)
        {
            if (Acked != null)
            {
                Acked(this, e);
            }
        }


        private string getRamdomID()
        {
            int temp = Guid.NewGuid().GetHashCode();
            if (temp < 0) temp = -temp;
            return DateTime.Now.ToString("ddhhmmssfff") + temp.ToString();
        }


        public enum NetAttributes 
        {
            Notice,//通知，如无要求无须回复
            Enquire,//询问，向对方索要数据
            Respond,//回答对方的询问
            Ack//回答查收
        }

        public enum NetMods
        {
            String,//命令模式，所有需传递的信息都包含在一个String类型的变量NetContent中。只有一个类型。
            Object//对象模式，所有需传递的信息包含在该对象中
        }
    }
}
