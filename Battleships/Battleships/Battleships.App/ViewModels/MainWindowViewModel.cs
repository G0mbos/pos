using Battleships.API;
using Battleships.API.Models;
using Battleships.App.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net;
using System.Net.Sockets;
using System.Windows;

namespace Battleships.App.ViewModels
{
    public class MainWindowViewModel : ObservableObject, INetworkListener<Msg>
    {
        private bool _isGameReady = false;
        public bool IsGameReady 
        {
            get => _isGameReady;
            set
            {
                _isGameReady = value;
                OnPropertyChanged();
                OpponentClick.NotifyCanExecuteChanged();
                SelfClick.NotifyCanExecuteChanged();
            }
        }

        private bool _isMyTurn = true;

        public bool IsMyTurn
        {
            get => _isMyTurn;
            set
            {
                _isMyTurn = value;
                OnPropertyChanged();
                OpponentClick.NotifyCanExecuteChanged();
            }
        }


        public List<List<Ocean>> SelfGrid { get; }

        public List<List<Ocean>> OpponentGrid { get; set; }

        public RelayCommand<Ocean> SelfClick { get; set; }

        public RelayCommand<Ocean> OpponentClick { get; }

        public bool IsHorizontal { get; set; } = true;

        private readonly List<int> shipList = [5, 4, 4, 3, 3, 3, 2, 2, 2, 2];
        private readonly int gridSize = 10;
        private int currentShipIndex = 0;
        private Transfer<Msg> t;

        private bool isClient = false ;

        public MainWindowViewModel()
        {
            SelfClick = new(OnSelfClick, CanSelfClick);
            OpponentClick = new(OnOpponentClick, CanOpponentClick);

            //Fill with data
            SelfGrid = BuildMap();
            OpponentGrid = BuildMap();

            //Network
            if (isClient)
            {
                TcpClient client = new TcpClient("localhost", 12345);
                t = new Transfer<Msg>(client, this);
            } else
            {
                TcpListener listener = new(IPAddress.Any, 12345);
                listener.Start();
                ThreadPool.QueueUserWorkItem(x =>
                {
                    while (true)
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        Console.WriteLine("New Client connected!");
                        t = new Transfer<Msg>(client, this);
                    }
                });
                IsMyTurn = false;
            }
        }

        private bool CanOpponentClick(Ocean? ocean)
        {
            return IsGameReady && IsMyTurn;
        }

        private void OnOpponentClick(Ocean? ocean)
        {
            if (ocean is null)
                return;

            ocean.IsHit = true;

            Msg msg = new();
            msg.Type = PacketType.Shot;

            msg.X = ocean.Column;
            msg.Y = ocean.Row;
            t.Send(msg);

            IsMyTurn = false;
        }

        private bool CanSelfClick(Ocean? ocean)
        {
            return !IsGameReady;
        }

        private void OnSelfClick(Ocean? ocean)
        {
            if (ocean is null)
                return;

            if (CanPlaceShip(ocean))
            {
                PlaceShip(ocean);
                currentShipIndex++;
            }
            else
            {
                MessageBox.Show($"Ship cannto be placed here...");
            }

            // Check for Game Start
            if (currentShipIndex >= shipList.Count)
            {
                IsGameReady = true;
                OnPropertyChanged(nameof(IsGameReady));
                MessageBox.Show($"All Ships are placed, game is ready!");
            }

        }

        private void PlaceShip(Ocean ocean)
        {
            int length = shipList[currentShipIndex];

            if (IsHorizontal)
            {
                for (int i = ocean.Column; i < ocean.Column + length; i++)
                {
                    SelfGrid[ocean.Row][i].IsShip = true;
                }
            }
            else
            {
                for (int i = ocean.Row; i < ocean.Row + length; i++)
                {
                    SelfGrid[i][ocean.Column].IsShip = true;
                }

            }

            OnPropertyChanged();
        }


        private bool CanPlaceShip(Ocean ocean)
        {
            if (IsHorizontal)
            {
                if (ocean.Column + shipList[currentShipIndex] > gridSize) return false;
                for (int i = ocean.Column; i < ocean.Column + shipList[currentShipIndex]; i++)
                {
                    if (SelfGrid[ocean.Row][i].IsShip || IsAdjacentOccupied(ocean.Row, i))
                        return false;
                }
            }
            else
            {
                if (ocean.Row + shipList[currentShipIndex] > gridSize) return false;
                for (int i = ocean.Row; i < ocean.Row + shipList[currentShipIndex]; i++)
                {
                    if (SelfGrid[i][ocean.Column].IsShip || IsAdjacentOccupied(i, ocean.Column))
                        return false;
                }
            }
            return true;
        }

        private bool IsAdjacentOccupied(int row, int col)
        {
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i >= 0 && i < gridSize && j >= 0 && j < gridSize && SelfGrid[i][j].IsShip)
                        return true;
                }
            }
            return false;
        }

        private List<List<Ocean>> BuildMap()
        {
            var ocean = new List<List<Ocean>>();

            for (int row = 0; row < gridSize; row++)
            {
                var oceanRow = new List<Ocean>();
                for (int col = 0; col < gridSize; col++)
                {
                    oceanRow.Add(new Ocean() { Row = row, Column = col });
                }
                ocean.Add(oceanRow);
            }

            return ocean;
        }

        public void OnDisconnected(Transfer<Msg> t)
        {
            //throw new NotImplementedException();
        }

        public void OnMSG(Msg msg, Transfer<Msg> t)
        {
            if (IsMyTurn) // I am currently choosing a field
                return;

            if (msg.Type == PacketType.Shot)
            {
                Ocean field = SelfGrid[msg.Y][msg.X];
                field.IsHit = true;

                Msg response = new();
                response.Type = PacketType.ShotResponse;

                response.Hit = field.IsShip;
                response.X = msg.X;
                response.Y = msg.Y;

                t.Send(response);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    IsMyTurn = true;
                });
            }
            else if (msg.Type == PacketType.ShotResponse)
            {
                OpponentGrid[msg.Y][msg.X].IsShip = msg.Hit;
            }
        }

        public void OnDebug(string msg, Transfer<Msg> t)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }
    }
}
