using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaschenrechnerNW.Server
{
    public static class VariableReference
    {
        public static Dictionary<string, double> Variables { get; } = new Dictionary<string, double>();
        public static string UnfinishedCalculation { get; set; } = "";

        public static string UnfinishedVariable { get; set; } = "";

        public static void Reset()
        {
            Variables.Clear();
            UnfinishedCalculation = "";
            UnfinishedVariable = "";
        }
    }
}
