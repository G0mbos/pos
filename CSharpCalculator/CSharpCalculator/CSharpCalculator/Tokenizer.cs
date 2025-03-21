using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace CSharpCalculator
{
    class Tokenizer
    {
        static Regex mathRegex = new Regex(@"[0-9a-zäöü\+\-\*/\(\)\,\^]");
        static Regex numberRegex = new Regex(@"[0-9\,]$");
        static Regex plusMinusRegex = new Regex(@"[\+\-]$");
        static Regex multiplyDivideRegex = new Regex(@"[\*/]$");
        static Regex powerRegex = new Regex(@"[\^]$");
        static Regex bracesRegex = new Regex(@"[\(\)]$");
        static Regex variableRegex = new Regex(@"[a-zA-ZäöüÄÖÜ]$");


        public static List<Token> tokenize(string mathExp)
        {
            if (mathExp is null || mathExp == string.Empty)
            {
                MessageBox.Show("Please enter a valid expression", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Token>();
            }
            List<Token> tokens = new List<Token>();
            MatchCollection matches = mathRegex.Matches(mathExp);

            int nonMatchIndex = mathExp.IndexOf(mathExp.FirstOrDefault(c => !mathRegex.IsMatch(c.ToString())));
            if (nonMatchIndex != -1)
            {
                MessageBox.Show($"{mathExp}\nSyntax Error at index {nonMatchIndex}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return tokens;
            }
            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];
                Token token = new Token
                {
                    value = match.Value,
                    index = i,
                    type = match.Value switch
                    {
                        var t when numberRegex.IsMatch(t) => Token.Type.Ziffer,
                        var t when plusMinusRegex.IsMatch(t) => Token.Type.PlusMinus,
                        var t when multiplyDivideRegex.IsMatch(t) => Token.Type.MalDividiert,
                        var t when powerRegex.IsMatch(t) => Token.Type.Hoch,
                        var t when bracesRegex.IsMatch(t) => Token.Type.Klammer,
                        var t when variableRegex.IsMatch(t) => Token.Type.Variable,
                    }
                };
                tokens.Add(token);
            }
            return tokens;
        }
    }
}
