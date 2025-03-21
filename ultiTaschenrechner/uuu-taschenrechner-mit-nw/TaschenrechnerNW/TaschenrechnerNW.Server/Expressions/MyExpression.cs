using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaschenrechnerNW.Server.Expressions
{
    public abstract class MyExpression
    {
        public int index;
        protected List<Token> tokens;
        public bool IsParsed { get; protected set; }

        protected MyExpression(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public abstract void Parse();
        public abstract double Evaluate();
    }
}
