using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

public class ProcessManager
{
    private static ProcessManager _instance;

    public static ProcessManager getInstance()
    {
        if (_instance == null)
        {
            _instance = new ProcessManager();
        }
        return _instance;
    }

    /// <summary>
    /// 上一步产生的结果列表
    /// </summary>
    private List<IBattleResult> _curResultList;

    private List<IBattleResult> _nextResultList;

    private List<BattleEventBase> _curEventList;

    private List<BattleEventBase> _nextEventList;

    private bool _isProcessing;

    private int _processingEventsIndex;
    private int _processingEffectsIndex;
    private List<ISkillEffect> _processingEffects;

    public delegate void ProcessCompleteHandler();
    private ProcessCompleteHandler _onProcessComplete;

    private bool _isGettingStarted;

    public bool isProcessing
    {
        get { return this._isProcessing; }
    }

    public ProcessManager()
    {
        this._curResultList = new List<IBattleResult>();
        this._nextResultList = new List<IBattleResult>();
        this._curEventList = new List<BattleEventBase>();
        this._nextEventList = new List<BattleEventBase>();
        this._isProcessing = false;
        this._isGettingStarted = false;
    }

    public void addResult(IBattleResult result)
    {
        this._nextResultList.Add(result);
    }

    public bool excuteResults()
    {
        int resultCount = this._curResultList.Count;
        if (resultCount > 0)
        {
            IBattleResult result;
            for (int i=0;i<resultCount;i++)
            {
                result = this._curResultList[i];
                result.execute();
            }
            Debug.Log("Execute results complete!");
            this._curResultList.Clear();
            return true;
        }
        return false;
    }

    public void raiseEvent(BattleEventBase battleEvent)
    {
        this._nextEventList.Add(battleEvent);
    }

    public bool processInstantEvent()
    {
        int eventCount = this._curEventList.Count;
        List<ISkillEffect> effects;
        if (eventCount > 0)
        {
            BattleEventBase evt;
            for (int i = 0; i < eventCount; i++)
            {
                evt = this._curEventList[i];
                Debug.Log("processing event : " + evt.getEventName());
                effects = evt.getTriggerEffects();
                for (int j = 0; j < effects.Count; j++)
                {
                    InterpreterManager.getInstance().addParam(evt.getEventVO(), BattleConsts.ParamType.VO);
                    InterpreterManager.getInstance().callFunction(effects[j].getOperation(), 1);
                }
            }
            Debug.Log("process events complete!");
            this._curEventList.Clear();
            return true;
        }
        return false;
    }

    public void startProcess(ProcessCompleteHandler callback=null)
    {
        if ( !this._isProcessing )
        {
            if ( callback != null )
            {
                this._onProcessComplete += callback;
            }
            BattleGlobal.Core.battleInfo.isProcessingComplete = false;
            this._isGettingStarted = true;
        }
    }

    private void process()
    {
        if (this._isProcessing)
        {
            return;
        }
        this._isProcessing = true;
        bool flag0, flag1;
        while (true)
        {
            this._curResultList.AddRange(this._nextResultList);
            this._nextResultList.Clear();
            this._curEventList.AddRange(this._nextEventList);
            this._nextEventList.Clear();
            flag0 = this.excuteResults();
            flag1 = this.processInstantEvent();
            if (!flag0 && !flag1)
            {
                break;
            }
        }
        this._isProcessing = false;
        if ( this._onProcessComplete != null )
        {
            this._onProcessComplete();
        }
        BattleGlobal.Core.battleInfo.isProcessingComplete = true;
        this._onProcessComplete = null;
        Debug.Log("Processing All Complete!");
    }

    public void threadUpdate()
    {
        while ( true )
        {
            if ( this._isGettingStarted )
            {
                this._isGettingStarted = false;
                this.process();
            }
            Thread.Sleep(16);
        }
    }
}