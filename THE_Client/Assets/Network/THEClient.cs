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
//【TCP大数据包传输完毕LongDataTransportEnding】{0}←   数据包的哈希值

public class THEClient : NetCore
{
    /// <summary>
    /// 使用的服务IP地址
    /// </summary>
    IPAddress THEServerAddress;
    /// <summary>
    /// 服务器端口
    /// </summary>
    private readonly int port;
    public TcpClient tcpClient;
    public NetworkStream networkStream;
    /// <summary>
    /// 用于获取客户端的状态
    /// </summary>
    public State state { get; private set; }
    /// <summary>
    /// 客户端的状态
    /// </summary>
    public enum State { Connecting, Resting, Lost };
    public int bufferSize { get; private set; }
    public byte[] buffer;
    /// <summary>
    /// 两次读数据时的间隔时间，单位毫秒
    /// </summary>
    public int waitingTime = 50;
    private List<byte[]> tempMessage;
    private bool longDataMod = false;

    public THEClient(int bufferSize = 1024)
    {
        this.bufferSize = bufferSize;
        buffer = new byte[bufferSize];
        state = State.Resting;
        THEServerAddress = null;
        useSendAsync = true;
    }
    public THEClient(string localAddress, int port = 80, int bufferSize = 1024)
        : this(NetCore.GetIPAddress(localAddress)[0], port, bufferSize)
    {
    }

    public THEClient(IPAddress localAddress, int port = 80, int bufferSize = 1024)
    {
        this.THEServerAddress = localAddress;
        this.port = port;
        this.bufferSize = bufferSize;
        buffer = new byte[bufferSize];
        state = State.Resting;
        useSendAsync = true;
    }

    /// <summary>
    /// 连接服务器
    /// </summary>
    /// <returns></returns>

    public bool Connect(string localAddress, int port = 80)
    {
        return Connect(NetCore.GetIPAddress(localAddress)[0], port);
    }

    /// <summary>
    /// 连接服务器
    /// </summary>
    /// <returns></returns>

    public bool Connect(IPAddress localAddress, int port = 80)
    {
        return Connect(new IPEndPoint(localAddress, port));
    }

    /// <summary>
    /// 连接服务器
    /// </summary>
    /// <returns></returns>

