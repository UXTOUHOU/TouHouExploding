using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadManager : MonoBehaviour,ICommand
{
	private static LoadManager _instance;

	public static LoadManager getInstance()
	{
		return _instance;
	}

	void Awake()
	{
		_instance = this;
		this._bundleToGroupDic = new Dictionary<string, List<string>> ();
		this._loadBundleVOMap = new Dictionary<string, LoadBundleVO> ();
		GameObject.DontDestroyOnLoad (this);
		this.init ();
	}

	private void init()
	{
		CommandManager.getInstance ().addCommand (CommandConsts.CommandConsts_UpdateLoadingBundleProgress, this);
		CommandManager.getInstance ().addCommand (CommandConsts.CommandConsts_LoadBundleComplete, this);
		List<string> loadSceneResList = new List<string> ();
		//loadSceneResList.Add (WindowName.LoadingView);
		//this._loadSceneResList = loadSceneResList.ToArray ();
	}

	private int MaxLoadingBundlesCount = 3;

	private int _curLoadIndex;
	private int _nextLoadIndex;
	private int _curLoadedCount;
	/**当前正在加载的数目*/
	private int _curLoadingCount;
	private string[] _loadList;
	private int _loadTotalCount;
	private Dictionary<string,List<string>> _bundleToGroupDic;
	private Dictionary<string,LoadBundleVO> _loadBundleVOMap;
	/**将要加载的场景名称*/
	private string _nextScene;

	public delegate void Callback();
	private Callback _loadCompleteCallback;
	/**加载场景模块需要的资源*/
	private string[] _loadSceneResList;
	/**下次需要加载的资源*/
	private string[] _nextLoadBundleIds;

	public void loadNextScene(string sceneName,string[] bundleIds)
	{
		this._nextScene = sceneName;
		this._nextLoadBundleIds = bundleIds;
		this.loadAssetBundles (this._loadSceneResList, this.loadLoadSceneResCallback);
		//Application.LoadLevel ("LoadScene");
		//this.LoadAssetBundles (bundleIds);
	}

	public void loadAssetBundles(string[] bundleIds,Callback callback)
	{
		this._loadList = bundleIds;
		this._curLoadIndex = 0;
		this._nextLoadIndex = 0;
		this._loadTotalCount = bundleIds.Length;
		this._loadCompleteCallback = callback;
		while (this._curLoadingCount < this._loadTotalCount && this._nextLoadIndex < this._loadTotalCount )
		{
			this.loadNextBundle();
		}
	}

	/*public void LoadAssetBundles(string[] bundleIds,string groupName="")
	{
		this._loadList = bundleIds;
		this._curLoadIndex = 0;
		this._nextLoadIndex = 0;
		this._loadTotalCount = bundleIds.Length;
		this._loadCompleteCallback = this.loadNextLevel;
		while (this._curLoadingCount < this._loadTotalCount && this._nextLoadIndex < this._loadTotalCount )
		{
			this.loadNextBundle();
		}
		this._nextLoadBundleIds = bundleIds;
		this.loadAssetBundles (bundleIds, this.loadNextLevel);
	}*/

	private void loadNextBundle()
	{
		this._curLoadingCount++;
		this.StartCoroutine (ResourceManager.getInstance ().cacheAssetBundle (this._loadList[this._nextLoadIndex++]));
	}

	public void recvCommand(int cmdNum,object[] args)
	{
		switch ( cmdNum ) 
		{
		case CommandConsts.CommandConsts_LoadAllBundlesComplete:
			break;
		case CommandConsts.CommandConsts_UpdateLoadingBundleProgress:
			this.updateLoadingProgress(args[0].ToString(),(int)args[1],(int)args[2]);
			break;
		case CommandConsts.CommandConsts_LoadBundleComplete:
			this.onLoadBundleComplete(args[0].ToString());
			break;
		}
	}

	private void updateLoadingProgress(string bundleId,int curLoadedCount,int totalCount)
	{
		LoadBundleVO vo;
		if (this._loadBundleVOMap.TryGetValue (bundleId, out vo)) 
		{
			vo.curLoadedResCount = curLoadedCount;
			this.calculateProgress ();
		} 
		else 
		{
			vo = new LoadBundleVO();
			vo.curLoadedResCount = curLoadedCount;
			vo.totalResCount = totalCount;
			this._loadBundleVOMap.Add(bundleId,vo);
		}
	}

	private void calculateProgress()
	{
		float progress = 0;
		float per = 1.0f / this._loadTotalCount;
		foreach (LoadBundleVO vo in this._loadBundleVOMap.Values)
		{
			progress += per * vo.curLoadedResCount / vo.totalResCount;
		}
		progress = Mathf.FloorToInt (progress * 100f) / 100.0f;
		Debug.Log (progress);
		CommandManager.getInstance ().runCommand (CommandConsts.CommandConsts_UpdateLoadingProgress, progress);
	}

	private void onLoadBundleComplete(string bundleId)
	{
		this._curLoadingCount--;
		if (this._nextLoadIndex < this._loadTotalCount) {
			this.loadNextBundle ();
		}
		// 全部加载完成
		else if (this._curLoadingCount == 0) 
		{
			CommandManager.getInstance().runCommand(CommandConsts.CommandConsts_LoadAllBundlesComplete);
			if ( this._loadCompleteCallback != null )
			{
				this._loadCompleteCallback();
			}
		}
	}

	private void loadLoadSceneResCallback()
	{
		Application.LoadLevel ("LoadScene");
		this.loadAssetBundles (this._nextLoadBundleIds,this.loadNextLevel);
	}

	private void loadNextLevel()
	{
		Application.LoadLevel(this._nextScene);
	}
}

internal class LoadBundleVO
{
	public string groupName;
	public int totalResCount;
	public int curLoadedResCount;
}
