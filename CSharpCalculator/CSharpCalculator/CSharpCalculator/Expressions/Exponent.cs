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
    public class Exponent : Expression
    {
        bool _isNegative = false;
        Zahl zahl;
        Variable? variable;
        Klammer? klammer;
        public Exponent(List<Token> tokens) : base(tokens) { }
        public override void Parse()
        {
            if (tokens.Count == 0) throw new SyntaxException(-69);
            Token token = tokens.First();
            index = token.index;
            if (token.type == Token.Type.PlusMinus && token.value == "-")
            {
                _isNegative = true;
                tokens.RemoveAt(0);
                token = tokens.First();
                index = token.index;
            }
            else if (token.type == Token.Type.Klammer)
            {
                klammer = new Klammer(tokens);
                klammer.Parse();
            }
            else if (token.type == Token.Type.Ziffer)
            {
                zahl = new Zahl(tokens);
                zahl.Parse();
            }
            else if (token.type == Token.Type.Variable)
            {
                variable = new Variable(tokens);
                variable.Parse();
            }
            else
            {
                throw new SyntaxException(token.index);
            }
        }

        public override double Calculate()
        {
            double result = 0;
            if (klammer != null)
            {
                result = klammer.Calculate();
            }
            else if(variable != null)
            {
                result = variable.Calculate();
            }
            else
            {
                result = zahl.Calculate();
            }
            if (_isNegative) result *= -1;

            return result;
        }
    }
}
