using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace VierGewinnt.Server
{
    class Server : INetworkListener<MSG>
    {
        public int _columns { get; set; } = 4;
        public int _rows { get; set; } = 4;
        public int _player { get; set; } = 2;

        private readonly TcpListener _listener;

        public Server(int port)
        {
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            _listener.Start();
            Console.WriteLine("Server gestartet");
            AcceptClients();

        }

        public void AcceptClients()
        {
            while (true)
            {
                var client = _listener.AcceptTcpClient();
                Console.WriteLine("Client verbunden");
                _ = new Transfer<MSG>(client, (INetworkListener<MSG>)this);
            }
        }

        public void OnDisconnected(Transfer<MSG> t)
        {
            Console.WriteLine("Disconnected");
        }

        public void OnMSG(MSG msg, Transfer<MSG> t)
        {
            if (msg.type == MessageType.LOADGAME)
            {
                _columns = msg.loadgame.Columns;
                _rows = msg.loadgame.Rows;
                _player = msg.loadgame.player;
                Console.WriteLine("Game loaded");
                Console.WriteLine("Columns: " + _columns);
                Console.WriteLine("Rows: " + _rows);
                Console.WriteLine("Player: " + _player);
            }
            else if (msg.type == MessageType.PICK)
            {
                Console.WriteLine("Player " + msg.pick.player + " picked column " + msg.pick.column);
                Random rnd = new Random();
                int col;


                col = rnd.Next(0, _columns);

                var response = new MSG
                {
                    type = MessageType.PICK,
                    pick = new Pick
                    {
                        column = col,
                        player = _player
                    }
                };

                t.Send(response);
            }
            else if (msg.type == MessageType.RESPONSE && msg.response.success == false)
            {
                Console.WriteLine("Serverpick was not successful -- try again");
                Random rnd = new Random();
                int col;


                col = rnd.Next(0, _columns);

                var response = new MSG
                {
                    type = MessageType.PICK,
                    pick = new Pick
                    {
                        column = col,
                        player = _player
                    }
                };

                t.Send(response);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void OnDebug(string msg, Transfer<MSG> t)
        {
            //throw new NotImplementedException();
        }
    }
}
