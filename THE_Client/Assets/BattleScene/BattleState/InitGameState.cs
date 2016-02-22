using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class InitGameState : IState
{
    public InitGameState()
    {

    }

    public void onStateEnter()
    {
        BattleSceneMain.getInstance().chessboard.init();
    }

    public void onStateExit()
    {
    }

    public void update()
    {
    }
}

