using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BattleScene
{
	public class Cell
	{
		public GameObject obj;
		public GameObject background;
		public Unit unit;

		public static Color selected = new Color(10F / 255, 23F / 255, 96F / 255);
		public static Color movable = new Color(70F / 255, 112F / 255, 70F / 255);
		public static Color atk = new Color(149F / 255, 70F / 255, 70F / 255);

		public static GameObject btnMove = GameObject.Find("Canvas/ButtonMove");
		public static GameObject btnAttack = GameObject.Find("Canvas/ButtonAttack");
		public static GameObject btnSkill = GameObject.Find("Canvas/ButtonSkill");
		public void ShowOperateButton()
		{
			float cellSize = Chessboard.CellSize;
			//GameObject btnMove = GameObject.Find("Canvas/ButtonMove");
			//GameObject btnAttack = GameObject.Find("Canvas/ButtonAttack");
			//GameObject btnSkill = GameObject.Find("Canvas/ButtonSkill");
			btnMove.SetActive(true);
			btnAttack.SetActive(true);
			btnSkill.SetActive(true);
			btnMove.transform.position = background.transform.position + new Vector3(0, cellSize / 1.3F, 0);
			btnAttack.transform.position = background.transform.position + new Vector3(-cellSize / 1.5F, cellSize / 2F, 0);
			btnSkill.transform.position = background.transform.position + new Vector3(cellSize / 1.5F, cellSize / 2F, 0);
		
			GameObject canvas = btnMove.transform.parent.gameObject;
			int childCount = canvas.transform.childCount;
			btnMove.transform.SetSiblingIndex(childCount);
			btnAttack.transform.SetSiblingIndex(childCount);
			btnSkill.transform.SetSiblingIndex(childCount);
		}

		public void HideOperateButton()
		{
			//GameObject btnMove = GameObject.Find("Canvas/ButtonMove");
			//GameObject btnAttack = GameObject.Find("Canvas/ButtonAttack");
			//GameObject btnSkill = GameObject.Find("Canvas/ButtonSkill");
			btnMove.SetActive(false);
			btnAttack.SetActive(false);
			btnSkill.SetActive(false);
		}

		public void ShowSkillButton()
		{ 
			
		}
		public void SetBackgroundColor(Color newColor)
		{
			background.GetComponent<Image>().color = newColor;
		}
		public void ShowUnitGroup()
		{ 
			
		}
		public void HideUnitGroup()
		{ 
		
		}
		public void ShowUnitHP()
		{ 
		
		}
		public void HideUnitHP()
		{ 
		
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