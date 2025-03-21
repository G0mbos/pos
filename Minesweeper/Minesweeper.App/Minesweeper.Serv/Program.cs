using Minesweeper.Serv;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        Server server = new Server();
        await Task.Delay(-1);
    }
}