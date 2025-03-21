// main program

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaschenrechnerNW.API;
using TaschenrechnerNW.API.Models;

namespace TaschenrechnerNW.Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var server = new Server(12345);
            // Pause the program until ended
            await Task.Delay(-1);
        }
    }
}
