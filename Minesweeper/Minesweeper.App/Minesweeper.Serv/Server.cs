using Minesweeper.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Serv;

class Server : INetworkListener<Msg>
{
    TcpListener _listener;
    public List<Game> Games = new List<Game>();

    public Server()
    {
        _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 12345);
        _listener.Start();
        Console.WriteLine($"Server started listening on port {12345} (localhost)");
        AcceptClients();
    }
    public async void AcceptClients()
    {
        while (true)
        {
            var client = await _listener.AcceptTcpClientAsync();
            _ = new Transfer<Msg>(client, this);
        }
    }

    public void OnDebug(string msg, Transfer<Msg> t)
    {
        Debug.WriteLine(msg);
        Console.WriteLine(msg);
    }
    public void OnDisconnected(Transfer<Msg> t)
    {
        //throw new NotImplementedException();
    }
    public void OnMSG(Msg msg, Transfer<Msg> t)
    {
        if (msg.Type == MessageTypes.StartGame)
        {
            
            var startGame = msg.StartGame!;
            var size = startGame.Size;
            var minesCount = startGame.MinesCount;
            var game = new Game();
  
            game.GameId = new Random().Next(1000000);

            for (int i = 0; i < minesCount; i++)
            {
                int col = new Random().Next(size);
                int row = new Random().Next(size);
                
                game.Mines.Add(new Mine
                {
                    Column = col,
                    Row = row
                });
            }

            Games.Add(game);

            var responseMsg = new Msg
            {
                Type = MessageTypes.CreatedGame,
                CreatedGame = new CreatedGame
                {
                    GameId = game.GameId
                }
            };
            t.Send(responseMsg);
        }
        else if (msg.Type == MessageTypes.Dig)
        {
            var dig = msg.Dig!;
            var column = dig.Column;
            var row = dig.Row;

            var game = Games.Find(g => g.GameId == dig.GameId);

            if (game == null)
            {
                return;
            }


            if(game.Mines.Any(m => m.Row == msg.Dig!.Row && m.Column == msg.Dig!.Column))
            {
                var responseMsg = new Msg
                {
                    Type = MessageTypes.Response,
                    Response = new Response
                    {
                        NearbyMines = 9
                    }
                };
                t.Send(responseMsg);
                return;
            }

            var nearbyMines = game.Mines.Count(m => Math.Abs(m.Row - row) <= 1 && Math.Abs(m.Column - column) <= 1);
            var response = new Response
            {
                NearbyMines = nearbyMines,
                Column = column,
                Row = row
            };
            t.Send(new Msg
            {
                Type = MessageTypes.Response,
                Response = response
            });
        }
    }
}