using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCalculator.Expressions
{
    public class PlusMinus : Expression
    {
        MalDividiert malDividiert;
        List<PlusMinusFollow> plusMinusFollows = new List<PlusMinusFollow>();
        public PlusMinus(List<Token> tokens) : base(tokens) { }

        public override void Parse()
        {
            malDividiert = new MalDividiert(tokens);
            malDividiert.Parse();
            while(tokens.Count > 0 && tokens.First().type == Token.Type.PlusMinus)
            {
                PlusMinusFollow pmf = new PlusMinusFollow(tokens);
                plusMinusFollows.Add(pmf);
                pmf.Parse();
            }
        }
        public override double Calculate()
        {
            return malDividiert.Calculate() + plusMinusFollows.Sum(x => x.Calculate());
        }
    }
}
