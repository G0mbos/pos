using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CSharpCalculator.Parser;

namespace CSharpCalculator.Expressions
{
    public class Klammer : Expression
    {
        PlusMinus plusMinus;
        public Klammer(List<Token> tokens) : base(tokens) { }
        public override void Parse()
        {
            Token token = tokens.First();
            index = token.index;
            if (token.value == "(")
            {
                tokens.RemoveAt(0);
                plusMinus = new PlusMinus(tokens);
                plusMinus.Parse();
                token = tokens.First();
                index = token.index;
                if (token.value == ")")
                {
                    tokens.RemoveAt(0);
                }
                else
                {
                    throw new SyntaxException(index);
                }
            }
            else
            {
                throw new SyntaxException(index);
            }
        }
        public override double Calculate()
        {
            return plusMinus.Calculate();
        }
    }
}
