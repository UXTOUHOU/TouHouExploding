using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BattleScene;
using System.Threading;

public class BattleSceneMain : MonoBehaviour 
{

    void Awake()
    {
        PopUpManager.getInstance().init();
        BattleCore core = new BattleCore();
        BattleGlobal.Core = core;
    }

	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
        BattleGlobal.Core.update();
	}
}
