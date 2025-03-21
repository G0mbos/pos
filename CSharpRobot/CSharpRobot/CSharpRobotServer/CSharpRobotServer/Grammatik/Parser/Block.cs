using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpRobotServer.Grammatik.Tokenizer;
using CSharpRobotServer.ViewModel;

namespace CSharpRobotServer.Grammatik.Parser
{
    public class Block : Expression
    {
        List<Expression> expressions = new List<Expression>();
        public Block(List<Token> tokens, MainWindowViewModel viewModel) : base(tokens, viewModel) { }
        public override void Parse()
        {
            if(tokens.Count == 0)
                return;
            
            Token token = tokens.First();
            while (token.Type == Token.TokenType.REPEAT || token.Type == Token.TokenType.MOVE || token.Type == Token.TokenType.COLLECT || token.Type == Token.TokenType.CONDITIONER)
            {
                switch (token.Type)
                {
                    case Token.TokenType.MOVE:
                        expressions.Add(new Move(tokens, viewModel));
                        expressions.Last().Parse();
                        break;
                    case Token.TokenType.COLLECT:
                        expressions.Add(new Collect(tokens, viewModel));
                        expressions.Last().Parse();
                        break;
                    case Token.TokenType.REPEAT:
                        expressions.Add(new Repeat(tokens, viewModel));
                        expressions.Last().Parse();
                        break;
                    case Token.TokenType.CONDITIONER:
                        if(token.Value.Equals("IF"))
                            expressions.Add(new If(tokens, viewModel));
                        else
                            expressions.Add(new Until(tokens, viewModel));
                        expressions.Last().Parse();
                        break;
                }
                if (tokens.Count == 0) break;
                token = tokens.First();

            }
        }
        public override async Task EvaluateAsync()
        {
            foreach(var expression in expressions)
            {
                await expression.EvaluateAsync();
            }
        }
    }
}
