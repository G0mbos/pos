using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static CSharpCalculator.Parser;

namespace CSharpCalculator.Expressions
{
    public class Variable : Expression
    {
        string placeholder;
        double value;
        public Variable(List<Token> tokens) : base(tokens) { }
        public override void Parse()
        {
            Token token = tokens.First();
            index = token.index;
            if (token.type == Token.Type.Variable)
            {
                placeholder = token.value;
                tokens.RemoveAt(0);
            }
            else
            {
                throw new SyntaxException(index);
            }
        }
        public override double Calculate()
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Please enter the value for the variable " + placeholder, "Input Required", "0");
            if (double.TryParse(input, out value))
            {
                return value;
            }
            else
            {
                MessageBox.Show("Invalid input. Please enter a numeric value.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
        }
    }
}
