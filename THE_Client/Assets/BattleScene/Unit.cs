using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace BattleScene
{
	public enum EGroupType
	{
		GT_Yourself,
		GT_Enemy
	}

	public class Unit
	{
		public GameObject UnitImage;
		public Cell CurrentCell;
		public CardAttribute UnitAttribute;

		private GameObject textHP;
		private EGroupType groupType;
		private GameObject imageGroup;

		public bool Movable = true;
		public bool Attackable = true;

		public int HP
		{
			get
			{
				return UnitAttribute.hp;
			}

			set
			{
				UnitAttribute.hp = value;
				if(textHP != null)
					textHP.GetComponent<Text>().text = UnitAttribute.hp.ToString();
				if (value <= 0)
				{
					RemoveUnit();
				}
			}
		}

		public EGroupType GroupType
		{
			get
			{
				return groupType;
			}

			set
			{
				groupType = value;
				if (imageGroup != null)
					imageGroup.GetComponent<Image>().sprite = CreateGroupSprite(groupType);
			}
		}

		public Unit(int unitID)
		{
			UnitImage = new GameObject();
			UnitImage.AddComponent<Image>();
			UnitImage.GetComponent<Image>().sprite = CreateUnitSprite(unitID);

			//事件穿透
			UnitImage.AddComponent<RayIgnore>();
			//Test
			UnitAttribute = new CardAttribute();
			//
		}

		private Sprite CreateUnitSprite(int unitID)
		{
			Texture2D cardTexture = Resources.Load<Texture2D>("Units/Unit_" + unitID);
			Sprite unitSprite = Sprite.Create(cardTexture, new Rect(0, 0, cardTexture.width, cardTexture.height), new Vector2(0.5f, 0.5f));
			return unitSprite;
		}

		public void InitUnitGroup()
		{
			if (imageGroup == null)
			{
				imageGroup = new GameObject("UnitGroup");
				imageGroup.transform.SetParent(GameObject.Find("/Canvas/UnitGroup").transform);
				imageGroup.AddComponent<RectTransform>().pivot = new Vector2(1, 1);
				imageGroup.AddComponent<RayIgnore>();
				//读取阵营标志的图片
				Sprite groupSprite = CreateGroupSprite(groupType);

				imageGroup.transform.localScale = new Vector3(Chessboard.CellSize / groupSprite.texture.width / 64,
					Chessboard.CellSize / groupSprite.texture.width / 64,
					1);
				imageGroup.AddComponent<Image>().sprite = groupSprite;

				UpdateGroupPosition();
			}
		}

		public void UpdateGroupPosition()
		{
			Vector3 cellPosition = CurrentCell.GetLocalPosition();
			imageGroup.transform.localPosition = new Vector3(Chessboard.CellSize / 2 + cellPosition.x, 
					Chessboard.CellSize / 2 + cellPosition.y, 
					0);
		}

		public void SetGroupVisible(bool visible)
		{
			imageGroup.SetActive(visible);
		}

		public void InitUnitHP()
		{
			if (textHP == null)
			{
				textHP = GameObject.Instantiate(GameObject.Find("UnitHPPrefab"));
				textHP.transform.SetParent(GameObject.Find("/Canvas/UnitHP").transform);
				textHP.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
				textHP.GetComponent<Text>().text = HP.ToString();
				UpdateHPPosition();
			}
		}

		public void UpdateHPPosition()
		{
			Vector3 cellPosition = CurrentCell.GetLocalPosition();
			textHP.transform.localPosition = new Vector3(-Chessboard.CellSize / 2 + cellPosition.x, 
					Chessboard.CellSize / 2 + cellPosition.y, 
					0);
		}

		public void SetHPVisible(bool visible)
		{
			textHP.SetActive(visible);
		}

		////显示阵营标志
		//public void ShowUnitGroup()
		//{
		//	if (imageGroup == null)
		//	{
		//		imageGroup = new GameObject("UnitGroup");
		//		imageGroup.transform.SetParent(GameObject.Find("/Canvas").transform);
		//		imageGroup.AddComponent<RectTransform>().pivot = new Vector2(1, 1);
		//		imageGroup.AddComponent<RayIgnore>();
		//		//读取阵营标志的图片
		//		Sprite groupSprite = CreateGroupSprite(groupType);

		//		imageGroup.transform.localScale = new Vector3(Chessboard.CellSize / groupSprite.texture.width / 64, 
		//			Chessboard.CellSize / groupSprite.texture.width / 64, 
		//			1);
		//		imageGroup.AddComponent<Image>().sprite = groupSprite;
		//	}
		//	else
		//	{
		//		imageGroup.SetActive(true);
		//	}
		//	//更新位置
		//	Vector3 cellPosition = CurrentCell.GetLocalPosition();
		//	imageGroup.transform.localPosition = new Vector3(Chessboard.CellSize / 2 + cellPosition.x, Chessboard.CellSize / 2 + cellPosition.y, 0);
		//}

		private Sprite CreateGroupSprite(EGroupType type)
		{
			Texture2D groupTexture;
			if (type == EGroupType.GT_Yourself)
				groupTexture = Resources.Load<Texture2D>("Images/GroupBlue");
			else// if (groupType == EGroupType.GT_Enemy)
				groupTexture = Resources.Load<Texture2D>("Images/GroupRed");
			Sprite groupSprite = Sprite.Create(groupTexture,
				new Rect(0, 0, groupTexture.width, groupTexture.height),
				new Vector2(0.5F, 0.5F));
			return groupSprite;
		}

		//public void HideUnitGroup()
		//{
		//	imageGroup.SetActive(false);
		//}

		////显示HP
		//public void ShowUnitHP()
		//{
		//	if (textHP == null)
		//	{
		//		textHP = GameObject.Instantiate(GameObject.Find("UnitHPPrefab"));
		//		textHP.transform.SetParent(GameObject.Find("/Canvas").transform);
		//		textHP.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
		//		textHP.GetComponent<Text>().text = HP.ToString();
		//	}
		//	else
		//	{
		//		textHP.SetActive(true);
		//	}
		//	//更新位置
		//	Vector3 cellPosition = CurrentCell.GetLocalPosition();
		//	textHP.transform.localPosition = new Vector3(-Chessboard.CellSize / 2 + cellPosition.x, Chessboard.CellSize / 2 + cellPosition.y, 0);
		//}

		////隐藏HP
		//public void HideUnitHP()
		//{
		//	textHP.SetActive(false);
		//}

		public void RemoveUnit()
		{
			if (CurrentCell != null)
				CurrentCell.UnitOnCell = null;
			GameObject.Destroy(UnitImage);
			GameObject.Destroy(textHP);
			GameObject.Destroy(imageGroup);
			
			//删除技能产生的效果
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