using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Net;

public class Msg
{
    public MessageTypes Type { get; set; }
    public Dig? Dig { get; set; }
    public CreatedGame? CreatedGame { get; set; }
    public Response? Response { get; set; }
    public StartGame? StartGame { get; set; }
}
