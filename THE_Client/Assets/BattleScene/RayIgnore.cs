using UnityEngine;
using System.Collections;

public class RayIgnore : MonoBehaviour, ICanvasRaycastFilter

{

	public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
	{
		Debug.Log("t");
		return true;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
