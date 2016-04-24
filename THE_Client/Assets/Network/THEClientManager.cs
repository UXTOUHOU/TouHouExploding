using UnityEngine;
using System.Collections;
using System;

public class THEClientManager
{
    private static THEClientManager _instance;

    public static THEClientManager getInstance()
    {
        if (_instance == null)
        {
            _instance = new THEClientManager();
        }
        return _instance;
    }

    private THEClient _client;

    public THEClientManager()
    {
        this._client = new THEClient();
        this._client.getNoticeEvent += this.log;
    }

    public bool connectServer(string serverIP,int port)
    {
        return this._client.Connect(serverIP, port);
    }

    public bool sendCommand(string command)
    {
        return this._client.Send(command);
    }

    public void log(System.Object sender, GetNoticeEventArgs e)
    {
        Debug.Log(e.message);
    }
}
