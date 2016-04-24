using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;

//大包发送协议：先发送
//【TCP大数据包准备传输LongDataTransportStarting】
//【TCP大数据包传输完毕LongDataTransportEnding】{0}←   数据包的哈希值
public abstract class NetCore
{

    /// <summary>
    /// 接受到某客户端/服务端消息执行事件的委托
    /// </summary>
    /// <param name="str">发送数据的内容</param>
    /// <param name="user">发送数据的客户端，如为空则为系统通知</param>
    public delegate void GetNoticeEventHandler(Object sender, GetNoticeEventArgs e);

    /// <summary>
    /// 接受到客户端/服务端消息执行的事件
    /// </summary>
    public event GetNoticeEventHandler getNoticeEvent;


    /// <summary>
    /// 是否使用异步方法调用委托，默认为false
    /// </summary>
    public bool useNoticeAsync { get; set; }

    /// <summary>
    /// 是否使用异步方法发送信息，默认为false
    /// </summary>
    public bool useSendAsync { get; set; }

    /// <summary>
    /// 该类是不是已经关闭，默认为false
    /// </summary>
    public bool isClosed { get; protected set; }
    public NetCore()
    {
        useNoticeAsync = false;
        useSendAsync = false;
        isClosed = false;
    }
    /// <summary>
    /// 通知所有观察者
    /// </summary>
    /// <param name="e">通知的事件</param>
    public virtual void Notice(GetNoticeEventArgs e)
    {
        Notice(this, e);
    }
    /// <summary>
    /// 通知所有观察者，注意，该种重载不会通知总监控
    /// </summary>
    /// <param name="sender">发送消息的类</param>
    /// <param name="e">通知的事件</param>
    public virtual void Notice(Object sender, GetNoticeEventArgs e)
    {
        if (getNoticeEvent == null) return;
        if (useNoticeAsync)//判断是否使用异步
        {
            getNoticeEvent.BeginInvoke(sender, e, null, null);
        }
        else
        {
            getNoticeEvent(sender, e);
        }
    }
    /// <summary>
    /// 用于发送信息
    /// </summary>
    /// <param name="msg">所发送的信息</param>
    /// <returns></returns>
    public abstract bool Send(string msg);
    /// <summary>
    /// 用于关闭释放资源
    /// </summary>
    /// <returns></returns>
    public virtual bool Close()
    {
        if (isClosed) return false;
        isClosed = true;
        return true;
    }

    public static IPAddress[] GetIPAddress(string hostname)
    {
        IPAddress ip;
        IPAddress[] ips;
        if (IPAddress.TryParse(hostname, out ip) == false)
            ips = Dns.GetHostAddresses(hostname);
        else
            ips = new IPAddress[1] { ip };
        return ips;
    }


    /// <summary>
    /// GetIP方法的简化输出版本
    /// </summary>
    /// <param name="hostname"></param>
    /// <returns></returns>
    public static string GetSimpleIP(string hostname)
    {
        return GetIPAddress(hostname)[0].ToString();
    }

}











public class GetNoticeEventArgs : EventArgs
{
    /// <summary>
    /// 发送该消息的服务端，只有服务端类别中有值
    /// </summary>
    public THEServer owner { get; private set; }

    /// <summary>
    /// 发送该消息的客户端，只有客户端类别中有值
    /// </summary>
    public THEClient clientOwner { get; private set; }

    /// <summary>
    /// 与该条提醒相关联的客户端，可能会未赋值
    /// </summary>
    public THEServer.User relatedUser { get; private set; }

    /// <summary>
    /// 与该条提醒相关联的客户端，可能会未赋值
    /// </summary>
    public List<THEServer.User> relatedUsers { get; private set; }

    /// <summary>
    /// 该消息所属类型。
    /// </summary>
    public Attribute attribute { get; private set; }

    /// <summary>
    /// 实际发送的信息。只有发送/接受信息通知才会有值
    /// </summary>
    public string commucateMessage { get; private set; }


    /// <summary>
    /// 该信息中较为友好的提示。 （可供直接向用户输出）
    /// </summary>
    public string message { get; private set; }

    /// <summary>
    /// 该条信息的内容（需要解释器处理）
    /// </summary>
    public string content { get; private set; }

