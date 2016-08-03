using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class AssetBundleMaker : EditorWindow {

	[MenuItem("YKTools/AssetBundle Maker")]
	static void AddWindow()
	{
		Rect windowRect = new Rect (0, 0, 500, 500);
		AssetBundleMaker window = (AssetBundleMaker)EditorWindow.GetWindowWithRect (typeof(AssetBundleMaker), windowRect, true);
		window.Show ();
	}

	private string TempCfgPath = "Assets/StreamingAssets/config.asset";

	private int PlatformPC = 0;
	private int PlatformAndroid = 1;
	private int PlatformIPhone = 2;

	private int _usePlatform = 0;

	void OnGUI()
	{
		if (GUILayout.Button ("PC上使用", GUILayout.Width (200))) {
			this._usePlatform = this.PlatformPC;
		}
		if (GUILayout.Button ("Android上使用", GUILayout.Width (200))) {
			this._usePlatform = this.PlatformAndroid;
		}
		if (GUILayout.Button ("IPhone上使用", GUILayout.Width (200))) {
			this._usePlatform = this.PlatformIPhone;
		}
		EditorGUILayout.LabelField ("当前选择的使用平台  ：", this.getCurPlatformStr ());
		if (GUILayout.Button ("分别打包", GUILayout.Width (200))) {
			this.createAssetBundlesWithSelections();
		}
		if (GUILayout.Button ("全部打包", GUILayout.Width (200))) {
			this.createOneAssetBunldeWithSelections();
		}
	}

	private string getCurPlatformStr()
	{
		if (this._usePlatform == this.PlatformPC)
		{
			return "PC";
		}
		else if (this._usePlatform == this.PlatformAndroid) 
		{
			return "Android";
		}
		else if (this._usePlatform == this.PlatformIPhone) 
		{
			return "IPhone";
		}
		return "";
	}

	private void createAssetBundlesWithSelections()
	{
		object[] selectedAssets = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);
		//遍历所有的游戏对象
		foreach (Object obj in selectedAssets) 
		{
			this.buildAssetBundle(obj,null);
		}
		//刷新编辑器
		AssetDatabase.Refresh ();
	}

	private void createOneAssetBunldeWithSelections()
	{
		Object[] selectedAssets = Selection.GetFiltered (typeof(object), SelectionMode.DeepAssets);
		if (this.buildAssetBundle (null, selectedAssets)) {
			//刷新编辑器
			AssetDatabase.Refresh ();
		}
	}

	private bool buildAssetBundle(Object mainAsset,Object[] assets)
	{
		bool buildSuccess = false;
		//string sourcePath = AssetDatabase.GetAssetPath (mainAsset);
		//本地测试：建议最后将Assetbundle放在StreamingAssets文件夹下，如果没有就创建一个，因为移动平台下只能读取这个路径
		//StreamingAssets是只读路径，不能写入
		//服务器下载：就不需要放在这里，服务器上客户端用www类进行下载。
		string targetPath;
		string assetName;
		int i,len;
		// 自动生成资源配置文件
		AssetBundleConfig cfg = ScriptableObject.CreateInstance<AssetBundleConfig>();
		ResourceVO vo;
		List<Object> outList = new List<Object> ();
		// 首先检测是否存在StreamingAssets文件夹
		// 打包单个
		if (mainAsset != null) 
		{
			targetPath = Application.dataPath + "/StreamingAssets/" + mainAsset.name + ".assetbundle";
			assetName = mainAsset.name;
			vo = new ResourceVO();
			vo.resId = mainAsset.name;
			cfg.resourceVOs.Add(vo);
			outList.Add(mainAsset);
		} 
		// 打包全部
		else 
		{
			targetPath = Application.dataPath + "/StreamingAssets/" + assets[0].name + ".assetbundle";
			assetName = "所有";
			len = assets.Length;
			for (i=0;i<len;i++)
			{
				vo = new ResourceVO();
				vo.resId = assets[i].name;
				cfg.resourceVOs.Add(vo);
				outList.Add(assets[i]);
			}
		}
		outList.Add(this.saveAsObj(cfg));
		mainAsset = null;
		if (this._usePlatform == this.PlatformPC)
		{
			buildSuccess = BuildPipeline.BuildAssetBundle (null, outList.ToArray(), targetPath, BuildAssetBundleOptions.CollectDependencies);
		}
		else if (this._usePlatform == this.PlatformAndroid) 
		{
			buildSuccess = BuildPipeline.BuildAssetBundle (null, outList.ToArray(), targetPath, BuildAssetBundleOptions.CollectDependencies,BuildTarget.Android);
		}
		else if (this._usePlatform == this.PlatformIPhone) 
		{
			buildSuccess = BuildPipeline.BuildAssetBundle (null, outList.ToArray(), targetPath, BuildAssetBundleOptions.CollectDependencies,BuildTarget.iOS);
		}
		if ( buildSuccess )
		{
			Debug.Log (assetName + "资源打包成功");
		} 
		else 
		{
			Debug.Log(assetName +"资源打包失败");
		}
		this.deleteTempCfgFile ();
		return buildSuccess;
	}

	private Object saveAsObj(Object cfg)
	{
		AssetDatabase.CreateAsset (cfg, this.TempCfgPath);
		Object outObj = AssetDatabase.LoadAssetAtPath (this.TempCfgPath,typeof(AssetBundleConfig));
		return outObj;
	}

	private void deleteTempCfgFile()
	{
		AssetDatabase.DeleteAsset (this.TempCfgPath);
	}
}
