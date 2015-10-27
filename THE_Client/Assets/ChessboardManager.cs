using UnityEngine;
using System.Collections;

public class ChessboardManager : MonoBehaviour {
	public GameObject chessboardPanel;

	private 
	// Use this for initialization
	void Start () {
		RectTransform rt = chessboardPanel.GetComponent<RectTransform>();
		Rect chessboardRect = new Rect(chessboardPanel.transform.localPosition.x, chessboardPanel.transform.localPosition.y,
			Screen.width * (rt.anchorMax.x - rt.anchorMin.x), Screen.height * (rt.anchorMax.y - rt.anchorMin.y));
		Debug.Log(chessboardRect);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
