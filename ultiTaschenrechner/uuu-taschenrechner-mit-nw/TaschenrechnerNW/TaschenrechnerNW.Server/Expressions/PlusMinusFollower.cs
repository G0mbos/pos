using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaschenrechnerNW.Server.Expressions
{
    public class PlusMinusFollower : MyExpression
    {
        private MalDividiert operand;
        private bool isPlus;
        public PlusMinusFollower(List<Token> tokens) : base(tokens)
        {
        }

        public override double Evaluate()
        {
            if (!IsParsed)
            {
                Parse();
            }

            return isPlus ? operand.Evaluate() : -operand.Evaluate();
        }

        public override void Parse()
        {
            if (tokens.Count == 0)
            {
                throw new SyntaxErrorException("Syntax Error!");
            }

            IsParsed = true;

            if (tokens[0].value == "+")
            {
                isPlus = true;
            }
            else if (tokens[0].value == "-")
            {
                isPlus = false;
            }
            else
            {
                throw new SyntaxErrorException("Ungültiger Operator");
            }

            operand = new MalDividiert(tokens[1..]);
        }
    }
}
