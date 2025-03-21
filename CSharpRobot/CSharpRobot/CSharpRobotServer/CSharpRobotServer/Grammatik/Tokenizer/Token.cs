using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpRobotServer.Grammatik.Tokenizer
{
    public class Token
    {
        public enum TokenType { REPEAT, NUMBER, BRACKET, MOVE, DIRECTION, COLLECT, CONDITIONER, ISA, ELEMENT }
        public TokenType Type { get; set; }
        public string Value { get; set; }
    }
}
