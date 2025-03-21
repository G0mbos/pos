using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaschenrechnerNW.Server.Expressions
{
    public class MalDividiertFollower : MyExpression
    {
        public MalDividiertFollower(List<Token> tokens) : base(tokens)
        {
        }

        private bool isMultiplication;
        private Hoch operand;

        public override double Evaluate()
        {
            if (!IsParsed)
            {
                Parse();
            }
            return isMultiplication ? operand.Evaluate() : (1/operand.Evaluate());
        }

        public override void Parse()
        {
            IsParsed = true;

            if (tokens[0].value == "*")
            {
                isMultiplication = true;
            }
            else if (tokens[0].value == "/")
            {
                isMultiplication = false;
            }
            else
            {
                throw new SyntaxErrorException("Ungültiger Operator");
            }

            operand = new Hoch(tokens[1..]);
        }
    }
}
