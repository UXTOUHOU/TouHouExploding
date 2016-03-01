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
    /// <summary>
    /// 游戏是否已经开始
    /// </summary>
    private bool _isGameStarted;

    public void startGame()
    {
        this._isGameStarted = true;
    }

    void Awake()
    {
        _instance = this;
        BattleGlobal.Core = this;
    }

	// Use this for initialization
	void Start () 
    {
        this._isGameStarted = false;
        BattleStateManager.getInstance().setState(BattleConsts.BattleState_InitGame);
	}
	
	// Update is called once per frame
	void Update () 
    {
        BattleStateManager.getInstance().update();
        if ( this._isGameStarted )
        {
            UnitManager.getInatance().update();
        }
	}
}
