using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.API.Models
{
    public enum PacketType
    {
        Configuration, Field, Shot, ShotResponse, Finish
    }
}
