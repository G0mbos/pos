using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaschenrechnerNW.API;
using TaschenrechnerNW.API.Models;

namespace TaschenrechnerNW.Server
{
    public class Server : INetworkListener<Msg>
    {
        private TcpListener _listener;
        private Transfer<Msg> nwService;

        public Server(int port)
        {
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            _listener.Start();
            Console.WriteLine($"Server started listening on port {port} (localhost)");

            ThreadPool.QueueUserWorkItem(y =>
            {
                while (true)
                {
                    TcpClient client = _listener.AcceptTcpClient();
                    System.Diagnostics.Debug.WriteLine("Client connected!");
                    nwService = new Transfer<Msg>(client, this);

                    if (client.Connected)
                    {
                        Console.WriteLine("New client connected!");
                        break;
                    }
                }
            });
        }

        public void OnDebug(string msg, Transfer<Msg> transfer)
        {
            Console.WriteLine(msg);
        }

        public void OnDisconnected(Transfer<Msg> transfer)
        {
            Console.WriteLine("Disconnected from Client!");
        }

        public void OnMSG(Msg msg, Transfer<Msg> transfer)
        {
            Calculator calculator = new Calculator();

            if (msg.Type == MessageType.Calculation)
            {
                Console.WriteLine("Calculation: " + msg.Content);
                try
                {
                    transfer.Send(new Msg { Type = MessageType.Response, Content = calculator.Calculate(msg.Content) });
                    VariableReference.Reset();
                }
                catch (NotImplementedException e)
                {
                    VariableReference.UnfinishedCalculation = msg.Content;
                    VariableReference.UnfinishedVariable = e.Message;
                    transfer.Send(new Msg { Type = MessageType.VariableRequest, Content = e.Message});
                }
            }
            else if (msg.Type == MessageType.VariableCompletion)
            {
                Console.WriteLine("#######VariableCompletion: " + msg.Content);
                Console.WriteLine("Completing unfinished calculation: " + VariableReference.UnfinishedCalculation);
                VariableReference.Variables.Add(VariableReference.UnfinishedVariable, double.Parse(msg.Content));
                try
                {
                    transfer.Send(new Msg { Type = MessageType.Response, Content = calculator.Calculate(VariableReference.UnfinishedCalculation) });
                    VariableReference.Reset();
                }
                catch (NotImplementedException e)
                {
                    VariableReference.UnfinishedVariable = e.Message;
                    transfer.Send(new Msg { Type = MessageType.VariableRequest, Content = e.Message });
                }
            }
            else
            {
                Console.WriteLine(msg.Content);
            }
        }
    }
}
