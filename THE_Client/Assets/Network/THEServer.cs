using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

//大包发送协议：先发送
//【TCP大数据包准备传输LongDataTransportStarting】
//【TCP大数据包传输完毕LongDataTransportEnding】{0}←   数据包哈希值

public class THEServer : NetCore
{

    /// <summary>
    /// 所有已连接的客户端
    /// </summary>
    public List<User> users = new List<User>();
    /// <summary>
    /// 使用的本机IP地址
    /// </summary>
    IPAddress localAddress;
    /// <summary>
    /// 监听端口
    /// </summary>
    private readonly int port;
    public IPEndPoint ipEndPoint
    {
        get
        {
            return new IPEndPoint(localAddress, port);
        }
    }

    public int bufferSize = 1024;
    /// <summary>
    /// 两次读数据时的间隔时间，单位毫秒，改变该项可以更改所有子User设置
    /// </summary>
    public int waitingTime
    {
        set
        {
            foreach (User u in users)
            {
                u.waitingTime = value;
            }
        }
    }
    private TcpListener myListener;

    /// <summary>
    /// 用于获取服务端的状态
    /// </summary>
    public State state { get; private set; }

    public bool allowConnect { get; private set; }

    /// <summary>
    /// 用于记录是否为正常退出
    /// </summary>
    public bool isStopped
    {
        get
        {
            return _isStopped;
        }
        private set
        {
            _isStopped = value;
            allowConnect = !isStopped;
        }
    }
    private bool _isStopped = true;


    /// <summary>
    /// 服务端的状态
    /// </summary>
    public enum State { Listening, Resting, Error };

    public THEServer(string localAddress, int port = 80, int bufferSize = 1024)
        : this(NetCore.GetIPAddress(localAddress)[0], port, bufferSize)
    {
    }
    public THEServer(IPAddress localAddress, int port = 80, int bufferSize = 1024)
        : this(new IPEndPoint(localAddress, port), bufferSize)
    {
    }
    public THEServer(IPEndPoint ipEndPoint, int bufferSize = 1024)
    {
        this.localAddress = ipEndPoint.Address;
        this.port = ipEndPoint.Port;
        this.bufferSize = bufferSize;
        myListener = new TcpListener(this.localAddress, port);
        state = State.Resting;
        allowConnect = false;
    }

    /// <summary>
    /// 用于开启服务端。
    /// </summary>
    /// <returns></returns>
    public bool Start()
    {
        if (state == State.Listening)
        {
            Notice(GetNoticeEventArgs.GetStartFailed(this));
            return false;
        }
        myListener.Start();
        //myListener.BeginAcceptTcpClient(ReceiveTcpClient, new Object());
        state = State.Listening;
        isStopped = false;
        allowConnect = false;//这里暂时禁止是为了下一步避嫌。
        StartConnect();
        return true;
    }
    /// <summary>
    /// 用于关闭服务器。
    /// </summary>
    /// <returns></returns>
    public bool Stop()
    {
        if (state != State.Listening)
        {
            Notice(GetNoticeEventArgs.GetStopFailed(this));
            return false;
        }
        state = State.Resting;
        StopConnect();
        isStopped = true;
        while (users.Count > 0)
        {
            users[0].Close();
        }
        return true;
    }

    /// <summary>
    /// 接收到客户端时调用的方法。并且会持续监听。
    /// </summary>
    /// <param name="asyncResult"></param>
    public void ReceiveTcpClient(IAsyncResult asyncResult)
    {
        //如果退出则停止运行
        if (allowConnect == false) return;
        TcpClient tcpClient = myListener.EndAcceptTcpClient(asyncResult);
        User u = new User(tcpClient, this, bufferSize);
        users.Add(u);
        //如果不退出，则再次接受客户端连接请求
        if (allowConnect == true)
        {
            myListener.BeginAcceptTcpClient(ReceiveTcpClient, new Object());
        }
    }
    /// <summary>
    /// 用于禁止新客户端连接
    /// </summary>
    public bool StopConnect()
    {
        if (isStopped == true || allowConnect == false)
        {
            Notice(GetNoticeEventArgs.GetStopClientReceivingFailed(this));
            return false;
        }
        allowConnect = false;
        Notice(GetNoticeEventArgs.GetStopClientReceivingSucceed(this));
        return true;
    }
    /// <summary>
    /// 用于开启/重新开启新客户端连接
    /// </summary>
    public bool StartConnect()
    {
        if (isStopped == true || allowConnect == true)
        {
            Notice(GetNoticeEventArgs.GetClientReceivingFailed(this));
            return false;
        }
        allowConnect = true;
        myListener.BeginAcceptTcpClient(ReceiveTcpClient, new Object());
        Notice(GetNoticeEventArgs.GetClientReceivingSucceed(this));
        return true;
    }
    /// <summary>
    /// 移除用户
    /// </summary>
    /// <param name="user"></param>
    private void RemoveUser(User user)
    {
        user.Close();
        Notice(GetNoticeEventArgs.GetClientRemove(this, user));
    }

