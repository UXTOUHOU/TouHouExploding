using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MainPhaseState : IState
{
    public MainPhaseState()
    {

    }

    public void onStateEnter()
    {
        //添加点击事件
        BattleSceneMain.getInstance().chessboard.addClickEventHandler(this.cellClickEventHandler);
    }

    public void onStateExit()
    {

    }

    public void update()
    {

    }

    public void cellClickEventHandler(GameObject go)
    {
        Cell cell = go.GetComponent<Cell>();
    }
}