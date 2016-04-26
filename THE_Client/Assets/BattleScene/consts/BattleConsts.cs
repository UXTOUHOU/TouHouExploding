
public class BattleConsts
{
	public enum BattleState
	{
		InitGame = 1,                       // 初始化游戏
		TurnStartPhase = 2,                 // 回合开始阶段
		StandbyPhase = 3,                   // 准备阶段
		MainPhase = 4,                      // 主要阶段，等待玩家自己操作
		MainPhase_Idle = 4,                 // 主要阶段的子状态-空闲
		MainPhase_SelectUnitAction = 5,     // 主要阶段的子状态-选择单位行动
		MainPhase_SelectUnitSkill = 6,      // 主要阶段的子状态-选择单位技能
		MainPhase_SelectMovePath = 7,       // 主要阶段的子状态-选择单位移动路径
		MainPhase_MoveUnit = 8,             // 主要阶段的子状态-单位移动
		MainPhase_SelectAttackTarget = 9,   // 主要阶段的子状态-选择攻击目标
		MainPhase_UnitAttack = 10,          // 主要阶段的子状态-单位攻击
		MainPhase_CounterAttack = 11,       // 主要阶段的子状态-反击
		MainPhase_SummoningUnit = 12,       // 召唤单位（选择位置）
		Processing = 13,                    // 处理事件-结果的状态
	}
// <summary>
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

	public enum LineFlag
	{
		Horizon = 1,
		Vertical = 2,
		Cross = 3,
	}

	public enum Property
	{
		PlayerId = 1000,
		UnitId = 1001,
		Unit = 1002,

		BPointCostBase = 2000,
		BPointCostExtra = 2001,
		BPointCostCurrent = 2002,


		AttackAttacker = 3000,
		AttackDefender = 3001,
		MinAttackDisExtra = 3002,
		MaxAttackDisExtra = 3003,
		PhysicalDamage = 3010,
		PhysicalDamageBaseOutgoing = 3011,
		PhysicalDamagePercentage = 3012,
		PhysicalDamageExtraOutgoing = 3013,
		SpellDamage = 3020,
		SpellDamageBaseOutgoing = 3021,
		SpellDamagePercentage = 3022,
		SpellDamageExtraOutgoing = 3023,
		TotalDamageReduction = 3040,
		PhysicalDamageReduction = 3041,
		SpellDamageReduction = 3042,
		DamageAttacker = 3050,
		DamageVictim = 3051,
		DamageReason = 3060,
		HpRemoval = 3070,
		CalcPhysicalDamage = 3080,
		CalcSpellDamage = 3081,
		CalcHpRemoval = 3082,

		TranslateTarget = 3100,
		TranslateReason = 3101,
		TranslateOriginalRow = 3110,
		TranslateOriginalCol = 3111,
		TranslateOffsetRow = 3120,
		TranslateOffsetCol = 3121,
		TranslateTargetRow = 3130,
		TranslateTargetCol = 3131,
		TranslatePath = 3140,

		SummoningUnit = 3200,
		SummoningPos = 3201,
		SummoningReason = 3202,

	}

	public enum Code
	{
		CheckBPointCostByAttack = 1,
		SelectAttackTarget = 2,
		PerDamage = 3,
		Damage = 4,
		CalcDamageAttacker = 5,
		CalcDamageVictim = 6,
		TakeDamage = 7,				// 受到伤害时
		DealDamage = 8,				// 造成伤害时
		Translate = 9,				// 发生位移时
		FlagAttackTarget = 10,		// 被选为攻击对象时
		PreCounterAttack = 11,		// 执行反击前
		SummonUnitSuccess = 12,		// 召唤单位成功时
	}

	public enum DamageReason
	{
		Attack = 1,
		CounterAttack = 2,
		Spell = 3,
		Buff = 4,
	}


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

	public enum ParamType
	{
		Int = 0x01,
		Boolean = 0x02,
		String = 0x04,
		VO = 0x10,
		Unit = 0x20,
		Effect = 0x40,
		Event = 0x80,
	}

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
	public enum ResetType
	{
		SelfTurnStart = 1,
		OppoTurnStart = 2,
		SelfTurnEnd = 3,
		OppoTurnEnd = 4,
		TurnStart = 5,
		TurnEnd = 6,
	}

	public const string UNIT_TYPE_NORMAL = "Normal";
	public const string UNIT_TYPE_GIRL = "Girl";

	public enum CellType
	{
		Unit = 1,
		Card = 2,
    }

	/// <summary>
	/// 单位池最大单位数量
	/// </summary>
	public const int MAX_UNIT_POOL_COUNT = 6;

	public static int[][] DEFAULT_SUMMONING_UNIT_POS = new int[2][]{ new int[]{0,12,24,36,48,60,72,84},new int[]{11,23,35,47,59,71,83,95} };
}

