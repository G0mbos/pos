using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpRobotServer.Grammatik.Tokenizer;
using CSharpRobotServer.ViewModel;

namespace CSharpRobotServer.Grammatik.Parser
{
    class Repeat : Expression
    {
        int _times;
        Block block;
        public Repeat(List<Token> tokens, MainWindowViewModel viewModel) : base(tokens, viewModel) { }
        public override void Parse()
        {
            tokens.RemoveAt(0);
            Token t = tokens.First();
            if (t.Type == Token.TokenType.NUMBER)
            {
                _times = int.Parse(t.Value);
                tokens.RemoveAt(0);
                t = tokens.First();
                if (t.Type == Token.TokenType.BRACKET && t.Value == "{")
                {
                    tokens.RemoveAt(0);
                    block = new Block(tokens, viewModel);
                    block.Parse();
                    if(tokens.First().Type == Token.TokenType.BRACKET && tokens.First().Value == "}")
                    {
                        tokens.RemoveAt(0);
                    }
                    else
                    {
                        throw new Exception("Expected } after repeat block");
                    }
                }
                else
                {
                    throw new Exception("Expected { after repeat number");
                }
            }
            else
            {
                throw new Exception("Expected number after repeat");
            }
        }
        public override async Task EvaluateAsync()
        {
            for(int i = 0; i < _times; i++)
            {
                await block.EvaluateAsync();
            }
        }
    }
}
