using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CSharpCalculator.Parser;

namespace CSharpCalculator.Expressions
{
    public class MalDividiertFollow : Expression
    {
        public string operrand;
        Hoch hoch;
        public MalDividiertFollow(List<Token> tokens) : base(tokens) { }
        public override void Parse()
        {
            Token token = tokens.First();
            index = token.index;
            if (token.type == Token.Type.MalDividiert)
            {
                operrand = token.value;
                tokens.RemoveAt(0);
                hoch = new Hoch(tokens);
                hoch.Parse();
            }
            else
            {
                throw new SyntaxException(index);
            }
        }
        public override double Calculate()
        {
            return hoch.Calculate();
        }
    }
}
