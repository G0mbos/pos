using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CSharpRobotServer.Grammatik.Tokenizer;
using CSharpRobotServer.ViewModel;

namespace CSharpRobotServer.Grammatik.Parser
{
    class Parser
    {
        private List<Token> tokens;
        private MainWindowViewModel viewModel;
        Block block;
        public Parser(List<Token> tokens, MainWindowViewModel viewModel) { this.tokens = tokens; this.viewModel = viewModel; }
        public void Parse()
        {
            block = new Block(tokens, viewModel);
            block.Parse();
        }
        public async Task<bool> EvaluateAsync()
        {
            try
            {
                await block.EvaluateAsync();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
