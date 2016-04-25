using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BattleScene;
using System.Threading;

public class BattleCore
{
    private Chessboard _chessboard;
    /// <summary>
    /// 通过这个方法拿到地图的ui控制类
    /// </summary>
    public Chessboard chessboard
    {
        get { return this._chessboard; }
        set { this._chessboard = value; }
    }

    private List<Player> _playerList;

    /// <summary>
    /// 游戏是否已经开始
    /// </summary>
    private bool _isGameStarted;

    private Thread _processorThread;

    private BattleInfo _info;
    public BattleInfo battleInfo
    {
        get { return this._info; }
    }

    private MTRandom _random;

    private UnitPool[] _unitPools;

    public void init()
    {
        int i;
        BattleGlobal.Core = this;
        this._playerList = new List<Player>();
        for (i=0;i<2;i++)
        {
            this._playerList.Add(new Player());
        }
        if (this._info == null)
        {
            this._info = new BattleInfo();
        }
        this._info.reset();
        this._random = new MTRandom();
        // test 设置固定随机种子
        this.initRandomSeed();
        // 初始化单位池
        this._unitPools = new UnitPool[2];
        for (i = 0; i < 2; i++)
        {
            this._unitPools[0] = new UnitPool();
            this._unitPools[0].init();
            this._unitPools[0].fill();
        }
        this._isGameStarted = false;
        // 初始化战场区域
        this._chessboard.init();
    }

    public void startGame()
    {
        this._isGameStarted = true;
        this._processorThread = new Thread(new ThreadStart(ProcessManager.getInstance().threadUpdate));
        this._processorThread.Start();
    }

    /// <summary>
    /// 获取玩家
    /// </summary>
    /// <param name="playerId"></param>
    /// <returns></returns>
    public Player getPlayer(int playerId)
    {
        if (playerId != 0 && playerId != 1)
        {
            return null;
        }
        return this._playerList[playerId];
    }

    /// <summary>
    /// 获取单位池
    /// </summary>
    /// <param name="playerId"></param>
    /// <returns></returns>
    public UnitPool getUnitPool(int playerId)
    {
        if ( playerId < 0 || playerId > 1 )
        {
            return null;
        }
        return this._unitPools[playerId];
    }

    public void update()
    {
		BattleStateManager.getInstance().update();
        if (this._isGameStarted)
        {
            UnitManager.getInatance().update();
        }
    }

    public void initRandomSeed(long seed = 19650218)
    {
        this._random.init(seed);
    }

    public int getRandom(int min, int max)
    {
        return this._random.getNext(min, max);
    }
}
