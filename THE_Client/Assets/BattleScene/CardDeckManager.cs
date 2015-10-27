using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardDeckManager : MonoBehaviour {
    public GameObject cardDeckInner;
    public GameObject cardDeckPrefab;
    // Use this for initialization
    void Start () {
        //Init
        //向CardList中添加卡片
        int[] cardID = { 1, 2, 3, 6, 11, 13, 19 };
        for (int i = 0; i < 7; i++)
        {
            GameObject newCard = Instantiate(cardDeckPrefab.transform.FindChild("Panel").gameObject);
            newCard.name = "Panel_Card_" + cardID[i].ToString("d4");
            newCard.transform.SetParent(cardDeckInner.transform);
            
            string cardName = "Card_" + cardID[i].ToString("d4");
            Texture2D cardTexture = Resources.Load<Texture2D>("Cards/" + cardName);
            Sprite cardSprite = Sprite.Create(cardTexture, new Rect(0, 0, cardTexture.width, cardTexture.height), new Vector2(0.5f, 0.5f));
            newCard.transform.FindChild("Card").GetComponent<Image>().sprite = cardSprite;
            newCard.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
