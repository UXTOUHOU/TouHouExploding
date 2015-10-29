using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Deck : MonoBehaviour 
{
	private static Deck singleton;
	public static Deck GetInstance()
	{
		return singleton;
	}

	public GameObject deckRect;
	public GameObject canvas;
	public GameObject cardPrefab;

	private float deckCardWidth, deckCardHeight;
	private const float cardWidth = 420F, cardHeight = 590F;

	private ArrayList deckCard = new ArrayList();
	public static void AddDeckCard(int cardID)
	{
		Deck deck = singleton;
		GameObject card = Instantiate(deck.cardPrefab.transform.FindChild("Card").gameObject);
		card.name = "Card_" + cardID.ToString().PadLeft(4, '0');									//Set name Card_0000
		card.transform.SetParent(deck.canvas.transform);											//Set parent
		card.GetComponent<Image>().sprite = SelectDeck.CreateCardSprite(cardID);					//Set card sprite
		SetCardSize(card, deck.deckCardWidth, deck.deckCardHeight);									//Set size
		deck.deckCard.Add(card);
		deck.SortDeckCard();
	}

	public static void RemoveDeckCard(GameObject card)
	{
		Deck deck = singleton;

		int i = deck.deckCard.IndexOf(card);
		if (i == -1) return;						//没有匹配的卡片
		deck.deckCard.RemoveAt(i);
		Destroy(card);
		deck.SortDeckCard();
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
		singleton = this;

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
