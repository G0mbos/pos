using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaschenrechnerNW.Server.Expressions;

namespace TaschenrechnerNW.Server
{
    public class Parser
    {
        private MyExpression expression;
        public MyExpression Parse(List<Token> tokens)
        {
            Console.WriteLine("Anzahl Tokens: " + tokens.Count);
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
                    case TokenType.Ziffer:
                    case TokenType.PlusMinus: // falls ein Minus vor einer Zahl steht
                    case TokenType.Klammer: // falls eine Klammer aufgemacht wir
                    case TokenType.Variable:
                        expression = new PlusMinus(tokens);
                        expression.Parse();
                        break;
                    default:
                        throw new Exception();
                }
                return expression;
            }
            catch (SyntaxErrorException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            catch (NotImplementedException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
