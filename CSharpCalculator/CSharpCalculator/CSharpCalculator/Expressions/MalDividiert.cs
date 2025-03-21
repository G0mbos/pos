using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCalculator.Expressions
{
    public class MalDividiert : Expression
    {
        Hoch hoch;
        List<MalDividiertFollow> malDividiertFollows = new List<MalDividiertFollow>();
        public MalDividiert(List<Token> tokens) : base(tokens) { }
        public override void Parse()
        {
            hoch = new Hoch(tokens);
            hoch.Parse();
            while(tokens.Count > 0 && tokens.First().type == Token.Type.MalDividiert)
            {
                MalDividiertFollow mdf = new MalDividiertFollow(tokens);
                malDividiertFollows.Add(mdf);
                mdf.Parse();
            }
        }
        public override double Calculate()
        {
            double result;
            result = hoch.Calculate();
            foreach (MalDividiertFollow mdf in malDividiertFollows)
            {
                if (mdf.operrand == "*") // ka wie man das anders machen soll
                {
                    result *= mdf.Calculate();
                }
                else
                {
                    result /= mdf.Calculate();
                }
            }
            return result;
        }
    }
}
