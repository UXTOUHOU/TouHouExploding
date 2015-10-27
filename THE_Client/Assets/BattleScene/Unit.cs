using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Chessboard
{
	public class Unit
	{
		public GameObject image;
		public Unit(int unitID)
		{
			image = new GameObject();
			image.AddComponent<Image>();
			image.GetComponent<Image>().sprite = CreateUnitSprite(unitID);
		}

		private Sprite CreateUnitSprite(int unitID)
		{
			Texture2D cardTexture = Resources.Load<Texture2D>("Units/Unit_" + unitID);
			Sprite unitSprite = Sprite.Create(cardTexture, new Rect(0, 0, cardTexture.width, cardTexture.height), new Vector2(0.5f, 0.5f));
			return unitSprite;
		}
		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}