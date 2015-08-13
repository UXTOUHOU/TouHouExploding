using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace TouHou_Exploding
{
    [DataContract]
    class Card:IDProvider.IID
    {
        [DataMember]
        public int cost { get; set; }
        [DataMember]
        public int id { get; set; }

    }
    class PolicyCard : Card
    {

    }
    class Character : Card
    {
        public Type type { get; set; }
        public Unit.Attribute unit { get; set; }//召唤出的角色属性
        public enum Type{ Common,Hero,Servent }//Common召唤出的为普通单位，Hero为少女单位，Servent为基本效果产生的单位
    }
   [DataContract]
    class Unit : IDProvider.IID 
   {
       public Character card
       {
           get
           {
               return _card;
           }
       }
       [DataMember]
       private Character _card;
       [DataMember]
       public int id { get; set; }
       [DataMember]
       public Map.Region at { get; set; }
       [DataMember]
       public Attribute attribute { get; set; }
       public Statue statue { get; set; }
       public Unit(Character transCard)
       {
           attribute = transCard.unit.Clone();
       }
       public class Attribute//属性
       {
           public string name { get; set; }//不解释
           public int blood { get; set; }//血量
           public int mobility { get; set; }//机动性
           public int attack { get; set; }//攻击伤害
           public int range { get; set; }//射程
           public List<Basic.Skill> skill { get; set; }//单位技能
           public MoveMethous moveMethous;//移动方式
           public Attribute Clone()
           {
               return (Attribute)this.MemberwiseClone();
           }
           public enum MoveMethous{ Walk,Fly,Transport }
       }
       public class Statue//人物状态
       {

       }
    }

}
