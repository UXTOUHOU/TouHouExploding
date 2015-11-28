using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace BattleScene
{
	public class UnitUI
	{
		public GameObject UnitImage;
		private Unit unit;
		private GameObject textHP;
		private GameObject imageGroup;

		public UnitUI(Unit summonUnit, ChessboardPosition position)
		{
			if (Chessboard.GetCell(position).UnitOnCell == null)
			{
				unit = summonUnit;
				InitUnitImage();
				InitUnitGroupImage();
				InitUnitHPText();

				UnitUIManager.AddUnitSprite(this);
			}
		}

		private void InitUnitImage()
		{
			UnitImage = new GameObject();
			UnitImage.transform.SetParent(GameObject.Find("/Canvas/UnitImage").transform);
			Vector3 cellPosition = unit.CurrentCell.GetLocalPosition();
			UnitImage.transform.localPosition = cellPosition;
			UnitImage.transform.localScale = new Vector3(Chessboard.CellSize / 75, Chessboard.CellSize / 75, 1);
			UnitImage.AddComponent<Image>().sprite = CreateUnitSprite(unit.UnitAttribute.ID);
			UnitImage.AddComponent<RayIgnore>();
		}

		private Sprite CreateUnitSprite(int unitID)
		{
			Texture2D cardTexture = Resources.Load<Texture2D>("Units/Unit_" + unitID);
			Sprite unitSprite = Sprite.Create(cardTexture, new Rect(0, 0, cardTexture.width, cardTexture.height), new Vector2(0.5f, 0.5f));
			return unitSprite;
		}

		private void InitUnitGroupImage()
		{
			if (imageGroup == null)
			{
				imageGroup = new GameObject("UnitGroup");
				imageGroup.transform.SetParent(GameObject.Find("/Canvas/UnitGroup").transform);
				imageGroup.AddComponent<RectTransform>().pivot = new Vector2(1, 1);
				imageGroup.AddComponent<RayIgnore>();
				//读取阵营标志的图片
				Sprite groupSprite = CreateGroupSprite(unit.GroupType);

				imageGroup.transform.localScale = new Vector3(Chessboard.CellSize / groupSprite.texture.width / 64,
					Chessboard.CellSize / groupSprite.texture.width / 64,
					1);
				imageGroup.AddComponent<Image>().sprite = groupSprite;

				UpdateGroupPosition();
			}
		}
		private void UpdateGroupPosition()
		{
			Vector3 cellPosition = unit.CurrentCell.GetLocalPosition();
			imageGroup.transform.localPosition = new Vector3(Chessboard.CellSize / 2 + cellPosition.x,
					Chessboard.CellSize / 2 + cellPosition.y,
					0);
		}

		public void UpdateGroup()
		{
			imageGroup.GetComponent<Image>().sprite = CreateGroupSprite(unit.GroupType);
		}

		public void UpdateTextHP()
		{
			textHP.GetComponent<Text>().text = unit.HP.ToString();
		}

		public void SetGroupVisible(bool visible)
		{
			imageGroup.SetActive(visible);
		}

		private void InitUnitHPText()
		{
			if (textHP == null)
			{
				textHP = GameObject.Instantiate(GameObject.Find("UnitHPPrefab"));
				textHP.transform.SetParent(GameObject.Find("/Canvas/UnitHP").transform);
				textHP.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
				textHP.GetComponent<Text>().text = unit.HP.ToString();
				UpdateHPPosition();
			}
		}

		private void UpdateHPPosition()
		{
			Vector3 cellPosition = unit.CurrentCell.GetLocalPosition();
			textHP.transform.localPosition = new Vector3(-Chessboard.CellSize / 2 + cellPosition.x,
					Chessboard.CellSize / 2 + cellPosition.y,
					0);
		}

		public void SetHPVisible(bool visible)
		{
			textHP.SetActive(visible);
		}

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

		public void RemoveUnitSprite()
		{
			GameObject.Destroy(UnitImage);
			GameObject.Destroy(textHP);
			GameObject.Destroy(imageGroup);
		}

		public void SetPosition(Vector3 position)
		{
			UnitImage.transform.position = position;
		}
	}
}