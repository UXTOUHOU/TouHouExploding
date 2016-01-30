using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace BattleScene
{
	public class Cemetery : MonoBehaviour
	{
		private static Cemetery singleton = null;
		public static Cemetery GetInstance()
		{
			return singleton;
		}

		public GameObject CemeteryRect;
		public GameObject cemeteryCardPrefab;

		private ArrayList cemeteryCard = new ArrayList();
		private float cemeteryCardWidth, cemeteryCardHeight;
		private const float cardWidth = 420F, cardHeight = 590F;

		public void AddCemeteryCard(int cardID)
		{
			if (cemeteryCard.Count >= 6)
				Debug.Log("墓地卡片超过6张");

			Cemetery cemetery = singleton;
			GameObject card = Instantiate(cemetery.cemeteryCardPrefab.transform.FindChild("Card").gameObject);
			card.name = "Card_" + cardID.ToString().PadLeft(4, '0');                                    //Set name "Card_XXXX"
			card.transform.SetParent(CemeteryRect.transform);
			card.GetComponent<Image>().sprite = DataManager.CreateCardSprite(cardID);
			Deck.SetCardSize(card, cemetery.cemeteryCardWidth, cemetery.cemeteryCardHeight);
			cemetery.cemeteryCard.Add(card);
			cemetery.SortCemeteryCard();

		}

		private void SortCemeteryCard()
		{
			int cardNum = cemeteryCard.Count;
			Vector3 cemeteryPosition = new Vector3();//CemeteryRect.transform.localPosition;
			cemeteryPosition.x = cemeteryCardWidth * (cardNum / 2F - 0.5F);
			foreach (GameObject it in cemeteryCard)
			{
				GameObject card = it;
				card.transform.localPosition = cemeteryPosition;
				cemeteryPosition.x += cemeteryCardWidth;
			}
		}

		public void SetCemeteryActive(bool active)
		{
			CemeteryRect.SetActive(active);
			CemeteryRect.transform.SetSiblingIndex(GameObject.Find("/Canvas").transform.childCount);
		}

		private void RemoveCemeteryCard(int cardID)
		{
			throw new NotImplementedException();
		}

		// Use this for initialization
		void Start()
		{
			singleton = this;

			var anchor = CemeteryRect.GetComponent<RectTransform>();
			cemeteryCardHeight = (anchor.anchorMax.y - anchor.anchorMin.y) * Screen.height;
			cemeteryCardWidth = cemeteryCardHeight * cardWidth / cardHeight;

			//test
			AddCemeteryCard(6);
			//
		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}