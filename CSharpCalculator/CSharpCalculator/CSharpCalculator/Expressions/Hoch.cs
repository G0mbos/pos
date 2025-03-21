using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCalculator.Expressions
{
    public class Hoch : Expression
    {
        Exponent exponent;
        List<HochFollow> hochFollows = new List<HochFollow>();
        public Hoch(List<Token> tokens) : base(tokens) { }
        public override void Parse()
        {
            exponent = new Exponent(tokens);
            exponent.Parse();
            while (tokens.Count > 0 && tokens.First().type == Token.Type.Hoch)
            {
                HochFollow hochFollow = new HochFollow(tokens);
                hochFollow.Parse();
                hochFollows.Add(hochFollow);
            }
        }

        public override double Calculate()
        {
            double result = exponent.Calculate();
            foreach (HochFollow hochFollow in hochFollows)
            {
                result = Math.Pow(result, hochFollow.Calculate());
            }
            return result;
        }
    }
}