    public bool Connect(IPEndPoint ipEndPoint = null)
    {
        if (state == State.Connecting) return false;
        if (ipEndPoint == null)
        {
            if (THEServerAddress == null)
            {
                return false;
            }
            else
            {
                ipEndPoint = this.ipEndPoint;
            }
        }
        tcpClient = new TcpClient();
        try
        {
            tcpClient.Connect(ipEndPoint);
        }
        catch
        {
            Notice(GetNoticeEventArgs.GetConnectFailed(this, ipEndPoint));
            return false;
        }
        networkStream = tcpClient.GetStream();
        state = State.Connecting;
        networkStream.BeginRead(buffer, 0, bufferSize, ReadCallback, new Object());
        Notice(GetNoticeEventArgs.GetConnectSucceed(this, ipEndPoint));
        return true;
    }
    /// <summary>
    /// 退出服务器
    /// </summary>
    /// <returns></returns>
    public bool Disconnect()
    {
        if (state != State.Connecting) return false;
        isStopped = true;
        Notice(GetNoticeEventArgs.GetExit(this, ipEndPoint));
        tcpClient.Close();
        state = State.Resting;
        return true;
    }
    /// <summary>
    /// 发送数据
    /// </summary>
    /// <returns></returns>
    public override bool Send(string msg)
    {
        return Send(msg, true);
    }
    /// <summary>
    /// 发送数据，第二个参数决定是否显示信息。只影响发送成功的情况。第三个参数为发送完成后的回调方法，如果填了则默认此次发送使用异步模式。
    /// </summary>
    /// <returns></returns>
    public bool Send(string msg, bool ShowMsg, AsyncCallback callBack = null)
    {
        if (state != State.Connecting) return false;
        byte[] sendBuffer = Encoding.Unicode.GetBytes(msg);
        //如果大于1024进入大数据模式
        if (Encoding.Unicode.GetByteCount(msg) > 1024)
        {
            Send("【TCP大数据包准备传输LongDataTransportStarting】", false);
            List<byte> temp = sendBuffer.ToList();
            while (temp.Count > 1024)
            {
                byte[] newBuffer = new byte[1024];
                for (int a = 0; a < 1024; a++)
                {
                    newBuffer[a] = temp[a];
                }
                Send(Encoding.Unicode.GetString(newBuffer), false);
                temp.RemoveRange(0, 1024);
            }
            Send(Encoding.Unicode.GetString(temp.ToArray()), false);
            Send("【TCP大数据包传输完毕LongDataTransportEnding】" + msg.GetHashCode(), false);
            if (ShowMsg)
                Notice(GetNoticeEventArgs.GetSendToTHEServer(this, msg));
            return true;
        }
        try
        {
            if (useSendAsync || callBack != null)//检测发送是否使用异步方法
            {
                networkStream.BeginWrite(sendBuffer, 0, sendBuffer.Length, callBack, null);
            }
            else
            {
                networkStream.Write(sendBuffer, 0, sendBuffer.Length);
            }

            System.Diagnostics.Debug.WriteLine(msg);
            System.Diagnostics.Debug.WriteLine("-------------------------------------------------");
            if (ShowMsg)
                Notice(GetNoticeEventArgs.GetSendToTHEServer(this, msg));
            return true;
        }
        catch (IOException)
        {
            if (isClosed == false)
                Notice(GetNoticeEventArgs.GetConnectLost(this, ipEndPoint));
            this.Close();
            return false;
        }
    }
    /// <summary>
    /// 接收数据
    /// </summary>
    /// <returns></returns>
    public void ReadCallback(IAsyncResult ar)
    {
        if (isStopped == true) return;
        //↓检测是否为空载←实际测试并没有什么卵用
        if (tcpClient.Connected == false)
        {

            Notice(GetNoticeEventArgs.GetConnectLost(this, ipEndPoint));
            Close();
            return;
        }
        try
        {
            //结束异步读取方法,返回值是读取了多少字节
            int bytesRead = networkStream.EndRead(ar);
            String msg = Encoding.Unicode.GetString(buffer, 0, bytesRead);
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
                        Notice(GetNoticeEventArgs.GetReceiveFromTHEServer(this, msg));
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
                            Notice(GetNoticeEventArgs.GetReceiveFromTHEServer(this, longMsg));
                        }
                        else
                        {
                            //不匹配
                            Notice(GetNoticeEventArgs.GetLongDataReceiveFail(this));
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
            //        Notice(GetNoticeEventArgs.GetReceiveFromTHEServer(this, tempMessage));
            //        tempMessage = "";

            //    }
            //}
            if (isStopped == false)
            {
                //这次读取BeginRead结束，继续下一次读取
                networkStream.BeginRead(buffer, 0, bufferSize, ReadCallback, new Object());
            }
        }
        catch (IOException)
        {
            Notice(GetNoticeEventArgs.GetConnectLost(this, ipEndPoint));
            this.Close();
        }

    }
    /// <summary>
    /// 关闭客户端，释放资源，关闭后不可重新开启
    /// </summary>
    /// <returns></returns>
    public override bool Close()
    {
        if (isClosed == true) return false;
        isStopped = true;
        if (state == State.Connecting) Disconnect();
        Notice(GetNoticeEventArgs.GetCloseClient(this));
        tcpClient.Close();
        return true;
    }
    /// <summary>
    /// 用于记录是否为正常退出
    /// </summary>
    public bool isStopped { get; private set; }
    public IPEndPoint ipEndPoint
    {
        get
        {
            return new IPEndPoint(THEServerAddress, port);
        }
    }
}
