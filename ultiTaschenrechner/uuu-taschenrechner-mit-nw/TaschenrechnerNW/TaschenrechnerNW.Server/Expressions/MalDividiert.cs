using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaschenrechnerNW.Server.Expressions
{
    public class MalDividiert : MyExpression
    {
        private Hoch operand;
        private List<Token> operandTokens = new();
        private List<MalDividiertFollower> followers = new();
        private List<List<Token>> followerTokens = new();
        public MalDividiert(List<Token> tokens) : base(tokens)
        {
        }

        public override double Evaluate()
        {
            if (!IsParsed)
            {
                Parse();
            }

            double result = operand.Evaluate();

            foreach (MalDividiertFollower follower in followers)
            {
                result *= follower.Evaluate();
            }

            return result;
        }

        public override void Parse()
        {
            Console.WriteLine("Visited PlusMinus.Parse");
            IsParsed = true;

            int bracketDepth = 0;
            int malDividiertDepth = 0;

            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].type == TokenType.Klammer && tokens[i].value == "(")
                {
                    bracketDepth++;
                }
                else if (tokens[i].type == TokenType.Klammer && tokens[i].value == ")")
                {
                    bracketDepth--;
                }
                else if (bracketDepth == 0 && tokens[i].type == TokenType.MalDividiert)
                {
                    malDividiertDepth++;
                    followerTokens.Add([]);
                }

                if (malDividiertDepth == 0)
                {
                    operandTokens.Add(tokens[i]);
                }
                else
                {
                    followerTokens[malDividiertDepth - 1].Add(tokens[i]);
                }
            }

            operand = new Hoch(operandTokens);

            foreach (List<Token> singleFollowerTokens in followerTokens)
            {
                followers.Add(new MalDividiertFollower(singleFollowerTokens));
            }
        }
    }
}
