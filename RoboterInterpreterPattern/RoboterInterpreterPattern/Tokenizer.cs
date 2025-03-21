using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboterInterpreterPattern;

public class Tokenizer
{
    List<Token> tokens;

    public Tokenizer()
    {
        tokens = new List<Token>();
    }


    // Tokenize the input
    public List<Token> Tokenize(string input)
    {
        foreach (var line in input.Split('\n'))
        {
            foreach (var word_ in line.Split(' '))
            {
                string word = word_.Trim();
                if(word == "" || word == " ")
                {
                    continue;
                }
                if (word == "MOVE")
                {
                    tokens.Add(new Token(word, Token.Type.MOVE));
                }
                else if (word == "COLLECT")
                {
                    tokens.Add(new Token(word, Token.Type.COLLECT));
                }
                else if (word == "REPEAT")
                {
                    tokens.Add(new Token(word, Token.Type.LOOP));
                }
                else if (word == "UP" || word == "DOWN" || word == "RIGHT" || word == "LEFT")
                {
                    tokens.Add(new Token(word, Token.Type.DIRECTION));
                }
                else if (int.TryParse(word, out _))
                {
                    tokens.Add(new Token(word, Token.Type.DIGIT));
                }
                else if (word == "{")
                {
                    tokens.Add(new Token(word, Token.Type.BRACKET_BEGIN));
                }
                else if (word == "}")
                {
                    tokens.Add(new Token(word, Token.Type.BRACKET_END));
                }
                else
                {
                    tokens.Add(new Token(word, Token.Type.STATEMENT));
                }
            }
        }
        return tokens;
    }
}
