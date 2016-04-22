using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System;
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

    private static DataManager _instance;

    public static DataManager getInstance()
    {
        if ( _instance == null )
        {
            _instance = new DataManager();
        }
        return _instance;
    }

    private Dictionary<string, object> _datasMap;
    private Dictionary<string, string> _parserMap;
    private string _cfgFolderPath;

    public void init()
    {
        this._parserMap = new Dictionary<string, string>();
        this._parserMap.Add("Units", "UnitCfg");
        this._datasMap = new Dictionary<string, object>();
        this._cfgFolderPath = Path.Combine(Application.streamingAssetsPath, "Configs");
    }

    public object getDatasByName(string name)
    {
        object datas;
        if ( !this._datasMap.TryGetValue(name,out datas) )
        {
            // 解析xml
            datas = this.parseXML(name);
        }
        return datas;
    }

    private object parseXML(string name)
    {
        string filepath = Path.Combine(this._cfgFolderPath,name + ".xml");
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(filepath);
        XmlNode xmlNode = xmlDoc.SelectSingleNode("THE_Data");
        XmlNodeList nodeList = xmlNode.ChildNodes;
        string parserName;
        if ( this._parserMap.TryGetValue(name,out parserName) )
        {
            Type type = Type.GetType(parserName);
            IParser parser = (IParser)type.Assembly.CreateInstance(type.Name);
            IParser newParser;
            Dictionary<string, IParser> dic = new Dictionary<string, IParser>();
            foreach (XmlElement xe in nodeList)
            {
                newParser = parser.createNewInstance();
                newParser.parse(xe);
                dic.Add(xe.GetAttribute("Id"), newParser);
            }
            this._datasMap.Add(name, dic);
            return dic;
        }
        return null;
    }
}