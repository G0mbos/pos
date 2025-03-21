using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaschenrechnerNW.Server.Expressions;

namespace TaschenrechnerNW.Server
{
    public class Calculator
    {
        private List<Token> tokens;
        public string Calculate(string calculation)
        {
            try
            {
                tokens = Tokenizer.Tokenize(calculation);
                MyExpression expression = new Parser().Parse(tokens);
                if (expression == null)
                {
                    return "ERR";
                }
                return "" + expression.Evaluate();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
