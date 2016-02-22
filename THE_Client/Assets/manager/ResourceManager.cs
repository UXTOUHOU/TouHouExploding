using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceManager {

	private static ResourceManager _instance;
	
	public static ResourceManager getInstance()
	{
		if (_instance == null) {
			_instance = new ResourceManager();
		}
		return _instance;
	}

	private Dictionary<string,Dictionary<string,Object>> _dicAsset;
	private int _loadABTotalCount;
	private int _curLoadABCount;

	public void init()
	{
		this._dicAsset = new Dictionary<string, Dictionary<string, Object>> ();
	}

	public void loadAssetBundles(string[] bundleId)
	{
		this._loadABTotalCount = bundleId.Length;
	}

	public IEnumerator cacheAssetBundle(string bundleId)
	{
		if (this._dicAsset.ContainsKey (bundleId)) 
		{
			CommandManager.getInstance ().runCommand (CommandConsts.CommandConsts_LoadBundleComplete, bundleId);
			yield break;
		}
		Debug.Log ("begin cache bundle " + bundleId);
		WWW www = new WWW (Global.AssetBundleRootPath + bundleId + ".assetbundle");
		yield return www;
		if (www.error != null) 
		{
			Debug.Log("Load Bundle Faild " + bundleId + " Error Is " + www.error);
			yield break;
		}
		AssetBundle ab = www.assetBundle;
		AssetBundleConfig cfg = ab.LoadAsset("config",typeof(AssetBundleConfig)) as AssetBundleConfig;
		AssetBundleRequest request;
		Dictionary<string, Object> dic = new Dictionary<string, Object> ();
		int resourceCount = cfg.resourceVOs.Count;
		CommandManager.getInstance ().runCommand (CommandConsts.CommandConsts_UpdateLoadingBundleProgress, bundleId, 0, resourceCount);
		//System.DateTime time = System.DateTime.Now;
		ResourceVO vo;
		for (int i=0; i<resourceCount; i++) 
		{
			vo = cfg.resourceVOs[i];
			request = ab.LoadAssetAsync(vo.resId,typeof(Object));
			yield return request;
			dic.Add(vo.resId,request.asset);
			CommandManager.getInstance ().runCommand (CommandConsts.CommandConsts_UpdateLoadingBundleProgress, bundleId, i+1, resourceCount);
		}
		this._dicAsset.Add (bundleId, dic);
		//Debug.Log("load asset      " +(System.DateTime.Now-time).TotalMilliseconds);
		ab.Unload(false);
		CommandManager.getInstance ().runCommand (CommandConsts.CommandConsts_LoadBundleComplete, bundleId);
		Debug.Log ("cache bundle complete " + bundleId);
	}

	/*public IEnumerator cacheAssetBundles(List<string> names)
	{

	}*/

	public bool isAssetCached(string assetBundleName)
	{
		return this._dicAsset.ContainsKey (assetBundleName);
	}

	public GameObject loadPrefab(string prefabName)
	{
		Dictionary<string,Object> dic;
		if ( this._dicAsset.TryGetValue(prefabName,out dic) )
		{
			Object instance;
			if ( dic.TryGetValue(prefabName,out instance) )
			{
				return (GameObject)instance;
			}
		}
		return null;
	}

	public Object getRes(string assetBundleName,string resId)
	{
		Dictionary<string,Object> dic;
		if (this._dicAsset.TryGetValue (assetBundleName, out dic)) 
		{
			Object res;
			if (dic.TryGetValue (resId, out res)) 
			{
				return res;
			}
		}
		return null;
	}
}
