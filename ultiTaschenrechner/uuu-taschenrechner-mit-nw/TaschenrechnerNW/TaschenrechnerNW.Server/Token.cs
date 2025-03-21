namespace TaschenrechnerNW.Server
{
    public enum TokenType { PlusMinus, MalDividiert, Hoch, Klammer, Ziffer, Variable, Komma }

    public class Token
    {
        public string value;
        public TokenType type;
    }
}
