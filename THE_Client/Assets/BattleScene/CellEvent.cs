using UnityEngine;
using System.Collections;


namespace Chessboard
{
	public class CellEvent : MonoBehaviour
	{
		public GameObject background;
		public Cell cell;

		public void OnLeftButtonDown()
		{
			Debug.Log("Press Cell:" + background.name);
		}

		public void OnMouseEnter()
		{
			Debug.Log("Enter Cell:" + background.name);
		}

		public void OnMouseLeave()
		{
			Debug.Log("Leave Cell:" + background.name);
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