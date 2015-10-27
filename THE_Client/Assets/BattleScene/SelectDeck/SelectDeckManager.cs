using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SelectDeckManager : MonoBehaviour {
	public GameObject cardListPrefab;
	public GameObject deckListPrefab;
	public GameObject cardListInner;
	public GameObject deckListInner;
	private Text textDeckCount;

	private const int maxDeckCount = 10;
	private Dictionary<int, int> mapDeckCard = new Dictionary<int, int>();
	private int deckCount = 0;



	private void SetDeckCount(int num)
	{
		if (num < 0 ||
			num > maxDeckCount) return;
		deckCount = num;
		textDeckCount.text = deckCount + "/" + maxDeckCount;
	}
	public bool AddDeckCard(int cardID)
	{
		if (deckCount >= maxDeckCount)
			return false;
		++deckCount;
		SetDeckCount(deckCount);
	
		if (mapDeckCard.ContainsKey(cardID))
		{//增加卡片计数
			int cardCount = ++mapDeckCard[cardID];
			GameObject card = deckListInner.transform.FindChild("Panel_Card_" + cardID.ToString("d4")).gameObject;
			card.transform.FindChild("CardCount").GetComponent<Text>().text = "×" + cardCount;
		}
		else 
		{//添加卡片
			mapDeckCard.Add(cardID, 1);
			GameObject newCell = Instantiate(deckListPrefab.transform.FindChild("Panel").gameObject);
			newCell.name = "Panel_Card_" + cardID.ToString("d4");
			newCell.transform.FindChild("Card").GetComponent<Image>().sprite = CreateCardSprite(cardID);
			newCell.transform.SetParent(deckListInner.transform);
		}
		Debug.Log("ADC");
		return true;
	}
	private static Sprite CreateCardSprite(int cardID)
	{
		Texture2D cardTexture = Resources.Load<Texture2D>("Cards/Card_" + cardID.ToString("d4"));
		Sprite cardSprite = Sprite.Create(cardTexture, new Rect(0, 0, cardTexture.width, cardTexture.height), new Vector2(0.5f, 0.5f));
		return cardSprite;
	}
	public bool RemoveDeckCard(int cardID)
	{
		if (deckCount <= 0)
			return false;
		--deckCount;
		SetDeckCount(deckCount);

		GameObject panel = deckListInner.transform.FindChild("Panel_Card_" + cardID.ToString("d4")).gameObject;
		if (mapDeckCard[cardID] <= 1)
		{
			mapDeckCard.Remove(cardID);
			Destroy(panel);
		}
		else
		{
			panel.transform.FindChild("CardCount").GetComponent<Text>().text = "×" + --mapDeckCard[cardID];
		}
		return true;
	}

	// Use this for initialization
	void Start () {
		//Init
		textDeckCount = GameObject.Find("/Canvas/TextDeckCount").GetComponent<Text>();
		//向CardList中添加卡片
		int[] cardID = {1, 2, 3, 6, 11, 13, 19};
		for (int i = 0; i < 7; i++)
		{
			GameObject newCard = Instantiate(cardListPrefab.transform.FindChild("Panel").gameObject);
			newCard.name = "Panel_Card_" + cardID[i].ToString("d4");
			newCard.transform.SetParent(cardListInner.transform);
            string cardName = "Card_" + cardID[i].ToString("d4");
            Texture2D cardTexture = Resources.Load<Texture2D>("Cards/" + cardName);
			Sprite cardSprite = Sprite.Create(cardTexture, new Rect(0, 0, cardTexture.width, cardTexture.height), new Vector2(0.5f, 0.5f));
			newCard.transform.FindChild("Card").GetComponent<Image>().sprite = cardSprite;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