    public GetNoticeEventArgs(THEServer owner, Attribute attribute)
    {
        this.owner = owner;
        this.attribute = attribute;
    }
    public GetNoticeEventArgs(THEClient owner, Attribute attribute)
    {
        this.clientOwner = owner;
        this.attribute = attribute;
    }
    private static GetNoticeEventArgs GetTHEServerSystem(THEServer owner)
    {
        GetNoticeEventArgs e = new GetNoticeEventArgs(owner, Attribute.THEServerSystem);
        return e;
    }









    //以下为服务器消息

    //以下为系统信息
    /// <summary>
    /// 获取开始失败的消息
    /// </summary>
    /// <param name="sender"></param>
    /// <returns></returns>
    public static GetNoticeEventArgs GetStartFailed(THEServer sender)
    {
        GetNoticeEventArgs e = GetTHEServerSystem(sender);
        e.content = "StartFailed";
        e.message = "抱歉的说QAQ服务端开启失败。是不是已经在运行了呢……？";
        return e;
    }
    /// <summary>
    /// 获取开始成功的消息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ipEndPoint">服务器监听的位置</param>
    /// <returns></returns>
    public static GetNoticeEventArgs GetStartSucceed(THEServer sender, IPEndPoint ipEndPoint)
    {
        GetNoticeEventArgs e = GetTHEServerSystem(sender);
        e.content = string.Format("StartSucceed,{0},{1}", ipEndPoint.Address.ToString(), ipEndPoint.Port);
        e.message = string.Format("嘟嘟噜~~服务端成功运行啦~~正在监听IP:{0}，端口:{1}", ipEndPoint.Address.ToString(), ipEndPoint.Port);
        return e;
    }
    /// <summary>
    /// 获取停止成功的消息
    /// </summary>
    /// <param name="sender"></param>
    /// <returns></returns>
    public static GetNoticeEventArgs GetStopSucceed(THEServer sender)
    {
        GetNoticeEventArgs e = GetTHEServerSystem(sender);
        e.content = "StopSucceed";
        e.message = "服务端成功关闭~放假啦~滚去休息啦~";
        return e;
    }
    /// <summary>
    /// 获取停止失败的消息
    /// </summary>
    /// <param name="sender"></param>
    /// <returns></returns>
    public static GetNoticeEventArgs GetStopFailed(THEServer sender)
    {
        GetNoticeEventArgs e = GetTHEServerSystem(sender);
        e.content = "StopFailded";
        e.message = "服务端停止失败。很可能是因为根本没开啊魂淡= =|||";
        return e;
    }
    /// <summary>
    /// 获取开启监听成功的消息
    /// </summary>
    /// <param name="sender"></param>
    /// <returns></returns>
    public static GetNoticeEventArgs GetClientReceivingSucceed(THEServer sender)
    {
        GetNoticeEventArgs e = GetTHEServerSystem(sender);
        e.content = "ClientReceivingSucceed";
        e.message = "服务端开启监听~开门迎接客人啦~";
        return e;
    }
    /// <summary>
    /// 获取开启监听失败的消息
    /// </summary>
    /// <param name="sender"></param>
    /// <returns></returns>
    public static GetNoticeEventArgs GetClientReceivingFailed(THEServer sender)
    {
        GetNoticeEventArgs e = GetTHEServerSystem(sender);
        e.content = "ClientReceivingFailed";
        e.message = "服务端开启监听失败呜呜~~~~(>_<)~~~~";
        return e;
    }
    /// <summary>
    /// 获取停止监听成功的消息
    /// </summary>
    /// <param name="sender"></param>
    /// <returns></returns>
    public static GetNoticeEventArgs GetStopClientReceivingSucceed(THEServer sender)
    {
        GetNoticeEventArgs e = GetTHEServerSystem(sender);
        e.content = "StopClientReceivingSucceed";
        e.message = "服务端停止监听~客房满啦~新客人就不要进啦~O(∩_∩)O~~";
        return e;
    }
    /// <summary>
    /// 获取停止监听失败的消息
    /// </summary>
    /// <param name="sender"></param>
    /// <returns></returns>
    public static GetNoticeEventArgs GetStopClientReceivingFailed(THEServer sender)
    {
        GetNoticeEventArgs e = GetTHEServerSystem(sender);
        e.content = "StopClientReceivingFailed";
        e.message = "服务端停止监听失败……才不是因为人家是⑨";
        return e;
    }
    /// <summary>
    /// 获取服务器释放全部资源的消息
    /// </summary>
    /// <param name="sender"></param>
    /// <returns></returns>
    public static GetNoticeEventArgs GetCloseTHEServer(THEServer sender)
    {
        GetNoticeEventArgs e = GetTHEServerSystem(sender);
        e.content = "CloseTHEServer";
        e.message = "啊！服务端释放全部资源……总算解放了【伸懒腰";
        return e;
    }
    /// <summary>
    /// 关闭某User的系统通知
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="">服务器监听的位置</param>
    /// <returns></returns>
    public static GetNoticeEventArgs GetCloseUser(THEServer sender, THEServer.User beClosedUser)
    {
        GetNoticeEventArgs e = GetTHEServerSystem(sender);
        e.relatedUser = beClosedUser;
        EndPoint RemoteEndPoint = beClosedUser.client.Client.RemoteEndPoint;
        e.content = string.Format("CloseUser,{0}", RemoteEndPoint);
        e.message = string.Format("刚刚释放了与这位客户端连接的资源_(:з」∠)_。→{0}", RemoteEndPoint);
        return e;
    }
    /// <summary>
    /// 某User的大数据包接收失败
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="">发送者</param>
    /// <returns></returns>
    public static GetNoticeEventArgs GetLongDataReceiveFail(THEServer sender, THEServer.User from)
    {
        GetNoticeEventArgs e = GetTHEServerSystem(sender);
        e.relatedUser = from;
        EndPoint RemoteEndPoint = from.client.Client.RemoteEndPoint;
        e.content = string.Format("LongDataReceiveFail,{0}", RemoteEndPoint);
        e.message = string.Format("这货发来的数据太长了……我吃不下QAQ求重发→{0}", RemoteEndPoint);
        return e;
    }

