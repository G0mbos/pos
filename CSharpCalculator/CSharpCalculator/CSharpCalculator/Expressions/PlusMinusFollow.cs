using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CSharpCalculator.Parser;

namespace CSharpCalculator.Expressions
{
    public class PlusMinusFollow : Expression
    {
        string Operrand;
        MalDividiert malDividiert;
        public PlusMinusFollow(List<Token> tokens) : base(tokens) { }
        public override void Parse()
        {
            Token token = tokens.First();
            index = token.index;
            if (token.type == Token.Type.PlusMinus)
            {
                Operrand = token.value;
                tokens.RemoveAt(0);
                malDividiert = new MalDividiert(tokens);
                malDividiert.Parse();
            }
            else
            {
                throw new SyntaxException(index);
            }
        }
        public override double Calculate()
        {
            if (Operrand == "+")
            {
                return malDividiert.Calculate();
            }
            else
            {
                return - malDividiert.Calculate();
            }
        }
    }
}
