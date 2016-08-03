using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class GameHallManager : MonoBehaviour {
	public UnityEngine.UI.Button btnReturnMainMenu;
	public UnityEngine.UI.Button btnCreateGame;
	public UnityEngine.UI.Button btnAddRoom;
	public EventSystem eventSys;

	void OnGUI()
	{
		btnCreateGame.OnDeselect(new BaseEventData(eventSys));
		btnAddRoom.OnDeselect(new BaseEventData(eventSys));
		btnReturnMainMenu.OnDeselect(new BaseEventData(eventSys));
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
