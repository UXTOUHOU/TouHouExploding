using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class gridCellSize : MonoBehaviour {
	public GameObject cardListPanel;
	public GameObject cardListInner;
	public GameObject deckListPanel;
	public GameObject deckListInner;
	// Use this for initialization
	void Start () {
		cardListPanel = GameObject.Find("/Canvas/CardList");
		cardListInner = GameObject.Find("/Canvas/CardList/Inner");
		//根据分辨率改变cardList cellSize
		GridLayoutGroup grid =	cardListInner.GetComponent<GridLayoutGroup>();
		RectTransform panelRect = cardListPanel.GetComponent<RectTransform>();
		Vector2 panelSize = new Vector2(Screen.width * (panelRect.anchorMax.x - panelRect.anchorMin.x), 
			Screen.height * (panelRect.anchorMax.y - panelRect.anchorMin.y));
		panelSize.y = panelSize.x * 4 / 3;
		if (grid.cellSize.x < panelSize.x / 4)
			panelSize /= 4;		//每行排4个
		else
			panelSize /= 3;		//每行排3个
		panelSize.x -= 0.5F;	//防止误差导致每行最后一个cell被排到下一行
		grid.cellSize = panelSize;
		//滚动到最上
		cardListPanel.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
		deckListPanel.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
