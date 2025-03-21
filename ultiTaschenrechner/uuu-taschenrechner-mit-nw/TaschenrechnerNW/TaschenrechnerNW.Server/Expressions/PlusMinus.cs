using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaschenrechnerNW.Server.Expressions
{
    public class PlusMinus : MyExpression
    {
        private MalDividiert operand;
        private List<Token> operandTokens = new();
        private bool isOperandNegative = false;
        private List<PlusMinusFollower> plusMinusFollowers = new();
        private List<List<Token>> plusMinusFollowerTokens = new();
        public PlusMinus(List<Token> tokens) : base(tokens)
        {
        }

        public override double Evaluate()
        {
            if (!IsParsed)
            {
                Parse();
            }

            double result = isOperandNegative ? -operand.Evaluate() : operand.Evaluate();

            foreach (PlusMinusFollower follower in plusMinusFollowers)
            {
                result += follower.Evaluate();
            }
            return result;
        }

        public override void Parse()
        {
            Console.WriteLine("Visited PlusMinus.Parse");
            IsParsed = true;

            if (tokens.Count == 0)
            {
                throw new SyntaxErrorException("Invalider Syntax");
            }
            
            int bracketDepth = 0;
            int plusMinusDepth = 0;
            isOperandNegative = tokens[0].type == TokenType.PlusMinus && tokens[0].value == "-";

            for (int i = 0; i < tokens.Count; i++)
            {
                if (isOperandNegative && i == 0)
                {
                    continue;
                }

                if (tokens[i].type == TokenType.Klammer && tokens[i].value == "(")
                {
                    bracketDepth++;
                }
                else if (tokens[i].type == TokenType.Klammer && tokens[i].value == ")")
                {
                    bracketDepth--;
                }
                else if (bracketDepth == 0 && tokens[i].type == TokenType.PlusMinus)
                {
                    plusMinusDepth++;
                    plusMinusFollowerTokens.Add([]);
                }

                if (plusMinusDepth == 0)
                {
                    operandTokens.Add(tokens[i]);
                }
                else
                {
                    plusMinusFollowerTokens[plusMinusDepth - 1].Add(tokens[i]);
                }
            }

            operand = new MalDividiert(operandTokens);

            foreach (List<Token> followerTokens in plusMinusFollowerTokens)
            {
                plusMinusFollowers.Add(new PlusMinusFollower(followerTokens));
            }
        }
    }
}
