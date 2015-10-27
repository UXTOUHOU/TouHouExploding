using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConfigButton : MonoBehaviour {
	public Button btnGameState;
	public Button btnTutorial;
	public Button btnStaff;
	public Button btnReturn;
	public void OnButtonReturn()
	{
		Application.LoadLevel("MainMenu");
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
