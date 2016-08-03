using UnityEngine;
using System.Collections;
using System;

public class THEServerManager
{
    private static THEServerManager _instance;

    public static THEServerManager getInstance()
    {
        if (_instance == null)
        {
            _instance = new THEServerManager();
        }
        return _instance;
    }

    private THEServer _server;

    public THEServerManager()
    {
        this._server = new THEServer("10.0.1.179",8201);
        this._server.getNoticeEvent += this.log;
    }

    public void startServer()
    {
        this._server.Start();
    }

    public void log(System.Object sender, GetNoticeEventArgs e)
    {
        Debug.Log(e.message);
    }
}