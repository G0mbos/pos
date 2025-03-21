using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.API.Models
{
    public class Msg
    {
        public PacketType Type { get; set; }

        // Config
        public int Start { get; set; } // 0 Server, 1 Client

        // Shot
        public int X { get; set; }
        public int Y { get; set; }

        // Shot Response
        public bool Hit { get; set; }
        public bool Sunk { get; set; } // Sunk = True if every part of the Ship is hit

        // Finish
        public int Winner { get; set; } // 0 Server, 1 Client (Server calculates winner)

        public override string ToString()
        {
            StringBuilder str = new();
            str.AppendLine($"===== New Message =====");
            str.AppendLine($"Packet Type: {Type}");
            str.AppendLine($"Start: {Start}");
            str.AppendLine($"X: {X}; Y: {Y}");
            str.AppendLine($"Hit: {Hit}");
            str.AppendLine($"Sunk: {Sunk}");
            str.AppendLine($"Winner: {Winner}");
            return str.ToString();
        }
    }
}
