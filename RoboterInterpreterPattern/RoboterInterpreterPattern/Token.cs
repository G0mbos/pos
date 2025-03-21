using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboterInterpreterPattern;

public class Token
{
    public Type Typ { get; set; }
    public string Value { get; set; }


    public Token(string value, Type type) 
    { 
        this.Typ = type;
        this.Value = value;
    }

    public enum Type
    {
        STATEMENT,
        LOOP,
        COLLECT,
        MOVE,
        DIRECTION,
        DIGIT,
        BRACKET_BEGIN,
        BRACKET_END
    }

    // Mottl ist ein cooler Typ


}