    //以下为服务端非系统类型
    public static GetNoticeEventArgs GetClientLogin(THEServer sender, THEServer.User connectUser)
    {
        GetNoticeEventArgs e = new GetNoticeEventArgs(sender, Attribute.ClientLogin);
        e.relatedUser = connectUser;
        EndPoint RemoteEndPoint = connectUser.client.Client.RemoteEndPoint;
        e.content = string.Format("ClientLogin,{0}", RemoteEndPoint);
        e.message = string.Format("哼哼~~有一个客户端姐姐连接啦。客户端信息：{0}", RemoteEndPoint);
        return e;
    }
    public static GetNoticeEventArgs GetClientLogoff(THEServer sender, THEServer.User logoffUser)
    {
        GetNoticeEventArgs e = new GetNoticeEventArgs(sender, Attribute.ClientLogoff);
        e.relatedUser = logoffUser;
        EndPoint RemoteEndPoint = logoffUser.client.Client.RemoteEndPoint;
        e.content = string.Format("ClientLogoff,{0}", RemoteEndPoint);
        e.message = string.Format("好桑心T^T有一个客户端姐姐不理我啦。这货→{0}", RemoteEndPoint);
        return e;
    }
    public static GetNoticeEventArgs GetClientRemove(THEServer sender, THEServer.User beRemovedUser)
    {
        GetNoticeEventArgs e = new GetNoticeEventArgs(sender, Attribute.ClientRemove);
        e.relatedUser = beRemovedUser;
        EndPoint RemoteEndPoint = beRemovedUser.client.Client.RemoteEndPoint;
        e.content = string.Format("ClientRemove,{0}", RemoteEndPoint);
        e.message = string.Format("咦！Master竟然不让我理这位客户端sama!是她做什么坏事了嘛0 0？→{0}", RemoteEndPoint);
        return e;
    }
    public static GetNoticeEventArgs GetClientLost(THEServer sender, THEServer.User beRemovedUser)
    {
        GetNoticeEventArgs e = new GetNoticeEventArgs(sender, Attribute.ClientLost);
        e.relatedUser = beRemovedUser;
        EndPoint RemoteEndPoint = beRemovedUser.client.Client.RemoteEndPoint;
        e.content = string.Format("ClientLost,{0}", RemoteEndPoint);
        e.message = string.Format("不好啦！不好啦！这位客户端失踪了( ⊙ o ⊙ )→{0}", RemoteEndPoint);
        return e;
    }
    public static GetNoticeEventArgs GetReceiveFromClient(THEServer sender, THEServer.User commucateUser, string msg)
    {
        GetNoticeEventArgs e = new GetNoticeEventArgs(sender, Attribute.ReceiveFromClient);
        e.relatedUser = commucateUser;
        e.commucateMessage = msg;
        EndPoint RemoteEndPoint = commucateUser.client.Client.RemoteEndPoint;

        e.content = string.Format("ReceiveFromClient,{0},{1}", RemoteEndPoint, msg);
        e.message = string.Format("客户端{0}发来消息：\r\n{1}", RemoteEndPoint, msg);
        return e;
    }
    public static GetNoticeEventArgs GetSendToClient(THEServer sender, THEServer.User commucateUser, string msg)
    {
        GetNoticeEventArgs e = new GetNoticeEventArgs(sender, Attribute.SendToClient);
        e.relatedUser = commucateUser;
        e.commucateMessage = msg;
        EndPoint RemoteEndPoint = commucateUser.client.Client.RemoteEndPoint;

        e.content = string.Format("SendToClient,{0},{1}", RemoteEndPoint, msg);
        e.message = string.Format("向客户端{0}发送消息：\r\n{1}", RemoteEndPoint, msg);
        return e;
    }
    public static GetNoticeEventArgs GetSendToClients(THEServer sender, List<THEServer.User> commucateUsers, string msg)
    {
        GetNoticeEventArgs e = new GetNoticeEventArgs(sender, Attribute.SendToClient);
        e.relatedUsers = commucateUsers;
        e.commucateMessage = msg;
        int num = commucateUsers.Count;
        string clientInfo = "";
        string showClientInfo = "";
        foreach (THEServer.User u in commucateUsers)
        {
            EndPoint RemoteEndPoint = u.client.Client.RemoteEndPoint;
            showClientInfo += RemoteEndPoint + "\r\n";
            clientInfo += RemoteEndPoint + ",";

        }

        e.content = string.Format("SendToClients,{0},{1}{2}", num, clientInfo, msg);
        e.message = string.Format("向{0}个客户端发送消息：\r\n{1}内容为：\r\n{2}", num, showClientInfo, msg);
        return e;
    }
    public static GetNoticeEventArgs GetSendToAll(THEServer sender, string msg)
    {
        GetNoticeEventArgs e = new GetNoticeEventArgs(sender, Attribute.SendToAll);
        e.commucateMessage = msg;
        int num = sender.users.Count;
        e.content = string.Format("SendToAll,{0},{1}", num, msg);
        e.message = string.Format("向所有客户端(共{0}个)广播：\r\n{1}", num, msg);
        return e;
    }
    public static GetNoticeEventArgs GetSendFailed(THEServer sender, THEServer.User commucateUser, string msg)
    {
        GetNoticeEventArgs e = new GetNoticeEventArgs(sender, Attribute.SendFailed);
        e.relatedUser = commucateUser;
        e.commucateMessage = msg;
        EndPoint RemoteEndPoint = commucateUser.client.Client.RemoteEndPoint;

        e.content = string.Format("SendFailed,{0},{1}", RemoteEndPoint, msg);
        e.message = string.Format("向{0}发送的消息：\r\n{1}\r\n发送失败！", RemoteEndPoint, msg);
        return e;
    }






