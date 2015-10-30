using UnityEngine;
using System.Collections;

public class ClickCard : MonoBehaviour {
	public GameObject ClickedCard;
	private SelectDeck deckManager;

	//标记点击的卡片在哪个panel中
	public enum CardPanel
	{
		DeckCard,
		CardList
	}
	public CardPanel CardBelong;

	public void OnClickDeckCard()
	{
		Debug.Log("ClickDeckCard");
		int cardID = ParsePanelCardID();
		deckManager.RemoveDeckCard(cardID);
	}

	private int ParsePanelCardID()
	{
		//name: Panel_Card_XXX
		int cardID = int.Parse(ClickedCard.transform.parent.name.Substring(11));
		return cardID;
	}

	public void OnClickCardListCard()
	{
		Debug.Log("ClickCardListCard");
		int cardID = ParsePanelCardID();
		deckManager.AddDeckCard(cardID);
	}
	// Use this for initialization
	void Start () {
		deckManager = GameObject.Find("/Main Camera").transform.GetComponent<SelectDeck>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
