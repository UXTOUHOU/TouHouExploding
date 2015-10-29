using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Chessboard
{
	public class Cell
	{
		public GameObject obj;
		public GameObject background;
		public Unit unit;

		public void SetBackgroundColor(Color newColor)
		{
			background.GetComponent<Image>().color = newColor;
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