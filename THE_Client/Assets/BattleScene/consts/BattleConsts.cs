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
    /// 主要阶段的子状态-空闲
    /// </summary>
    public const int MainPhaseSubState_Idle = 4;
    /// <summary>
    /// 主要阶段的子状态-选择单位行动
    /// </summary>
    public const int MainPhaseSubState_SelectUnitAction = 5;
    /// <summary>
    /// 主要阶段的子状态-选择单位技能
    /// </summary>
    public const int MainPhaseSubState_SelectUnitSkill = 6;
    /// <summary>
    /// 主要阶段的子状态-选择单位移动路径
    /// </summary>
    public const int MainPhaseSubState_SelectMovePath = 7;
    /// <summary>
    /// 主要阶段的子状态-单位移动
    /// </summary>
    public const int MainPhaseSubState_MoveUnit = 8;
    /// <summary>
    /// 主要阶段的子状态-选择攻击目标
    /// </summary>
    public const int MainPhaseSubState_SelectAttackTarget = 9;
    /// <summary>
    /// 主要阶段的子状态-单位攻击
    /// </summary>
    public const int MainPhaseSubState_UnitAttack = 10;
    /// <summary>
    /// 主要阶段的子状态-反击
    /// </summary>
    public const int MainPhaseSubState_CounterAttack = 11;
    /// <summary>
    /// 召唤单位（选择位置）
    /// </summary>
    public const int MainPhaseSubState_SummoningUnit = 12;
    /// <summary>
    /// 处理事件-结果的状态
    /// </summary>
    public const int BattleState_Processing = 13;

    /// <summary>
    /// 单元格操作-空闲
    /// </summary>
    public const int CellOp_Idle = 1;

    public const int MapMaxRow = 8;

    public const int MapMaxCol = 12;
    /// <summary>
    /// 单元格默认尺寸
    /// </summary>
    public const int DefaultCellSize = 76;
    /// <summary>
    /// 默认单位移动每格的时间
    /// </summary>
    public const float DefaultMoveTimePerCell = 0.5f;

    public const int BattleFieldLayer_Bg = 1;
    public const int BattleFieldLayer_Unit = 2;
    public const int BattleFieldLayer_Effect = 3;
    public const int BattleFieldLayer_Terrain = 4;
    public const int BattleFieldLayer_UI = 5;

    /// <summary>
    /// 当单元格被选中时
    /// </summary>
    public const int CMD_OnCellSelected = 40000;
    public const int CMD_SelectCard = 40001;
    /// <summary>
    /// 单位移动中路过单元格
    /// </summary>
    public const int CMD_UnitStepOnCell = 40002;
    /// <summary>
    /// 单位最终移动完成
    /// </summary>
    public const int CMD_UnitMoveComplete = 40003;

    public const int LINE_FLAG_HORIZON = 1;

    public const int LINE_FLAG_VERTICAL = 2;

    public const int LINE_FLAG_CROSS = 3;

    public const int PROPERTY_PLAYER_ID = 1000;
    public const int PROPERTY_UNIT_ID = 1001;
    public const int PROPERTY_UNIT = 1002;

    public const int PROPERTY_BPOINT_COST_BASE = 2000;
    public const int PROPERTY_BPOINT_COST_EXTRA = 2001;
    public const int PROPERTY_BPOINT_COST_CURRENT = 2002;


    public const int PROPERTY_ATTACK_ATTACKER = 3000;
    public const int PROPERTY_ATTACK_DEFENDER = 3001;
    public const int PROPERTY_MIN_ATTACK_DIS_EXTRA = 3002;
    public const int PROPERTY_MAX_ATTACK_DIS_EXTRA = 3003;
    public const int PROPERTY_PHYSICAL_DAMAGE = 3010;
    public const int PROPERTY_PHYSICAL_DAMAGE_BASE_OUTGOING = 3011;
    public const int PROPERTY_PHYSICAL_DAMAGE_PERCENTAGE = 3012;
    public const int PROPERTY_PHYSICAL_DAMAGE_EXTRA_OUTGOING = 3013;
    public const int PROPERTY_SPELL_DAMAGE = 3020;
    public const int PROPERTY_SPELL_DAMAGE_BASE_OUTGOING = 3021;
    public const int PROPERTY_SPELL_DAMAGE_PERCENTAGE = 3022;
    public const int PROPERTY_SPELL_DAMAGE_EXTRA_OUTGOING = 3023;
    public const int PROPERTY_TOTAL_DAMAGE_REDUCTION = 3040;
    public const int PROPERTY_PHYSICAL_DAMAGE_REDUCTION = 3041;
    public const int PROPERTY_SPELL_DAMAGE_REDUCTION = 3042;
    public const int PROPERTY_DAMAGE_ATTACKER = 3050;
    public const int PROPERTY_DAMAGE_VICTIM = 3051;
    public const int PROPERTY_DAMAGE_REASON = 3060;
    public const int PROPERTY_HP_REMOVAL = 3070;
    public const int PROPERTY_CALC_PHYSICAL_DAMAGE = 3080;
    public const int PROPERTY_CALC_SPELL_DAMAGE = 3081;
    public const int PROPERTY_CALC_HP_REMOVAL = 3082;

    public const int PROPERTY_TRANSLATE_TARGET = 3100;
    public const int PROPERTY_TRANSLATE_REASON = 3101;
    public const int PROPERTY_TRANSLATE_ORIGINAL_ROW = 3110;
    public const int PROPERTY_TRANSLATE_ORIGINAL_COL = 3111;
    public const int PROPERTY_TRANSLATE_OFFSET_ROW = 3120;
    public const int PROPERTY_TRANSLATE_OFFSET_COL = 3121;
    public const int PROPERTY_TRANSLATE_TARGET_ROW = 3130;
    public const int PROPERTY_TRANSLATE_TARGET_COL = 3131;
    public const int PROPERTY_TRANSLATE_PATH = 3140;

    public const int PROPERTY_SUMMONING_UNIT = 3200;
    public const int PROPERTY_SUMMONING_POS = 3201;
    public const int PROPERTY_SUMMONING_REASON = 3202;

    public const int CODE_CHECK_BPOINT_COST_BY_ATTACK = 1;
    public const int CODE_SELECT_ATTACK_TARGET = 2;
    public const int CODE_PRE_DAMAGE = 3;
    public const int CODE_DAMAGE = 4;
    public const int CODE_CAlC_DAMAGE_ATTACKER = 5;
    public const int CODE_CAlC_DAMAGE_VICTIM = 6;
    /// <summary>
    ///  受到伤害时
    /// </summary>
    public const int CODE_TAKE_DAMAGE = 7;
    /// <summary>
    /// 造成伤害时
    /// </summary>
    public const int CODE_DEAL_DAMAGE = 8;
    /// <summary>
    /// 发生位移时
    /// </summary>
    public const int CODE_TRANSLATE = 9;
    /// <summary>
    /// 被选为攻击对象时
    /// </summary>
    public const int CODE_FLAG_ATTACK_TARGET = 10;
    /// <summary>
    /// 执行反击前
    /// </summary>
    public const int CODE_PRE_COUNTER_ATTACK = 11;
    /// <summary>
    /// 召唤单位成功时
    /// </summary>
    public const int CODE_SUMMON_UNIT_SUCCESS = 12;

    public const int DAMAGE_REASON_ATTACK = 1;
    public const int DAMAGE_REASON_COUNTER_ATTACK = 2;
    public const int DAMAGE_REASON_SPELL = 3;
    public const int DAMAGE_REASON_BUFF = 4;

    public const int LUA_OPERATION_SUCCESS = 1;
    public const int LUA_OPERATION_FAIL = 0;

    /// <summary>
    /// 单位执行动作成功
    /// </summary>
    public const int UNIT_ACTION_SUCCESS = 1;
    /// <summary>
    /// 单位执行动作失败
    /// </summary>
    public const int UNIT_ACTION_FAIL = 0;

    public const int PARAM_TYPE_INT = 0x01;
    public const int PARAM_TYPE_BOOLEAN = 0x02;
    public const int PARAM_TYPE_STRING = 0x04;
    public const int PARAM_TYPE_VO = 0x10;
    public const int PARAM_TYPE_UNIT = 0x20;
    public const int PARAM_TYPE_EFFECT = 0x40;
    public const int PARAM_TYPE_EVENT = 0x80;

    /// <summary>
    /// 玩家1队伍id
    /// </summary>
    public const int TeamId_Player1 = 0;
    /// <summary>
    /// 玩家2队伍id
    /// </summary>
    public const int TeamId_Player2 = 1;
    /// <summary>
    /// 默认B点
    /// </summary>
    public const int DEFAULT_BPOINT = 8;
    /// <summary>
    /// 默认每回合可以使用的召唤次数
    /// </summary>
    public const int DEFAULT_MAX_SUMMONING_COUNT_PER_TURN = 1;

    public const int PROCESSOR_PROCESSING = 1;
    public const int PROCESSOR_IDLE = 0;

    /// <summary>
    /// 重置，新回合，暂时意义不明
    /// </summary>
    public const int RESET_TYPE_SELF_TURN_START = 1;
    public const int RESET_TYPE_OPPO_TURN_START = 2;
    public const int RESET_TYPE_SELF_TURN_END = 3;
    public const int RESET_TYPE_OPPO_TURN_END = 4;
    public const int RESET_TYPE_TURN_START = 5;
    public const int RESET_TYPE_TURN_END = 6;

    public const string UNIT_TYPE_NORMAL = "Normal";
    public const string UNIT_TYPE_GIRL = "Girl";

    public const int CELL_TYPE_UNIT = 1;
    public const int CELL_TYPE_CARD = 2;
    /// <summary>
    /// 单位池最大单位数量
    /// </summary>
    public const int MAX_UNIT_POOL_COUNT = 6;

    public static int[][] DEFAULT_SUMMONING_UNIT_POS = new int[2][]{ new int[]{0,12,24,36,48,60,72,84},new int[]{11,23,35,47,59,71,83,95} };
}

