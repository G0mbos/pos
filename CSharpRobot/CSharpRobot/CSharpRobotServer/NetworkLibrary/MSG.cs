using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLibrary
{
    public enum MSGType
    {
        RESET,
        COMMANDS,
        RESPONSE
    }

    public class MSG
    {
        public MSGType type { get; set; }
        public string command { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append("<==MSG==>");
            sb.Append($"Type: {type} ");
            sb.Append($"Command: {command} ");
            return sb.ToString();
        }
    }
}
