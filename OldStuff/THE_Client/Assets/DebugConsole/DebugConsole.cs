using UnityEngine;
using System.Collections;
using System;

public class DebugConsole : MonoBehaviour
{

    public string Content = "这里是控制台";
    public string Input = "";
    public int WindowWidth = 600;
    public int WindowHeight = 500;
    public string LastException = "";
    public bool Visible = true;
    private Rect windowRect0;

    // Use this for initialization
    void Start()
    {
        var screenWidth = Screen.width;
        var screenHeight = Screen.height;

        var windowX = (screenWidth - WindowWidth) / 2;
        var windowY = (screenHeight - WindowHeight) / 2;

        //将窗口放置到屏幕中间  
        windowRect0 = new Rect(windowX, windowY, WindowWidth, WindowHeight);
    }

    // Update is called once per frame
    void Update()
    {
        //按下回车发送指令
        if (UnityEngine.Input.GetKeyDown(KeyCode.Return))
        {
            Enter();
        }
        //按下F12切换显示
        if (UnityEngine.Input.GetKeyDown(KeyCode.F12))
        {
            Visible = !Visible;
        }
    }

    void OnGUI()
    {
        //content = GUI.TextArea(new Rect(10, 10, Screen.width * 0.5f, Screen.height * 0.5f), content);
        //input = GUI.TextField(new Rect(10, 10 + Screen.height * 0.5f, Screen.width * 0.5f - 50, 30), input);
        //if (GUI.Button(new Rect(Screen.width * 0.5f - 50 + 10, 10 + Screen.height * 0.5f, 50, 30), "Enter"))
        //{
        //    //如果按下按钮
        //}


        //windowRect0 = new Rect(windowRect0.x, windowRect0.y, WindowWidth, WindowHeight);
        if(Visible)
             windowRect0 = GUILayout.Window(0, windowRect0, DebugForm, "Console");


    }
    void DebugForm(int windowID)
    {
        //设定可拖动区域
        //GUI.DragWindow(new Rect(0, 0, WindowWidth, 20));
        //GUI.DragWindow(new Rect(0, 0, 10, WindowHeight));
        //GUI.DragWindow(new Rect(WindowWidth-10, 0, 10, WindowHeight));
        //GUI.DragWindow(new Rect(0, WindowHeight-10, WindowWidth, 10));

        GUI.DragWindow(new Rect(0, 0, WindowWidth, 20));
        GUI.DragWindow(new Rect(0, 0, 10, 1000000000000000000));
        GUI.DragWindow(new Rect(WindowWidth - 10, 0, 10, 1000000000000000000));
        //GUI.DragWindow(new Rect(0, WindowHeight - 10, WindowWidth, 10));
        //排版
        GUILayout.BeginVertical();
        GUILayout.TextArea(Content, GUILayout.ExpandHeight(true));

        GUILayout.BeginHorizontal();
        Input = GUILayout.TextField(Input, GUILayout.Height(25), GUILayout.ExpandWidth(true));
        if (GUILayout.Button("Enter", GUILayout.Height(25), GUILayout.Width(50)))
        {
            //如果按下按钮
            Enter();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
    /// <summary>
    /// 输入指令
    /// </summary>
    public void Enter()
    {
        if (Input == "") return;
        AddMsg("嘟嘟噜~收到命令【" + Input + "】~");
        DealCommend(Input);
        Input = "";
    }
    /// <summary>
    /// 处理指令。
    /// </summary>
    /// <param name="input"></param>
    public void DealCommend(string input)
    {
        //显示帮助
        if (input.StartsWith("？") || input.StartsWith("?") || input.StartsWith("Help") ||
            input.StartsWith("help") || input.StartsWith("HELP"))
            Help();
        //如果是控制台相关指令
        if (input.StartsWith("Console"))
        {
            try
            {
                string[] i = input.Split(' ', ',', ';', '，', '；', '-');
                //读取上一个错误
                if (i[1] == "LastException") AddMsg(LastException);
                //更改窗口高度
                if (i[1] == "Height") WindowHeight = int.Parse(i[2]);
                //更改窗口宽度
                if (i[1] == "Width") WindowWidth = int.Parse(i[2]);
                //清空
                if (i[1] == "Clear") Clear();
                //如果没有输入则显示帮助
                if (i[1] == "") Help();
            }
            catch(Exception e)
            {
                LastException = e.ToString();
                AddMsg("拒绝执行= =#！您可以输入Console LastException获取详细错误信息！");
            }
        }
    }
    public void AddMsg(string msg)
    {
        Content += Environment.NewLine + msg;
        Debug.Log(msg);
    }
    public void AddLine()
    {
        AddMsg("---------------------------------------");
    }
    public void Clear()
    {
        Content = "";
    }
    /// <summary>
    /// 显示帮助。
    /// </summary>
    public void Help()
    {
        AddLine();
        AddLine();
        AddMsg("这里是亲耐的帮助小姐米娜哦哈呦~");
        AddMsg("输入Help可以查看帮助哦~【还要你说= =？");
        AddMsg("如果要勾搭本小姐别忘了在命令前添加本小姐的名字Console哦ww");
        AddMsg("举个栗子，如果需要本小姐为您打扫屏幕只要对我说“Console Clear”就行了嗯。");
        AddMsg("BTW，本小姐非常严格，大小写不对一律无视，希望您注意啦。");
        AddMsg("BTW2，您对本小姐说什么本小姐会重复一遍方便您记录，但是不代表我听懂了，请注意。");
        AddLine();
        AddMsg("下面是本小姐能为您做的事情：【鞠躬");
        AddMsg("Clear——清屏。");
        AddMsg("LastException——显示上一个错误。");
        AddMsg("Height [x]——改变本小姐的身高【要是太矮了人家会生气的哦= =！");
        AddMsg("Width [x]——改变本小姐的宽度【只要你看着不别扭，我不介意瘦一点233");
        AddLine();
        AddMsg("下面要说的是本小姐的欧尼酱游戏核心桑的指令：ヾ(^▽^*)))");
        AddMsg("(。・＿・。)ﾉ什么？他还什么都做不了？真是太抱歉了。");
        AddLine();
        AddLine();
    }
}
