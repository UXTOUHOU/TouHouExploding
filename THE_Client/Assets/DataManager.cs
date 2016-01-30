using UnityEngine;
using System.Collections;
using BattleScene;

/// <summary>
/// 从外部文件取得数据的类
/// </summary>
public class DataManager
{
	/// <summary>
	/// 从资源文件中读取unit的texture并创建成Sprite
	/// </summary>
	/// <param name="unitID"></param>
	/// <returns></returns>
	public static Sprite CreateUnitSprite(int unitID)
	{
		Texture2D cardTexture = Resources.Load<Texture2D>("Units/Unit_" + unitID);
		Sprite unitSprite = Sprite.Create(cardTexture, 
			new Rect(0, 0, cardTexture.width, cardTexture.height), 
			new Vector2(0.5f, 0.5f));
		return unitSprite;
	}

	/// <summary>
	/// 从资源文件中读取card的texture并创建成Sprite
	/// </summary>
	/// <param name="cardID"></param>
	/// <returns></returns>
	public static Sprite CreateCardSprite(int cardID)
	{
		Texture2D cardTexture = Resources.Load<Texture2D>("Cards/Card_" + cardID.ToString().PadLeft(4, '0'));
		Sprite cardSprite = Sprite.Create(cardTexture,
			new Rect(0, 0, cardTexture.width, cardTexture.height),
			new Vector2(0.5f, 0.5f));
		return cardSprite;
	}

	/// <summary>
	/// 从资源文件中读取group的texture并创建成Sprite
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public static Sprite CreateGroupSprite(EGroupType type)
	{
		Texture2D groupTexture;
		if (type == EGroupType.BlueSide)
			groupTexture = Resources.Load<Texture2D>("Images/GroupBlue");
		else
			groupTexture = Resources.Load<Texture2D>("Images/GroupRed");
		Sprite groupSprite = Sprite.Create(groupTexture,
			new Rect(0, 0, groupTexture.width, groupTexture.height),
			new Vector2(0.5F, 0.5F));
		return groupSprite;
	}
}