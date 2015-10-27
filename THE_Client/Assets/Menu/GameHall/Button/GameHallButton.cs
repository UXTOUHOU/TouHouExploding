using UnityEngine;
using System.Collections;
public class GameHallButton : MonoBehaviour
{

	public void OnButtonCreateGame()
	{
	}
	public void OnButtonAddRoom()
	{
		//test
		Application.LoadLevel("SelectDeck");
		//
	}
	public void OnButtonReturnMainMenu()
	{
		Application.LoadLevel("MainMenu");
	}

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
