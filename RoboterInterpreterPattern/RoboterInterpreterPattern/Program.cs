using RoboterInterpreterPattern;
using System.Runtime.Serialization.Json;

class Program
{
    public static void Main(string[] args)
    {
        List<Token> tokens = new List<Token>();

        string input;
        input = ReadFile("../../../input.txt");
        Tokenizer tokenizer = new Tokenizer();
        tokens = tokenizer.Tokenize(input);

        foreach (var token in tokens)
        {
            Console.WriteLine("Type " + token.Typ + " | Value: " + token.Value);
        }

        Parser parser = new Parser();
        parser.Parse(tokens);
    }

    private static string ReadFile(string filePath)
    {
        try
        {
            return File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            return "Error reading file: " + ex.Message;
        }
    }

}