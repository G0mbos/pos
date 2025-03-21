using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCalculator.Expressions
{
    public abstract class Expression
    {
        public int index;
        protected List<Token> tokens;
        public Expression(List<Token> tokens)
        {
            this.tokens = tokens;
        }
        public abstract void Parse();
        public abstract double Calculate();
    }
}
