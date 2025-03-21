using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TaschenrechnerNW.Server
{
    public class Tokenizer
    {
        static Regex mathRegex = new Regex(@"[0-9a-z\+\-\*/\(\)\,\^]");
        static Regex digitRegex = new Regex(@"[0-9]$");
        static Regex commaRegex = new Regex(@"[\,]$");
        static Regex plusMinusRegex = new Regex(@"[\+\-]$");
        static Regex multiplyDivideRegex = new Regex(@"[\*/]$");
        static Regex powerRegex = new Regex(@"[\^]$");
        static Regex bracesRegex = new Regex(@"[\(\)]$");
        static Regex variableRegex = new Regex(@"[a-zA-Z]$");

        public static List<Token> Tokenize(string calculation)
        {
            calculation = calculation.Replace(" ", "");
            List<Token> tokens = new List<Token>();
            mathRegex.Matches(calculation).ToList().ForEach(m =>
            {
                if (digitRegex.IsMatch(m.Value))
                {
                    tokens.Add(new Token { value = m.Value, type = TokenType.Ziffer });
                }
                else if (commaRegex.IsMatch(m.Value))
                {
                    tokens.Add(new Token { value = m.Value, type = TokenType.Komma });
                }
                else if (plusMinusRegex.IsMatch(m.Value))
                {
                    tokens.Add(new Token { value = m.Value, type = TokenType.PlusMinus });
                }
                else if (multiplyDivideRegex.IsMatch(m.Value))
                {
                    tokens.Add(new Token { value = m.Value, type = TokenType.MalDividiert });
                }
                else if (powerRegex.IsMatch(m.Value))
                {
                    tokens.Add(new Token { value = m.Value, type = TokenType.Hoch });
                }
                else if (bracesRegex.IsMatch(m.Value))
                {
                    tokens.Add(new Token { value = m.Value, type = TokenType.Klammer });
                }
                else if (variableRegex.IsMatch(m.Value))
                {
                    tokens.Add(new Token { value = m.Value, type = TokenType.Variable });
                }
            });
            return tokens;
        }
    }
}
