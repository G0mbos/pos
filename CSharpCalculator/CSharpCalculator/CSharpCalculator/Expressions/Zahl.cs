using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CSharpCalculator;
using static CSharpCalculator.Parser;

namespace CSharpCalculator.Expressions
{
    public class Zahl : Expression
    {
        bool hasComma = false;
        List<string> digits = new List<string>();
        public Zahl(List<Token> tokens) : base(tokens) { }
        public override void Parse()
        {
            Token token = tokens.First();
            index = token.index;
            while (token.type == Token.Type.Ziffer) // Ziffern einlesen
            {
                if (token.value == ",")
                {
                    if (hasComma) throw new SyntaxException(index);
                    hasComma = true;
                }
                digits.Add(token.value);
                tokens.RemoveAt(0);
                if (tokens.Count == 0) break;
                token = tokens.First();
                index = token.index;
            }
        }
        public override double Calculate()
        {
            return double.Parse(string.Join("", digits));
        }
    }
}
