using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpCalculator;
using static CSharpCalculator.Parser;

namespace CSharpCalculator.Expressions
{
    public class HochFollow : Expression
    {
        Exponent exponent;
        public HochFollow(List<Token> tokens) : base(tokens) { }
        public override void Parse()
        {
            Token token = tokens.First();
            index = token.index;
            if (token.type == Token.Type.Hoch)
            {
                tokens.RemoveAt(0);
                exponent = new Exponent(tokens);
                exponent.Parse();
            }
            else
            {
                throw new SyntaxException(index);
            }
        }
        public override double Calculate()
        {
            return exponent.Calculate();
        }
    }
}
