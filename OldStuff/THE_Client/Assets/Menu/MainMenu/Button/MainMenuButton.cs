using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MainMenuButton : MonoBehaviour {
	public Button btnStart;
	public Button btnGallery;
	public Button btnConfig;
	public Button btnExit;
	public EventSystem eventSys;
	public void OnButtonStartGame()
	{
		Application.LoadLevel("GameHall");
		Debug.Log("StartGame");
	}
	public void OnButtonGallery()
	{
		Debug.Log("Gallery");
	}
	public void OnButtonConfig()
	{
		Application.LoadLevel("Config");
		Debug.Log("Config");
	}
	public void OnButtonExit()
	{
		Application.Quit();
	}
	void OnGUI()
	{
		//按钮弹起时切换到正常状态
		if (Input.GetMouseButtonUp(0))
		{
			btnStart.OnDeselect(new BaseEventData(eventSys));
			btnGallery.OnDeselect(new BaseEventData(eventSys));
			btnConfig.OnDeselect(new BaseEventData(eventSys));
			btnExit.OnDeselect(new BaseEventData(eventSys));
		}
	}
	// Use this for initialization
	void Start () 
	{
		eventSys = GameObject.Find("/EventSystem").GetComponent<EventSystem>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
