using CSharpCalculator.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CSharpCalculator
{
    public class Parser
    {

        List<Token> tokens;
        CSharpCalculator.Expressions.Expression expression;
        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public CSharpCalculator.Expressions.Expression Parse()
        {
            string input = string.Join("", tokens.Select(t => t.value));
            try
            {
                if (tokens.Count == 0)
                {
                    return null;
                }
                Token token;
                token = tokens.First();
                switch (token.type)
                {
                    case Token.Type.Ziffer:
                    case Token.Type.PlusMinus: // falls ein Minus vor einer Zahl steht
                    case Token.Type.Klammer: // falls eine Klammer aufgemacht wir
                    case Token.Type.Variable:
                        expression = new PlusMinus(tokens);
                        expression.Parse();
                        break;
                    default:
                        throw new Exception();
                }
                if(tokens.Count > 0)
                {
                    throw new SyntaxException(-69);
                }
                return expression;
            }
            catch (SyntaxException e)
            {
                if(e.ErrorIndex == -69)
                {
                    MessageBox.Show("Unexpected End of Input", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show($"{input}\nSyntax Error at index {e.ErrorIndex - 1}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return null;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unexpected Error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public class SyntaxException : Exception
        {
            public int ErrorIndex { get; private set; }

            public SyntaxException(int errorIndex) : base("Syntax Error")
            {
                ErrorIndex = errorIndex;
            }
        }
    }
}
