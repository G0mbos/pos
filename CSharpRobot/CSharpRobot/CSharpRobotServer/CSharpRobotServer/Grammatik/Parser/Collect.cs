using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpRobotServer.Grammatik.Tokenizer;
using CSharpRobotServer.ViewModel;

namespace CSharpRobotServer.Grammatik.Parser
{
    public class Collect : Expression
    {
        public Collect(List<Token> tokens, MainWindowViewModel viewModel) : base(tokens, viewModel) { }
        public override void Parse()
        {
            tokens.RemoveAt(0);
        }
        public override async Task EvaluateAsync()
        {
            viewModel.Collect();
            await Task.Delay(500);
        }
    }
}
