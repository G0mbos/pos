using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCalculator
{
    public class Token
    {
        public enum Type {  PlusMinus, MalDividiert, Hoch, Klammer, Ziffer, Variable}
        public string value;
        public Type type;
        public int index; // for error handling
    }
}
