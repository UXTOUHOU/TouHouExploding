using UnityEngine;
using System.Collections;

public class DeckCardEvent : MonoBehaviour {
	public GameObject card;

	public void OnMouseDown()
	{
		Debug.Log("Press Card:" + card.name);
		//Test
		Deck.RemoveDeckCard(card);
		//
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
