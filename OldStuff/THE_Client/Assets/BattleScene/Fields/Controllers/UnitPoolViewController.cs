﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPoolViewController : BaseViewController
{
	private const int CountPerPage = 3;
	private int _curIndex;

	private List<CardCell> _cardCells;
	private Text _titleText;
	private GameObject _preBtn;
	private GameObject _nextBtn;
	private GameObject _sureBtn;

	/// <summary>
	/// 是否为查看（反之则为召唤
	/// </summary>
	private bool _isCheck;
	/// <summary>
	/// 当前查看的单位池所属的玩家id
	/// </summary>
	private int _playerId;

	private UnitPool _curUnitPool;

	public override void initController()
	{
		this._windowName = WindowName.UNIT_POOL_VIEW;
		this._cardCells = new List<CardCell>();
		CardCell cell;
		Transform containerTrans = this.transform.FindChild("CardCellContainer");
		for (int i=0;i<CountPerPage;i++)
		{
			cell = containerTrans.FindChild("CardCell" + i).GetComponent<CardCell>();
			this._cardCells.Add(cell);
		}
		Transform bgTrans = this.transform.FindChild("Bg");
		this._titleText = bgTrans.FindChild("TitleText").GetComponent<Text>();
		this._preBtn = bgTrans.FindChild("PreBtn").gameObject;
		this._nextBtn = bgTrans.FindChild("NextBtn").gameObject;
		this._sureBtn = bgTrans.FindChild("SureBtn").gameObject;
		base.initController();
	}

	public override void onPopUp(object[] args)
	{
		base.onPopUp(args);
		this._playerId = (int)args[1];
		this._isCheck = (bool)args[2];
		this._curUnitPool = BattleGlobal.Core.getPlayer(this._playerId).unitPool;
		this._curIndex = 0;
		this.updateView();
		this.addListener();
	}

	public override void onClose()
	{
		this.removeListener();
		base.onClose();
	}

	private void addListener()
	{
		UIEventListener.Get(this._preBtn).onClick += this.preBtnHandler;
		UIEventListener.Get(this._nextBtn).onClick += this.nextBtnHandler;
		UIEventListener.Get(this._sureBtn).onClick += this.sureBtnHandler;
		if ( !this._isCheck )
		{
			int i;
			CardCell cell;
			for (i=0;i<CountPerPage;i++)
			{
				cell = this._cardCells[i];
				cell.tmpData = i;
				cell.addClickEventHandler(this.cellClickHandler);
			}
		}
	}

	private void removeListener()
	{
		UIEventListener.Get(this._preBtn).onClick -= this.preBtnHandler;
		UIEventListener.Get(this._nextBtn).onClick -= this.nextBtnHandler;
		UIEventListener.Get(this._sureBtn).onClick -= this.sureBtnHandler;
		if (!this._isCheck)
		{
			int i;
			CardCell cell;
			for (i = 0; i < CountPerPage; i++)
			{
				cell = this._cardCells[i];
				cell.removeClickEventHandler(this.cellClickHandler);
			}
		}
	}

	private void updateView()
	{
		// 更新title
		if ( this._isCheck )
		{
			this._titleText.text = "查看单位池";
			this._sureBtn.SetActive(true);
		}
		else
		{
			this._titleText.text = "选择要召唤的单位";
			this._sureBtn.SetActive(false);
		}
		this.updateCells();
	}

	private void updateCells()
	{
		List<string> curIds = this._curUnitPool.getCurIds();
		int totalCount = curIds.Count;
		int i, j;
		CardCell cell;
		for (i = 0, j = this._curIndex; i < CountPerPage; i++, j++)
		{
			cell = this._cardCells[i];
			if (j < totalCount)
			{
				cell.setActive(true);
				cell.bindCellData(curIds[j], CardCellType.Unit);
				cell.setIndexText("单位池【" + (j + 1) + "】");
			}
			else
			{
				cell.setActive(false);
			}
		}
	}

	private void preBtnHandler(GameObject go)
	{
		if ( this._curIndex != 0 )
		{
			this._curIndex--;
			this.updateCells();
		}
	}

	private void nextBtnHandler(GameObject go)
	{
		if ( this._curIndex + CountPerPage - 1 < BattleConsts.MAX_UNIT_POOL_COUNT - 1)
		{
			this._curIndex++;
			this.updateCells();
		}
	}

	private void sureBtnHandler(GameObject go)
	{
		CommandManager.getInstance().runCommand(CommandConsts.RemoveWindow, this._windowName);
	}

	private void cellClickHandler(object data,bool isSelected)
	{
		int index = (int)data;
		BattleGlobal.Core.battleInfo.summoningUnitIndex = index;
		CommandManager.getInstance().runCommand(CommandConsts.RemoveWindow, this._windowName);
		BattleStateManager.getInstance().setState(BattleConsts.BattleState.MainPhase_SummoningUnit);
	}
}

