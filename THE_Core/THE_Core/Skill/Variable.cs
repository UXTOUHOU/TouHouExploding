using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public class Variable
    {
        public int Index;
        public string Description;
        public VariableType Type;
    }

    public enum VariableType
    {
        Int,
        Percent,
        String
    }
}
