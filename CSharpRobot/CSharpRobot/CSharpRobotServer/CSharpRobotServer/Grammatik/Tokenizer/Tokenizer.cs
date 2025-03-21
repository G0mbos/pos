using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSharpRobotServer.Grammatik.Tokenizer
{
    public class Tokenizer
    {
        string input;
        List<Token> tokens = new List<Token>();
        Regex repeatRegex = new Regex(@"^REPEAT$");
        Regex numberRegex = new Regex(@"^\d+$");
        Regex bracketRegex = new Regex(@"^[\{\}]$");
        Regex moveRegex = new Regex(@"^MOVE$");
        Regex directionRegex = new Regex(@"^(UP|DOWN|LEFT|RIGHT)$");
        Regex collectRegex = new Regex(@"^COLLECT$");
        Regex conditionerRegex = new Regex(@"^IF|UNTIL$");
        Regex isaRegex = new Regex(@"^IS-A$");
        Regex elementRegex = new Regex(@"^(RED|GREEN|BLUE|OBSTACLE)$");
        public Tokenizer(string input) { this.input = input; }

        public List<Token> Tokenize()
        {
            if(input is null || input == string.Empty)
                return tokens;
            tokens = new List<Token>();
            string[] words = input.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries); // split by each word

            foreach (var word in words)
            {
                if (repeatRegex.IsMatch(word))
                {
                    tokens.Add(new Token { Type = Token.TokenType.REPEAT, Value = word });
                }
                else if (numberRegex.IsMatch(word))
                {
                    tokens.Add(new Token { Type = Token.TokenType.NUMBER, Value = word });
                }
                else if (bracketRegex.IsMatch(word))
                {
                    tokens.Add(new Token { Type = Token.TokenType.BRACKET, Value = word });
                }
                else if (moveRegex.IsMatch(word))
                {
                    tokens.Add(new Token { Type = Token.TokenType.MOVE, Value = word });
                }
                else if (directionRegex.IsMatch(word))
                {
                    tokens.Add(new Token { Type = Token.TokenType.DIRECTION, Value = word });
                }
                else if (collectRegex.IsMatch(word))
                {
                    tokens.Add(new Token { Type = Token.TokenType.COLLECT, Value = word });
                }
                else if (conditionerRegex.IsMatch(word))
                {
                    tokens.Add(new Token { Type = Token.TokenType.CONDITIONER, Value = word });
                }
                else if (isaRegex.IsMatch(word))
                {
                    tokens.Add(new Token { Type = Token.TokenType.ISA, Value = word });
                }
                else if (elementRegex.IsMatch(word))
                {
                    tokens.Add(new Token { Type = Token.TokenType.ELEMENT, Value = word });
                }
                else throw new Exception("Unknown token: " + word);
            }
            return tokens;
        }
    }
}
