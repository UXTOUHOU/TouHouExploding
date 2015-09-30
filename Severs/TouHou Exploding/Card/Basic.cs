using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouHou_Exploding
{
    public abstract class Skill:IDProvider.IID
    {
        public string name;
        public string description;
        public int id { get; set; }
        private bool haveUsed = false;
        public Unit master { get; set; }//持有该技能的卡牌
        public Skill(Unit unit=null)
        {
            master = unit;
        }
        public void Register(Unit unit)
        {
            master = unit;
        }
        public void Reset()
        {
            haveUsed = false;
        }
        public virtual bool CanUse()
        {
            return (!HaveUsed()) && master != null;
        }
        public virtual bool HaveUsed()
        {
            return haveUsed;
        }
        public virtual void Use(InputUse inputuse = null)
        {
            haveUsed = true;
        }
        public class InputUse
        {

        }
    } 
    public abstract class Effect
    {

    }
    class Art
    {

    }
    
}
