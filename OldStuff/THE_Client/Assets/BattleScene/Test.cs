using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	public Sprite btnNormal;
	public Sprite btnHavor;
	public Sprite btnActive;

	void Start(){
		//button = gameObject.GetComponent<SpriteRenderer>();
	}


	void OnMouseExit(){
		//button.sprite = btnNormal;
	}


	public void OnMouseDown()
	{
		Debug.Log("tc");
	}
	void OnMouseOver()
	{
		//Debug.Log("Over");
		if (Input.GetMouseButton(0))
		{
			//button.sprite = btnActive;
		}
		else
		{
			//button.sprite = btnHavor;
		}

		if (Input.GetMouseButtonDown(0))
		{
			Debug.Log("Press");
		}

		if (Input.GetMouseButtonUp(0))
		{
			Debug.Log("ClickBtn");
		}
	}
}
