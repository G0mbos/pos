using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpRobotServer.Grammatik.Tokenizer;
using CSharpRobotServer.ViewModel;

namespace CSharpRobotServer.Grammatik.Parser
{
    public abstract class Expression
    {
        protected MainWindowViewModel viewModel;
        protected List<Token> tokens = new List<Token>();
        public Expression(List<Token> tokens, MainWindowViewModel viewModel) { this.tokens = tokens; this.viewModel = viewModel; }
        public abstract void Parse();
        public abstract Task EvaluateAsync();
    }
}
