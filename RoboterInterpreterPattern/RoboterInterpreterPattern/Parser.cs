using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboterInterpreterPattern;

public class Parser
{

    public int X { get; set; }
    public int Y { get; set; }

    public Parser()
    {
        // start coordinates mocking a game field
        X = 0;
        Y = 0;
    }

    // parse the tokenlist and update the coordinates
    public void Parse(List<Token> tokens)
    {
        for(int i = 0; i < tokens.Count; i++)
        {
            Console.WriteLine(tokens[i].Typ + " | "+ tokens[i].Value);
            if (tokens[i].Typ == Token.Type.MOVE)
            {
                if (tokens[i + 1].Value == "LEFT") {
                    X--;
                }
                else if (tokens[i + 1].Value == "RIGHT")
                {
                    X++;
                }
                else if (tokens[i + 1].Value == "UP")
                {
                    Y++;
                }
                else if (tokens[i + 1].Value == "DOWN")
                {
                    Y--;
                }
                Console.WriteLine("X: " + this.X + " | Y: " + this.Y + " | Token: " + tokens[i+1].Value);
            }
            else if (tokens[i].Typ == Token.Type.LOOP)
            {
                Console.WriteLine("=========LOOP BEGIN");
                // get the digit after the loop
                int digit = int.Parse(tokens[i + 1].Value);
                if (tokens[i+2].Typ == Token.Type.BRACKET_BEGIN)
                {
                    int countvar = 0;
                    List<Token> loopTokenList = new List<Token>();
                    for (int j = i + 3; tokens[j].Typ != Token.Type.BRACKET_END; j++)
                    {
                        loopTokenList.Add(tokens[j]);
                        countvar = j;
                    }
                    i = countvar;

                    List<Token> newList = Enumerable.Repeat(loopTokenList, digit).SelectMany(x => x).ToList();
                    Parse(newList);

                }
                Console.WriteLine("=========LOOP END");
            }
        }
    }

}
