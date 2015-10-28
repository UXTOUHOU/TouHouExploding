using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Deck : MonoBehaviour 
{
	public GameObject deckRect;
	public GameObject canvas;

	private float deckCardWidth, deckCardHeight;
	private const float cardWidth = 420F, cardHeight = 590F;

	private ArrayList deckCard = new ArrayList();
	public void AddDeckCard(int cardID)
	{
		GameObject card = new GameObject();
		card.transform.SetParent(canvas.transform);
		card.AddComponent<Image>().sprite = SelectDeckManager.CreateCardSprite(cardID);
		SetCardSize(card, deckCardWidth, deckCardHeight);
		deckCard.Add(card);
		SortDeckCard();
	}

	private static void SetCardSize(GameObject card, float width, float height)
	{
		var rectTransform = card.GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(width, height);
	}

	//改变手牌数目之后重新排列卡片位置
	private void SortDeckCard()
	{
		int cardNum = deckCard.Count;
		Vector3 deckPosition = deckRect.transform.localPosition;
		deckPosition.x -= deckCardWidth * (cardNum / 2F - 0.5F);
		foreach (GameObject it in deckCard)
		{
			it.transform.localPosition = deckPosition;
			deckPosition.x += deckCardWidth;
		}
	}
	// Use this for initialization
	void Start () 
	{
		//初始化参数
		var anchor = deckRect.GetComponent<RectTransform>();
		deckCardHeight = (anchor.anchorMax.y - anchor.anchorMin.y) * Screen.height;
		deckCardWidth = deckCardHeight * cardWidth / cardHeight;

		//加入手牌测试
		AddDeckCard(6);
		AddDeckCard(11);
		AddDeckCard(13);
		//
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