    //以下为客户端消息
    private static GetNoticeEventArgs GetClientSystem(THEClient owner)
    {
        GetNoticeEventArgs e = new GetNoticeEventArgs(owner, Attribute.ClientSystem);
        return e;
    }
    public static GetNoticeEventArgs GetConnectSucceed(THEClient sender, IPEndPoint ipEndPoint)
    {
        GetNoticeEventArgs e = GetClientSystem(sender);
        e.content = String.Format("ConnectSucceed,{0},{1}", ipEndPoint.Address.ToString(), ipEndPoint.Port);
        e.message = "嘟嘟噜~找到服务器姐姐了~哦哈呦~~O(∩_∩)O~~";
        return e;
    }
    public static GetNoticeEventArgs GetConnectFailed(THEClient sender, IPEndPoint ipEndPoint)
    {
        GetNoticeEventArgs e = GetClientSystem(sender);
        e.content = String.Format("ConnectFailed,{0},{1}", ipEndPoint.Address.ToString(), ipEndPoint.Port);
        e.message = "(o゜▽゜)o☆索敌失败！并没有找到服务器sama。";
        return e;
    }
    public static GetNoticeEventArgs GetSendToTHEServer(THEClient sender, string msg)
    {
        GetNoticeEventArgs e = new GetNoticeEventArgs(sender, Attribute.SendToTHEServer);
        e.commucateMessage = msg;
        e.content = String.Format("Send,{0}", msg);
        e.message = String.Format("向服务器发送:\r\n{0}", msg);
        return e;
    }
    public static GetNoticeEventArgs GetSendFailed(THEClient sender, string msg)
    {
        GetNoticeEventArgs e = new GetNoticeEventArgs(sender, Attribute.SendFailed);
        e.commucateMessage = msg;
        e.content = String.Format("SendFailed,{0}", msg);
        e.message = String.Format("向服务器发送失败！发送内容：\r\n{0}", msg);
        return e;
    }
    public static GetNoticeEventArgs GetReceiveFromTHEServer(THEClient sender, string msg)
    {
        GetNoticeEventArgs e = new GetNoticeEventArgs(sender, Attribute.ReceiveFromTHEServer);
        e.commucateMessage = msg;
        e.content = String.Format("ReceiveFromTHEServer,{0}", msg);
        e.message = String.Format("接收到服务器内容：\r\n{0}", msg);
        return e;
    }
    public static GetNoticeEventArgs GetExit(THEClient sender, IPEndPoint ipEndPoint)
    {
        GetNoticeEventArgs e = GetClientSystem(sender);
        e.content = String.Format("Exit,{0},{1}", ipEndPoint.Address.ToString(), ipEndPoint.Port);
        e.message = String.Format("叶隐！成功关闭了与服务器的连接！\\(^o^)/YES!");
        return e;
    }
    public static GetNoticeEventArgs GetConnectLost(THEClient sender, IPEndPoint ipEndPoint)
    {
        GetNoticeEventArgs e = GetClientSystem(sender);
        e.content = String.Format("ConnectLost,{0},{1}", ipEndPoint.Address.ToString(), ipEndPoint.Port);
        e.message = String.Format("噫！少年你气场太强大了！搞得我和服务器失联了！坏人～(　TロT)σ");
        return e;
    }
    public static GetNoticeEventArgs GetCloseClient(THEClient sender)
    {
        GetNoticeEventArgs e = GetClientSystem(sender);
        e.content = string.Format("CloseClient");
        e.message = string.Format("这里刚刚释放了与服务端连接的资源_(:з」∠)_。");
        return e;
    }

