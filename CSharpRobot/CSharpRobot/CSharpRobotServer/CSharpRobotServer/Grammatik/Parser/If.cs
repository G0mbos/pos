using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpRobotServer.Grammatik.Tokenizer;
using CSharpRobotServer.ViewModel;
using CSharpRobotServer.Model;

namespace CSharpRobotServer.Grammatik.Parser
{
    class If : Expression
    {
        Direction direction;
        Block block;
        SpaceValue value;
        public If(List<Token> tokens, MainWindowViewModel viewModel) : base(tokens, viewModel) { }
        public override void Parse()
        {
            tokens.RemoveAt(0);
            Token t = tokens.First();
            if (t.Type == Token.TokenType.DIRECTION)
            {
                direction = (Direction)Enum.Parse(typeof(Direction), t.Value);
                tokens.RemoveAt(0);
                t = tokens.First();
                if (t.Type == Token.TokenType.ISA)
                {
                    tokens.RemoveAt(0);
                    t = tokens.First();
                    if (t.Type == Token.TokenType.ELEMENT)
                    {
                        value = (SpaceValue)Enum.Parse(typeof(SpaceValue), t.Value);
                        tokens.RemoveAt(0);
                        t = tokens.First();
                        if (t.Type == Token.TokenType.BRACKET && t.Value == "{")
                        {
                            tokens.RemoveAt(0);
                            block = new Block(tokens, viewModel);
                            block.Parse();
                            if (tokens.First().Type == Token.TokenType.BRACKET && tokens.First().Value == "}")
                            {
                                tokens.RemoveAt(0);
                            }
                            else
                            {
                                throw new Exception("Expected } after if block");
                            }
                        }
                        else
                        {
                            throw new Exception("Expected { after if direction");
                        }
                    }
                    else
                    {
                        throw new Exception("Expected element after ISA");
                    }
                }
                else
                {
                    throw new Exception("Expected ISA after direction");
                }
            }
            else
            {
                throw new Exception("Expected direction after move");
            }
        }
        public override async Task EvaluateAsync()
        {
            if(viewModel.CheckDirection(direction, value))
            {
                await block.EvaluateAsync();
            }
        }
    }
}
