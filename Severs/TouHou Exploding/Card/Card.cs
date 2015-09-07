using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace TouHou_Exploding
{
    public abstract class Card:IDProvider.IID
    {
        public int cost { get; set; }
        public int id { get; set; }

    }
    public class PolicyCard : Card
    {

    }
    public abstract class Character : Card
    {
        private Core _core;
        public Type type { get; set; }
        public Unit.Attribute unit { get; set; }//召唤出的角色属性
        public enum Type{ Common,Hero,Servant }//Common召唤出的为普通单位，Hero为少女单位，Servent为基本效果产生的单位
        public class CharacterSave
        {

        }
        public enum Preset { 毛玉, 天狗, 妖精, 永远亭的兔子, 自爆人形, 河童重工, 博丽灵梦, 魂魄妖梦, 十六夜咲夜, Custom }
        public Character(Core core, CharacterSave save)
        {

        }
        public Unit ToBattle()
        {
            Unit x =new Unit(this);
            return x;
        }
        public Character(Core core, Preset preset)
        {
            _core = core;
            unit = new Unit.Attribute();
            switch (preset)
            {
                case Preset.毛玉 :
                    {
                        unit.blood = 9;
                        unit.mobility = 2;
                        unit.attack = 2;
                        unit.range = 2;
                        //unit.Skill
                    }
                    break;
                case Preset.天狗:
                    {
                        unit.blood = 9;
                        unit.mobility = 4;
                        unit.attack = 5;
                        unit.range = 1;
                    }
                    break;
                case Preset.妖精:
                    {
                        unit.blood = 8;
                        unit.mobility = 3;
                        unit.attack = 2;
                        unit.range = -1;//-1即为特殊情况，要要使用自带的特殊方法判断：对射程3（不含）以下的单位额外造成1伤害。
                    }
                    break;
                case Preset.永远亭的兔子:
                    {
                        unit.blood = 9;
                        unit.mobility = 2;
                        unit.attack = 2;
                        unit.range = 2;
                    }
                    break;
                case Preset.自爆人形:
                    {

                    }
                    break;
                case Preset.河童重工:
                    {

                    }
                    break;
                case Preset.博丽灵梦:
                    {

                    }
                    break;
                case Preset.魂魄妖梦:
                    {

                    }
                    break;
                case Preset.十六夜咲夜:
                    {

                    }
                    break;
                case Preset.Custom:
                    {

                    }
                    break;
            }
        } 
    }
   public class Unit : IDProvider.IID 
   {
       public Character card
       {
           get
           {
               return _card;
           }
       }
       private Character _card;
       public int id { get; set; }
       public Region at { get; set; }
       public Attribute attribute { get; set; }
       public Statue statue { get; set; }
       public Unit(Character transCard)
       {
           attribute = transCard.unit.Clone();
           _card = transCard;
       }
       public bool Attack(Unit target)//攻击指令，如果不能攻击返回假
       {
           return false;
       }
       public int Hurt(int blood)//体力流失，返回剩余血量，死亡返回-1
       {
           attribute.blood -= blood;
           if(attribute.blood<=0)
           {
               Die();
               return -1;
           }
           return attribute.blood;
       }
       public void Die()
       {

       } 
       public class Attribute//属性
       {
           public string name { get; set; }//不解释
           public Character.Type type { get; set; }//角色类型
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
       public class Statue//人物附加状态
       {

       }
    }

}