    /// <summary>
    /// 向所有连接的客户端发送消息
    /// </summary>
    /// <param name="Msg"></param>
    /// <returns></returns>
    public override bool Send(string msg)
    {
        List<User> failToSend;
        return Send(msg, out failToSend);
    }
    /// <summary>
    /// 向所有连接的客户端发送消息
    /// </summary>
    /// <param name="Msg"></param>
    /// <param name="failToSend">发送失败的客户端</param>
    /// <returns></returns>
    public bool Send(string msg, out List<User> failToSend)
    {
        bool result = true;
        failToSend = new List<User>();
        Notice(GetNoticeEventArgs.GetSendToAll(this, msg));
        foreach (User u in users)
        {
            if (u.Send(msg, false) == false)
            {
                failToSend.Add(u);
                result = false;
            }
        }
        return result;
    }
    /// <summary>
    /// 向某客户端发送信息
    /// </summary>
    /// <param name="toSend">欲接受信息的客户端</param>
    /// <param name="msg">欲发送的信息</param>
    /// <returns></returns>
    public bool Send(User toSend, string msg)
    {
        return toSend.Send(msg);
    }

    /// <summary>
    /// 向某些客户端发送信息
    /// </summary>
    /// <param name="toSend">欲接受信息的客户端</param>
    /// <param name="msg">欲发送的信息</param>
    /// <param name="failToSend">发送失败的客户端</param>
    /// <returns>全部成功才返回True，如果为False，failToSend则会返回发送失败的客户端</returns>
    public bool Send(List<User> toSend, string msg, out List<User> failToSend)
    {
        bool result = true;
        failToSend = new List<User>();
        Notice(GetNoticeEventArgs.GetSendToClients(this, toSend, msg));
        foreach (User u in toSend)
        {
            if (u.Send(msg, false) == false)
            {
                failToSend.Add(u);
                result = false;
            }
        }
        return result;
    }

    public override bool Close()
    {
        if (isClosed == true) return false;
        if (state == State.Listening) Stop();
        foreach (User u in users)
        {
            u.Close();
        }
        myListener.Stop();
        isClosed = true;
        Notice(GetNoticeEventArgs.GetCloseTHEServer(this));
        return true;
    }

    /// <summary>
    /// 用于对单个连接的客户端进行操作。
    /// </summary>
    public class User : NetCore
    {
        /// <summary>
        /// 记录所属THEServer
        /// </summary>
        public THEServer owner;

        /// <summary>
        /// 用于获取该客户端的连接状态
        /// </summary>
        public State state { get; private set; }
        public TcpClient client { get; private set; }
        //public BinaryReader br { get; private set; }
        //public BinaryWriter bw { get; private set; }
        public string userName { get; set; }

        public int bufferSize { get; private set; }

        public byte[] buffer;

        private bool longDataMod = false;

        /// <summary>
        /// 两次读数据时的间隔时间，单位毫秒
        /// </summary>
        public int waitingTime = 50;

