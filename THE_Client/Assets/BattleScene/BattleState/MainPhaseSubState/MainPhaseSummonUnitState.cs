using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MainPhaseSummonUnitState : BattleStateBase
{
    private int[] _summoningPosList;

    public MainPhaseSummonUnitState(IFSM fsm)
        :base(fsm)
    {
        this._summoningPosList = new int[BattleConsts.MapMaxRow * BattleConsts.MapMaxCol];
    }

    public override void onStateEnter()
    {
        Debug.Log("Enter State : SummonUnit");
        // 获取当前可召唤的位置
        this.getSummoningPosByPlayerId(BattleGlobal.MyPlayerId, this._summoningPosList);
        // 显示当前可召唤的位置
        Chessboard.showRangeByRangeList(this._summoningPosList);
        // 添加点击事件
        Chessboard.addClickEventHandler(this.onCellClick);
    }

    public override void onStateExit()
    {
        Chessboard.activeRangeShow(false);
        Chessboard.removeClickEventHandler(this.onCellClick);
    }

    public override void update()
    {
        if ( Input.GetMouseButtonDown(1) )
        {
            this._fsm.setState(BattleConsts.BattleState.MainPhase_Idle);
        }
    }

    private void onCellClick(GameObject go)
    {
        Cell cell = go.GetComponent<Cell>();
        int pos = cell.posIndex;
        if ( cell != null && this._summoningPosList[pos] == 1)
        {
            if ( cell.IsCanSummonPlace() )
            {
                int playerId = BattleGlobal.MyPlayerId;
                // 召唤单位
                string unitId = BattleGlobal.Core.getPlayer(playerId).summonUnit(BattleGlobal.Core.battleInfo.summoningUnitIndex);
                if ( unitId != "" )
                {
                    Unit unit = new Unit(unitId);
                    unit.summon(pos, playerId, playerId);
                    // 召唤单位事件
                    // todo : summonReason
                    EventVOBase evtVO = BattleObjectFactory.createEventVO(BattleConsts.Code.SummonUnitSuccess);
                    evtVO.setProperty(BattleConsts.Property.SummoningUnit, unit);
                    evtVO.setProperty(BattleConsts.Property.SummoningPos, pos);
                    BattleEventBase evt = BattleObjectFactory.createBattleEvent(BattleConsts.Code.SummonUnitSuccess, evtVO);
                    ProcessManager.getInstance().raiseEvent(evt);
                    // 设置下一个状态
                    BattleGlobal.Core.battleInfo.nextState = BattleConsts.BattleState.MainPhase_Idle;
                    this._fsm.setState(BattleConsts.BattleState.Processing);
                }
                else
                {
                    Debug.Log("Summon Unit Fail ! Reason : unitId is not exist!");
                    this._fsm.setState(BattleConsts.BattleState.MainPhase_Idle);
                }
            }
        }
    }

    private int[] getSummoningPosByPlayerId(int playerId,int[] summoningPos=null)
    {
        int rowLimit = BattleConsts.MapMaxRow;
        int colLimit = BattleConsts.MapMaxCol;
        int i, j;
        if ( summoningPos == null )
        {
            summoningPos = new int[rowLimit*colLimit];
        }
        // 重置
        for (i=0;i<rowLimit;i++)
        {
            for (j=0;j<colLimit;j++)
            {
                summoningPos[i * colLimit + j] = 0;
            }
        }
        // 设置默认的召唤点
        int[] defaultPos = BattleConsts.DEFAULT_SUMMONING_UNIT_POS[playerId];
        int defaultCount = defaultPos.Length;
        for (i=0;i<defaultCount;i++)
        {
            summoningPos[defaultPos[i]] = 1;
        }
        // todo: 根据效果添加额外的召唤点
        return summoningPos;
    }
}
