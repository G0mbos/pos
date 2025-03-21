using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaschenrechnerNW.Server.Expressions
{
    public class NumberLike : MyExpression
    {
        private MyExpression subNumberLike;
        public NumberLike(List<Token> tokens) : base(tokens)
        {
        }

        public override void Parse()
        {
            if (tokens[0].type == TokenType.Ziffer)
            {
                subNumberLike = new Nummer(tokens);
            }
            else if (tokens[0].type == TokenType.Variable)
            {
                subNumberLike = new Variable(tokens);
            }
            else if (tokens[0].type == TokenType.Klammer && tokens[0].value == "(")
            {
                subNumberLike = new Klammer(tokens);
            }
            else
            {
                throw new SyntaxErrorException("Ungültige Nummer");
            }
        }

        public override double Evaluate()
        {
            if (!IsParsed)
            {
                Parse();
            }
            return subNumberLike.Evaluate();
        }
    }
}
