using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace TouHou_Exploding
{
    public abstract class Skill//:IDProvider.IID
    {
        public string name;
        public string description;
        public UseType useType = UseType.Active;
        public int cost = 0;//手牌花费的B点
        public int typeID { get; set; }
        private bool haveUsed = false;
        public Unit master { get; set; }//持有该技能的卡牌
        public Skill(Unit unit=null)
        {
            master = unit;
            SetAttribute();
        }
        public abstract void SetAttribute();//技能数值的模板，在这里声明新技能的参数
        public void Register(Unit unit)
        {
            master = unit;
        }
        public void Reset()
        {
            haveUsed = false;
        }
        public virtual bool CanUse(InputUse inputUse = null)
        {
            if (master.Owner.bDot < cost) return false;
            return (!HaveUsed()) && master != null;
        }
        public virtual bool HaveUsed()
        {
            return haveUsed;
        }
        public abstract bool Fuction(InputUse inputUse = null);//使用时效果的模板
        public virtual bool Use(InputUse inputUse = null)//使用时调用的方法
        {
            if (CanUse() == false) return false;

            if (Fuction(inputUse))
            {
                haveUsed = true;
                master.Owner.bDot -= cost;
                return true;
            }
            return false;
        }
        public class InputUse
        {

        }

        public enum UseType//发动类型
        {
            Active, Passive
        }
    }
        //主动——出牌阶段发动，需激活，一般限一次，blablablabla
        //被动——锁定技，blblablablabla
        //瞬间——每当你XXXXX时，你可以blablablabla
        //后援——出牌阶段发动，无需激活

    public abstract class ActiveSkill:Skill
    {
        public ActiveSkill(Unit unit)
            :base(unit)
        {
            useType = UseType.Active;
        }
    }
    public abstract class PassiveSkill : Skill
    {
        public PassiveSkill(Unit unit)
            :base(unit)
        {
            useType = UseType.Passive;
        }
    }
    public abstract class Effect
    {

    }
    class Art
    {

    }
    
}
