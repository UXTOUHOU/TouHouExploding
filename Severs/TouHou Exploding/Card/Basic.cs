using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouHou_Exploding
{
    public abstract class Skill:IDProvider.IID
    {
        public string name { get; set; }
        public int id { get; set; }
        private bool haveUsed = false;
        public void Reset()
        {
            haveUsed = false;
        }
        public abstract bool CanUse();
        public virtual bool HaveUsed()
        {
            return haveUsed;
        }
        public virtual void Use(String[] Args)
        {
            haveUsed = true;
        }
    } 
    public abstract class Effect
    {

    }
    class Art
    {

    }
    
}
