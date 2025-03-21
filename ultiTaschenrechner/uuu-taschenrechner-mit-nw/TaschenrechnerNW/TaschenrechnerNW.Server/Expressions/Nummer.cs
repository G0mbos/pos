using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaschenrechnerNW.Server.Expressions
{
    public class Nummer : MyExpression
    {
        public double Value { get => double.Parse(stringRepresentation); }

        private string stringRepresentation = "";
        private bool hasComma = false;

        public Nummer(List<Token> tokens) : base(tokens)
        {
        }

        public override void Parse()
        {
            foreach (Token t in tokens)
            {
                if (t.type == TokenType.Ziffer)
                {
                    stringRepresentation += t.value;
                }
                else if (t.type == TokenType.Komma)
                {
                    if (hasComma)
                    {
                        throw new SyntaxErrorException("Zahl hat mehr als ein Komma");
                    }
                    stringRepresentation += t.value;
                }
                else
                {
                    throw new SyntaxErrorException("Nummer enthält ungültige Zeichen");

                }
            }
        }

        public override double Evaluate()
        {
            if (!IsParsed)
            {
                Parse();
            }
            return Value;
        }
    }
}
