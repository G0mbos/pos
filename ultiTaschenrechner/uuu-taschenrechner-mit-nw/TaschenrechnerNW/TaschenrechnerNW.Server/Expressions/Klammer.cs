using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaschenrechnerNW.Server.Expressions
{
    public class Klammer : MyExpression
    {
        public Klammer(List<Token> tokens) : base(tokens)
        {
        }

        public override double Evaluate()
        {
            if (!IsParsed)
            {
                Parse();
            }
            return new PlusMinus(tokens[1..^1]).Evaluate();
        }

        public override void Parse()
        {
            IsParsed = true;
            if (tokens[0].value != "(" || tokens[^1].value != ")")
            {
                throw new SyntaxErrorException("Klammer nicht korrekt gesetzt");
            }
        }
    }
}
