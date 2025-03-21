using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Net;

public class Response
{
    public int NearbyMines { get; set; }
    public int Column { get; set; }
    public int Row { get; set; }
}
