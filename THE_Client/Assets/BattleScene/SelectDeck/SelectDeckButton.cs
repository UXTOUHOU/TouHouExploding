using UnityEngine;
using System.Collections;

public class SelectDeckButton : MonoBehaviour {
	public void OnButtonConfirm()
	{
		Application.LoadLevel("BattleScene");
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