    /// <summary>
    /// 某User的大数据包接收失败
    /// </summary>
    /// <param name="sender"></param>
    /// <returns></returns>
    public static GetNoticeEventArgs GetLongDataReceiveFail(THEClient sender)
    {
        GetNoticeEventArgs e = GetClientSystem(sender);
        e.content = string.Format("LongDataReceiveFail");
        e.message = string.Format("服务器发来的数据太长了……我吃不下QAQ求重发");
        return e;
    }

    /// <summary>
    /// 该消息所属类型的值。
    /// 服务器消息：
    ///     ClientMessage:从客户端接收的信息。
    ///     ClientLogin:客户端登陆的信息。
    ///     ClientLogoff:客户端登出的信息。
    ///     ClientRemove:删除一个客户端。
    ///     ClientLost:客户端连接丢失的信息。
    ///     ReceiveFromClient:客户端发来消息。
    ///     SendToClient:向某客户端发送信息。
    ///     SendToClients:向某些客户端发送信息。
    ///     SendToAll:向所有在线客户端发送的信息。
    ///     THEServerSystem(注意这里包含很多):系统（服务端）的消息。
    /// 
    /// 客户端消息：
    ///     ClientSystem(注意这里包含很多):系统（客户端）的消息。
    ///     ReceiveFromTHEServer:服务端发来消息。
    ///     SendToTHEServer:向服务端发送信息
    ///     
    /// 通用:
    ///     SendFailed:向某服务端/客户端发送信息失败。
    ///     SimpleMessage:其它纯文字消息。
    ///     UserSimpleMessage:带客户端信息的纯文字消息。
    /// 
    /// </summary>
    public enum Attribute
    {
        //服务端消息
        ClientMessage, ClientLogin, ClientLogoff, ClientRemove, ClientLost, ReceiveFromClient,
        SendToClient, SendToClients, SendToAll, THEServerSystem,
        //客户端消息
        ClientSystem, ReceiveFromTHEServer, SendToTHEServer,
        //通用
        SimpleMessage, UserSimpleMessage, SendFailed,
    };
}
