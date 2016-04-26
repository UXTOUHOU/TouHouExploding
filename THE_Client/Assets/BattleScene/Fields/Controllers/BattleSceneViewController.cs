using UnityEngine;
using System.Collections;

public class BattleSceneViewController : BaseViewController
{
	private GameObject _giveUpBtn;
	private GameObject _endTurnBtn;
	private GameObject _backToMenuBtn;

	private GameObject _myUnitPoolBtn;
	private GameObject _myUnitPoolBtnsContainer;
	/// <summary>
	/// 查看我的单位池-按钮
	/// </summary>
	private GameObject _checkMyUnitPoolBtn;
	/// <summary>
	/// 召唤
	/// </summary>
	private GameObject _summonUnitBtn;
	/// <summary>
	/// 是否显示单位池的操作列表
	/// </summary>
	private bool _isShowMyUnitPoolBtnContainer;

	// Use this for initialization
	void Start ()
	{
		// 战场区域
		Chessboard battleField = this.transform.FindChild("BattleField").GetComponent<Chessboard>();
		BattleGlobal.Core.chessboard = battleField;
		// 手牌区域（暂时也用作单位池）
		// 获取组件
		this._giveUpBtn = this.transform.FindChild("ButtonGiveUp").gameObject;
		this._endTurnBtn = this.transform.FindChild("ButtonEndTurn").gameObject;
		this._backToMenuBtn = this.transform.FindChild("ButtonPauseMenu").gameObject;
		this._myUnitPoolBtn = this.transform.FindChild("ButtonUnitPool").gameObject;
		this._myUnitPoolBtnsContainer = this.transform.FindChild("MyUnitPoolBtnContainer").gameObject;
		this._checkMyUnitPoolBtn = this._myUnitPoolBtnsContainer.transform.FindChild("ButtonCheck").gameObject;
		this._summonUnitBtn = this._myUnitPoolBtnsContainer.transform.FindChild("ButtonSummon").gameObject;
		this.addListeners();
		// 初始化一些变量
		this.showMyUnitPoolBtns(false);
		// 进入初始游戏状态
		BattleStateManager.getInstance().setState(BattleConsts.BattleState.InitGame);
	}

	void Update()
	{
		if ( Input.GetMouseButtonDown(1) )
		{
			if ( this._isShowMyUnitPoolBtnContainer )
			{
				this.showMyUnitPoolBtns(false);
			}
		}
	}

	private void addListeners()
	{
		UIEventListener.Get(this._giveUpBtn).onClick += this.giveUpBtnHandler;
		UIEventListener.Get(this._endTurnBtn).onClick += this.endTurnBtnHandler;
		UIEventListener.Get(this._backToMenuBtn).onClick += this.backToMenuBtnHandler;
		UIEventListener.Get(this._myUnitPoolBtn).onClick += this.myUnitPoolBtnHandler;
		UIEventListener.Get(this._checkMyUnitPoolBtn).onClick += this.checkMyUnitPoolBtnHandler;
		UIEventListener.Get(this._summonUnitBtn).onClick += this.SummonUnitBtnHandler;
	}

	private void giveUpBtnHandler(GameObject go)
	{

	}

	private void endTurnBtnHandler(GameObject go)
	{

	}

	private void backToMenuBtnHandler(GameObject go)
	{

	}

	/// <summary>
	/// 点击单位池
	/// </summary>
	/// <param name="go"></param>
	private void myUnitPoolBtnHandler(GameObject go)
	{
		if ( !this._isShowMyUnitPoolBtnContainer )
		{
			this._isShowMyUnitPoolBtnContainer = true;
			this.showMyUnitPoolBtns(true);
		}
	}

	/// <summary>
	/// 显示单位池的操作按钮
	/// </summary>
	/// <param name="isShow"></param>
	private void showMyUnitPoolBtns(bool isShow)
	{
		this._isShowMyUnitPoolBtnContainer = isShow;
		this._myUnitPoolBtnsContainer.SetActive(isShow);
		// todo:根据条件判断是否显示查看、召唤按钮
		if ( isShow )
		{
			if ( BattleGlobal.Core.battleInfo.isSummoningOpAvailabel )
			{
				this._summonUnitBtn.SetActive(true);
			}
			else
			{
				this._summonUnitBtn.SetActive(false);
			}
		}
	}

	private void checkMyUnitPoolBtnHandler(GameObject go)
	{
		this.showMyUnitPoolBtns(false);
		CommandManager.getInstance().runCommand(CommandConsts.PopUpWindow, WindowName.UNIT_POOL_VIEW, BattleGlobal.MyPlayerId, true);
	}

	private void SummonUnitBtnHandler(GameObject go)
	{
		this.showMyUnitPoolBtns(false);
		CommandManager.getInstance().runCommand(CommandConsts.PopUpWindow, WindowName.UNIT_POOL_VIEW, BattleGlobal.MyPlayerId, false);
	}
}
