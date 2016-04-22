using System;
using System.Xml;

// todo: 改成get方法
public class UnitCfg : IParser
{
    public string id;
    public string name;
    public string description;
    public string type;
    public int hitPoint;
    public int mobilityBase;
    public int attackBase;
    public int defenceBase;
    public int minAttackRange;
    public int maxAttackRange;
    public int isEnable;
    /// <summary>
    /// 卡片的资源名称
    /// </summary>
    public string cardTexture;
    /// <summary>
    /// 人物的资源名称
    /// </summary>
    public string characterTexture;

    public void parse(XmlElement xmlElement)
    {
        this.id = xmlElement.GetAttribute("Id");
        this.name = xmlElement.GetAttribute("Name");
        this.description = xmlElement.GetAttribute("Description");
        this.type = xmlElement.GetAttribute("Type");
        this.hitPoint = int.Parse(xmlElement.GetAttribute("HitPoint"));
        this.mobilityBase = int.Parse(xmlElement.GetAttribute("Mobility"));
        this.attackBase = int.Parse(xmlElement.GetAttribute("AttackPower"));
        //this.defenceBase = int.Parse(xmlElement.GetAttribute("Defence"));
        string range = xmlElement.GetAttribute("AttackRange");
        string[] rangeList = range.Split(',');
        if ( rangeList.Length == 1 )
        {
            this.minAttackRange = 0;
            this.maxAttackRange = int.Parse(rangeList[0]);
        }
        else
        {
            this.minAttackRange = int.Parse(rangeList[0]);
            this.maxAttackRange = int.Parse(rangeList[1]);
        }
        string tmpStr = xmlElement.GetAttribute("isEnable");
        isEnable = tmpStr == "" ? 0 : int.Parse(tmpStr);
        this.cardTexture = xmlElement.GetAttribute("CardTexture");
        this.characterTexture = xmlElement.GetAttribute("CharacterTexture");
    }

    public IParser createNewInstance()
    {
        return new UnitCfg();
    }
}