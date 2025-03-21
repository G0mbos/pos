using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaschenrechnerNW.Server.Expressions
{
    public class Hoch : MyExpression
    {
        private NumberLike basis;
        private Hoch exponent;

        private List<Token> basisTokens = new();
        private List<Token> exponentTokens = new();
        private bool isParsingExponent = false;

        public Hoch(List<Token> tokens) : base(tokens)
        {
        }

        public override double Evaluate()
        {
            if (!IsParsed)
            {
                Parse();
            }
            if (exponent == null)
            {
                return basis.Evaluate();
            }
            return Math.Pow(basis.Evaluate(), exponent.Evaluate());
        }

        public override void Parse()
        {
            Console.WriteLine("Visited Hoch.Parse");
            IsParsed = true;
            int bracketDepth = 0;
            for (int i = 0; i < tokens.Count; i++   )
            {
                if (tokens[i].type == TokenType.Klammer && tokens[i].value == "(")
                {
                    bracketDepth++;
                }
                else if (tokens[i].type == TokenType.Klammer && tokens[i].value == ")")
                {
                    bracketDepth--;
                }



                if (tokens[i].type == TokenType.Hoch && !isParsingExponent && bracketDepth == 0)
                {
                    isParsingExponent = true;
                }
                else if (!isParsingExponent)
                {
                    basisTokens.Add(tokens[i]);
                }
                else
                {
                    exponentTokens.Add(tokens[i]);
                }
            }

            basis = new NumberLike(basisTokens);
            if (exponentTokens.Count > 0)
            {
                exponent = new Hoch(exponentTokens);
            }
        }
    }   
}
