using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaschenrechnerNW.Server.Expressions
{
    public class Variable : MyExpression
    {
        private string name = "";

        public Variable(List<Token> tokens) : base(tokens)
        {
        }

        public override double Evaluate()
        {
            if (!IsParsed)
            {
                Parse();
            }

            if (!VariableReference.Variables.TryGetValue(name, out double result))
            {
                Console.WriteLine($"THROWING ERR BECAUSE {name} DOES NOT EXIST");
                throw new NotImplementedException(name);
            }
            return result;
        }

        public override void Parse()
        {
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].type != TokenType.Variable)
                {
                    throw new SyntaxErrorException("Ungültige Variable");
                }
                name += tokens[i].value;
            }
        }
    }
}
