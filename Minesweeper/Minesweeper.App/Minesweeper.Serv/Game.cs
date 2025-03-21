using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Serv;

public class Game
{
    public int GameId { get; set; }
    public List<Mine> Mines { get; set; } = new List<Mine>();
}
