using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class MainPhaseSelectUnitActionState : IState, ICommand
{
    private IFSM _fsm;

    private bool _isInit;

    private GameObject _unitActionView;

    private GameObject _btnMove;
    private GameObject _btnAttack;
    private GameObject _btnSkill;

    private bool _isPopUp;

    public MainPhaseSelectUnitActionState(IFSM fsm)
    {
        this._fsm = fsm;
        this._isInit = false;
        this._isPopUp = false;
    }

    private void init()
    {
        this._unitActionView = ResourceManager.getInstance().loadPrefab("Prefabs/UnitActionView");
        this._btnMove = this._unitActionView.transform.FindChild("ButtonMove").gameObject;
        this._btnAttack = this._unitActionView.transform.FindChild("ButtonAttack").gameObject;
        this._btnSkill = this._unitActionView.transform.FindChild("ButtonSkill").gameObject;
    }

    public void onStateEnter()
    {
        if (!this._isInit)
        {
            this.init();
        }
        this.showUnitActionView();
        OperationManager.getInstance().setOperation(BattleConsts.CellOp_Idle);
        CommandManager.getInstance().addCommand(BattleConsts.CMD_OnCellSelected, this);
    }

    public void onStateExit()
    {
        this.removeUnitActionView();
        CommandManager.getInstance().removeCommand(BattleConsts.CMD_OnCellSelected, this);
    }

    public void update()
    {
        if (Input.GetMouseButton(1))
        {
            this._fsm.setState(BattleConsts.MainPhaseSubState_Idle);
        }
    }

    public void recvCommand(int cmd, params object[] args)
    {
        switch (cmd)
        {
            case BattleConsts.CMD_OnCellSelected:
                this.showUnitActionView();
                break;
            default:
                break;
        }
    }

    private void showUnitActionView()
    {
        Cell cell = BattleGlobal.SelectedCell;
        BattleSceneMain.getInstance().chessboard.addChildOnLayer(this._unitActionView, BattleConsts.BattleFieldLayer_UI, cell.location.y, cell.location.x);
        if (!this._isPopUp)
        {
            this._unitActionView.SetActive(true);
            UIEventListener.Get(this._btnMove).onClick += this.btnMoveClickHander;
            UIEventListener.Get(this._btnAttack).onClick += this.btnAttackClickHander;
            UIEventListener.Get(this._btnSkill).onClick += this.btnSkillClickHander;
            this._isPopUp = true;
        }
    }

    private void removeUnitActionView()
    {
        UIEventListener.Get(this._btnMove).onClick -= this.btnMoveClickHander;
        UIEventListener.Get(this._btnAttack).onClick -= this.btnAttackClickHander;
        UIEventListener.Get(this._btnSkill).onClick -= this.btnSkillClickHander;
        this._unitActionView.SetActive(false);
        this._isPopUp = false;
    }

    private void btnMoveClickHander(GameObject go)
    {

    }

    private void btnAttackClickHander(GameObject go)
    {
    }

    private void btnSkillClickHander(GameObject go)
    {
    }
}
