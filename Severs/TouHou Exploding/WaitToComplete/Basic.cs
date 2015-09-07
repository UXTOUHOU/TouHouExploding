using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouHou_Exploding
{
    public class Basic
    {
        public class Skill:IDProvider.IID
        {
            public string name { get; set; }
            public int id { get; set; }
            public bool CanUse()
            {
                return false;
            }
        } 
        public abstract class Effect
        {

        }
        class Art
        {

        }
    }
}
