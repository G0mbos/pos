using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VierGewinnt.Network
{
    public class MSG
    {
        public MessageType type { get; set; }
        public Pick? pick { get; set; }
        public LoadGame? loadgame { get; set; }
        public Response? response { get; set; }
    }

    public enum MessageType { PICK, LOADGAME, RESPONSE}

    public class Pick
    {
        public int column { get; set; }
        public int player { get; set; }
    }

    public class LoadGame
    {
        public int Columns { get; set; }
        public int Rows { get; set; }
        public int player { get; set; }
    }

    public class Response
    {
        public bool success { get; set; }
    }
}
