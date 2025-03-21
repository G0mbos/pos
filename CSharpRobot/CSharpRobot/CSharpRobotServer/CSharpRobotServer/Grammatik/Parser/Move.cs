using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpRobotServer.Grammatik.Tokenizer;
using CSharpRobotServer.ViewModel;

namespace CSharpRobotServer.Grammatik.Parser
{
    public class Move : Expression
    {
        Direction direction;
        public Move(List<Token> tokens, MainWindowViewModel viewModel) : base(tokens, viewModel) { }
        public override void Parse()
        {
            tokens.RemoveAt(0);
            Token t = tokens.First();
            if (t.Type == Token.TokenType.DIRECTION)
            {
                direction = (Direction)Enum.Parse(typeof(Direction), t.Value);
                tokens.RemoveAt(0);
            }
            else
            {
                throw new Exception("Expected direction after move");
            }
        }
        public override async Task EvaluateAsync()
        {
            if(!viewModel.Move(direction))
            {
                throw new Exception("Can't move in that direction");
            }
            await Task.Delay(500);
        }
    }
}
