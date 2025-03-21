using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class JsonTokenizer
{
    private static readonly Regex TokenPattern = new Regex(
        @"(\s+|[{[\]}:,]|true|false|null|""(\\.|[^""])*""|-?\d+(\.\d+)?([eE][+-]?\d+)?)",
        RegexOptions.Compiled);

    public static List<string> Tokenize(string json)
    {
        var tokens = new List<string>();
        foreach (Match match in TokenPattern.Matches(json))
        {
            string token = match.Value.Trim();
            if (!string.IsNullOrEmpty(token))
                tokens.Add(token);
        }
        return tokens;
    }
}

class JsonParser
{
    private List<string> tokens;
    private int position = 0;

    public JsonParser(List<string> tokens)
    {
        this.tokens = tokens;
    }

    public object Parse()
    {
        return ParseValue();
    }

    private object ParseValue()
    {
        if (Match("{")) return ParseObject();
        if (Match("[")) return ParseArray();
        if (Match("true")) return true;
        if (Match("false")) return false;
        if (Match("null")) return null;
        if (IsNumber(Peek())) return ParseNumber();
        if (IsString(Peek())) return ParseString();

        throw new Exception($"Unexpected token: {Peek()}");
    }

    private Dictionary<string, object> ParseObject()
    {
        var obj = new Dictionary<string, object>();

        while (!Match("}"))
        {
            string key = ParseString();
            Expect(":");
            object value = ParseValue();
            obj[key] = value;

            if (!Match(",")) break;
        }

        return obj;
    }

    private List<object> ParseArray()
    {
        var list = new List<object>();

        while (!Match("]"))
        {
            list.Add(ParseValue());
            if (!Match(",")) break;
        }

        return list;
    }

    private string ParseString()
    {
        string token = Expect();
        if (token.StartsWith("\"") && token.EndsWith("\""))
            return token.Substring(1, token.Length - 2);
        throw new Exception($"Invalid string: {token}");
    }

    private double ParseNumber()
    {
        string token = Expect();
        if (double.TryParse(token, out double result))
            return result;
        throw new Exception($"Invalid number: {token}");
    }

    private string Expect()
    {
        if (position < tokens.Count)
            return tokens[position++];
        throw new Exception("Unexpected end of input");
    }

    private void Expect(string expected)
    {
        if (!Match(expected))
            throw new Exception($"Expected '{expected}' but found '{Peek()}'");
    }

    private bool Match(string expected)
    {
        if (position < tokens.Count && tokens[position] == expected)
        {
            position++;
            return true;
        }
        return false;
    }

    private string Peek()
    {
        return position < tokens.Count ? tokens[position] : null;
    }

    private bool IsNumber(string token)
    {
        return double.TryParse(token, out _);
    }

    private bool IsString(string token)
    {
        return token.StartsWith("\"") && token.EndsWith("\"");
    }
}

class Program
{
    static void Main()
    {
        string json = "{ \"name\": \"Alice\", \"age\": 25, \"isStudent\": false, \"grades\": [90, 85, 88] }";

        List<string> tokens = JsonTokenizer.Tokenize(json);
        JsonParser parser = new JsonParser(tokens);
        var result = parser.Parse();

        PrintJson(result);
    }

    static void PrintJson(object obj, int indent = 0)
    {
        string indentStr = new string(' ', indent);

        if (obj is Dictionary<string, object> dict)
        {
            Console.WriteLine(indentStr + "{");
            foreach (var kvp in dict)
            {
                Console.Write(indentStr + "  \"" + kvp.Key + "\": ");
                PrintJson(kvp.Value, indent + 2);
            }
            Console.WriteLine(indentStr + "}");
        }
        else if (obj is List<object> list)
        {
            Console.WriteLine(indentStr + "[");
            foreach (var item in list)
                PrintJson(item, indent + 2);
            Console.WriteLine(indentStr + "]");
        }
        else
        {
            Console.WriteLine(indentStr + obj);
        }
    }
}