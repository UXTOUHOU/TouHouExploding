using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public static class CommondHelper
    {
        public static void Commond(string commond)
        {
            ReadCommond(CommondHelper.Divide(commond));
        }
        public static void ReadCommond(string[] commond)
        {
            int length = commond.Length;

        }
        public static bool IsExist(string[] commond, int index)
        {
            return commond.Length > index;
        }
        public static string[] Divide(string commond)//用于分隔指令
        {
            string[] str = commond.Split(' ');
            return str;
        }
    }
}
