class BattleConsts
{
    /// <summary>
    /// 初始化游戏
    /// </summary>
    public const int BattleState_InitGame = 1;
    /// <summary>
    /// 回合开始阶段
    /// </summary>
    public const int BattleState_TurnStartPhase = 2;
    /// <summary>
    /// 准备阶段
    /// </summary>
    public const int BattleState_StandbyPhase = 3;
    /// <summary>
    /// 主要阶段，等待玩家自己操作
    /// </summary>
    public const int BattleState_MainPhase = 4;

    /// <summary>
    /// 单元格操作-空闲
    /// </summary>
    public const int CellOp_Idle = 1;

    public const int MapMaxRow = 8;

    public const int MapMaxCol = 12;
    /// <summary>
    /// 单元格默认尺寸
    /// </summary>
    public const int DefaultCellSize = 75;

    public const int BattleFieldLayer_Bg = 1;
    public const int BattleFieldLayer_Unit = 2;
    public const int BattleFieldLayer_Effect = 3;
    public const int BattleFieldLayer_Terrain = 4;
}

