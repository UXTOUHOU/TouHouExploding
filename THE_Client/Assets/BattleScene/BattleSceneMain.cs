using UnityEngine;
using System.Collections;
using BattleScene;

public class BattleSceneMain : MonoBehaviour 
{
    private static BattleSceneMain _instance;

    public static BattleSceneMain getInstance()
    {
        return _instance;
    }

    private Chessboard _chessboard;
    /// <summary>
    /// 通过这个方法拿到地图的ui控制类
    /// </summary>
    public Chessboard chessboard
    {
        get { return this._chessboard; }
        set { this._chessboard = value; }
    }

    void Awake()
    {
        _instance = this;
    }

	// Use this for initialization
	void Start () 
    {
        BattleStateManager.getInstance().setState(BattleConsts.BattleState_InitGame);
	}
	
	// Update is called once per frame
	void Update () 
    {
        BattleStateManager.getInstance().update();
        // 全局监听右键点击
        /*if (Input.GetMouseButton(1))
        {
            OperationManager.getInstance().onRightClick();
        }*/
	}
}