        public List<byte[]> tempMessage;
        private NetworkStream networkStream;
        public User(TcpClient client, THEServer owner, int bufferSize = 1024)
        {
            this.client = client;
            this.owner = owner;
            this.bufferSize = bufferSize;
            networkStream = client.GetStream();
            //br = new BinaryReader(networkStream);
            //bw = new BinaryWriter(networkStream);
            buffer = new byte[bufferSize];
            state = State.Connected;
            Notice(GetNoticeEventArgs.GetClientLogin(owner, this));
            if (owner.isStopped == false)
            {
                //开始读取信息
                networkStream.BeginRead(buffer, 0, bufferSize, ReadCallback, new Object());
            }
        }

        public override void Notice(GetNoticeEventArgs e)
        {
            Notice(owner, e);
            owner.Notice(e);
        }

        /// <summary>
        /// 客户端有数据发送过来，就会调用这个方法
        /// </summary>
        /// <param name="ar"></param>
        public void ReadCallback(IAsyncResult ar)
        {
            if (owner.isStopped == true) return;
            //↓检测是否为空载←实际测试并没有什么卵用
            if (client.Connected == false)
            {
                Notice(GetNoticeEventArgs.GetClientLost(owner, this));
                Close();
                return;
            }
            try
            {
                //结束异步读取方法,返回值是读取了多少字节
                int bytesRead = networkStream.EndRead(ar);
                string msg = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                if (bytesRead > 0)//如果接受到数据
                {
                    if (longDataMod == false)
                    //普通模式
                    {
                        //检测是否进入大数据模式
                        if (msg == "【TCP大数据包准备传输LongDataTransportStarting】")
                        {
                            longDataMod = true;
                            tempMessage = new List<byte[]>();
                        }
                        else
                        {
                            Notice(GetNoticeEventArgs.GetReceiveFromClient(owner, this, msg));
                        }
                    }
                    else//大数据模式
                    {
                        //然最后两个相加防止被割断
                        byte[] temp = new byte[bytesRead];
                        for (int a = 0; a < bytesRead; a++)
                        {
                            temp[a] = buffer[a];
                        }
                        int tempInt;
                        string tempString;
                        if (tempMessage.Count != 0)
                        {
                            tempInt = tempMessage[tempMessage.Count - 1].Count() + bytesRead;
                            byte[] a = new byte[tempMessage[tempMessage.Count - 1].Length + temp.Length];
                            tempMessage[tempMessage.Count - 1].CopyTo(a, 0);
                            temp.CopyTo(a, tempMessage[tempMessage.Count - 1].Length);
                            tempString = Encoding.Unicode.GetString(a);
                        }
                        else
                        {
                            tempInt = bytesRead;
                            tempString = Encoding.Unicode.GetString(temp);
                        }
                        string[] tempStringArray = tempString.Split('】');
                        //是否退出大数据模式
                        if (tempStringArray.Count() >= 2 && tempStringArray[tempStringArray.Count() - 2].EndsWith("【TCP大数据包传输完毕LongDataTransportEnding") == true)
                        {
                            longDataMod = false;

                            //拼包
                            int tempLength = temp.Length;
                            foreach (byte[] b in tempMessage)
                            {
                                tempLength += b.Length;
                            }
                            byte[] tempByteArray = new byte[tempLength];
                            int tempPosition = 0;
                            foreach (byte[] b in tempMessage)
                            {
                                b.CopyTo(tempByteArray, tempPosition);
                                tempPosition += b.Length;
                            }
                            temp.CopyTo(tempByteArray, tempPosition);
                            string longMsg = Encoding.Unicode.GetString(tempByteArray);
                            longMsg = longMsg.Substring(0, longMsg.LastIndexOf('【'));

                            //校验包完整性
                            int hashCode = int.Parse(tempStringArray[tempStringArray.Count() - 1]);

                            if (longMsg.GetHashCode() == hashCode)
                            {
                                //匹配
                                Notice(GetNoticeEventArgs.GetReceiveFromClient(owner, this, longMsg));
                            }
                            else
                            {
                                //不匹配
                                Notice(GetNoticeEventArgs.GetLongDataReceiveFail(owner, this));
                                System.Diagnostics.Debug.WriteLine(longMsg);
                            }
                            tempMessage = new List<byte[]>();
                        }
                        else
                        {
                            tempMessage.Add(temp);
                        }
                    }
                }
                //if (tempMessage == "")//判断当前是新数据状态，还是接受完成状态
                //{
                //    if (bytesRead > 0)//如果接受到数据
                //    {
                //        tempMessage += msg;
                //    }
                //}
                //else//接受完成或接受中状态
                //{
                //    if (bytesRead > 0)//如果接受到数据
                //    {
                //        tempMessage += msg;
                //    }
                //    else
                //    {
                //        Notice(GetNoticeEventArgs.GetReceiveFromClient(owner, this, tempMessage));
                //        tempMessage = "";

                //    }
                //}
                if (owner.isStopped == false && isClosed == false)
                {
                    //这次读取BeginRead结束，继续下一次读取
                    networkStream.BeginRead(buffer, 0, bufferSize, ReadCallback, new Object());
                }
            }
            catch (IOException)
            {
                Notice(GetNoticeEventArgs.GetClientLost(owner, this));
                this.Close();
            }
        }
        /// <summary>
        /// 向某客户端发送信息
        /// </summary>
        /// <param name="msg">所发送的信息</param>
        /// <param name="showTHEServerMsg">是否通知总监控，该选项对失败通知无效</param>
        /// <param name="showUserMsg">是否通知User监控，该选项对失败通知无效</param>
        /// <returns></returns>
        public bool Send(string msg, bool showTHEServerMsg, bool showUserMsg = true)
        {
            if (state != State.Connected) return false;
            byte[] sendBuffer = Encoding.Unicode.GetBytes(msg);

            //如果大于1024进入大数据模式
            if (Encoding.Unicode.GetByteCount(msg) > 1024)
            {
                Send("【TCP大数据包准备传输LongDataTransportStarting】", false, false);
                List<byte> temp = sendBuffer.ToList();
                while (temp.Count > 1024)
                {
                    byte[] newBuffer = new byte[1024];
                    for (int a = 0; a < 1024; a++)
                    {
                        newBuffer[a] = temp[a];
                    }
                    Send(Encoding.Unicode.GetString(newBuffer), false, false);
                    temp.RemoveRange(0, 1024);
                }
                Send(Encoding.Unicode.GetString(temp.ToArray()), false, false);
                Send("【TCP大数据包传输完毕LongDataTransportEnding】" + msg.GetHashCode(), false, false);
                //按情况通知
                if (showTHEServerMsg)
                {
                    owner.Notice(GetNoticeEventArgs.GetSendToClient(owner, this, msg));
                }
                if (showUserMsg)
                {
                    Notice(this, GetNoticeEventArgs.GetSendToClient(owner, this, msg));
                }
                return true;
            }

            try
            {
                if (useSendAsync)//检测发送是否使用异步方法
                {
                    networkStream.BeginWrite(sendBuffer, 0, sendBuffer.Length, null, null);
                }
                else
                {
                    networkStream.Write(sendBuffer, 0, sendBuffer.Length);
                }
            }
            catch (IOException)
            {
                if (isClosed == false)
                    Notice(GetNoticeEventArgs.GetClientLost(owner, this));
                this.Close();
                return false;
            }
            if (showTHEServerMsg)
            {
                owner.Notice(GetNoticeEventArgs.GetSendToClient(owner, this, msg));
            }
            if (showUserMsg)
            {
                Notice(this, GetNoticeEventArgs.GetSendToClient(owner, this, msg));
            }
            return true;
        }
        /// <summary>
        /// 向某客户端发送信息，默认通知选项为全通知
        /// </summary>
        /// <param name="msg">所发送的信息</param>
        /// <returns></returns>
        public override bool Send(string msg)
        {
            return Send(msg, true);
        }
        /// <summary>
        /// 关闭方法，回收资源
        /// </summary>
        public override bool Close()
        {
            if (isClosed == true) return false;
            if (state != State.Connected) return false;
            state = State.Closed;
            owner.users.Remove(this);
            //br.Close();
            //bw.Close();
            Notice(GetNoticeEventArgs.GetCloseUser(owner, this));
            client.Client.Close();
            client.Close();
            isClosed = true;
            return true;
        }

        /// <summary>
        /// 客户端状态：连接，关闭，丢失
        /// </summary>
        public enum State { Connected, Closed, Lost }
    }
}
