using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using VierGewinnt.Model;
using VierGewinnt.Network;

namespace VierGewinnt.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject, INetworkListener<MSG>
    {
        private bool _myTurn = true;
        private int _player = 1;
        private TcpClient _client;
        private Transfer<MSG> _transfer;

        private int column = 4;
        public int Column
        {
            get => column;
            set => SetProperty(ref column, value);
        }

        private int row = 4;
        public int Row
        {
            get => row;
            set => SetProperty(ref row, value);
        }


        [ObservableProperty]
        private ObservableCollection<Cell> field = new();

        // RelayCommand für das Setzen des Zugs
        public RelayCommand<int> MakeMoveCommand { get; }


        public MainWindowViewModel()
        {
            MakeMoveCommand = new RelayCommand<int>(OnMakeMove, CanMakeMove);

        }

        private bool CanMakeMove(int col)
        {

            return field.Any(c => c.X == col && c.Status == 0 && _myTurn == true); 
        }

        private void OnMakeMove(int col)
        {

            var emptyRow = Enumerable.Range(0, Row)  
                                     .Reverse()  
                                     .FirstOrDefault(r => !field.Any(c => c.X == col && c.Y == r && c.Status != 0));
            if (_myTurn == true)
            {
                if (emptyRow < Row)
                {
                    var cell = field.FirstOrDefault(c => c.X == col && c.Y == emptyRow);
                    if (cell != null)
                    {
                        cell.Status = _player;
                        _myTurn = false;
                    }

                    var ms = new MSG
                    {
                        type = MessageType.PICK,
                        pick = new Pick
                        {
                            column = col,
                            player = _player
                        }
                    };
                    _transfer.Send(ms);
                }
                
            }
        }








        public void StartGame()
        {
            field.Clear();
            BuildMap();

            _client = new TcpClient("localhost", 5000);
            _transfer = new Transfer<MSG>(_client, this);

            var msg = new MSG 
            { 
                type = MessageType.LOADGAME, 
                loadgame = new LoadGame 
                {
                    Columns = column, 
                    Rows = row,
                    player = _player == 1 ? 2 : 1
                } 
            };
            _transfer.Send(msg);
        }

        public void BuildMap()
        {
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    field.Add(new Cell { X = j, Y = i });
                }
            }
        }

        public void OnDisconnected(Transfer<MSG> t)
        {
            MessageBox.Show("Disconnected");
        }

        public void OnMSG(MSG msg, Transfer<MSG> t)
        {
            if (msg.type == MessageType.PICK)
            {
                var emptyRow = Enumerable.Range(0, Row)
                                     .Reverse()
                                     .FirstOrDefault(r => !field.Any(c => c.X == msg.pick.column && c.Y == r && c.Status != 0));
                if (_myTurn != true && emptyRow < Row)
                {
                    var cell = field.FirstOrDefault(c => c.X == msg.pick.column && c.Y == emptyRow);
                    if (cell.Status == 0)
                    {
                        cell.Status = msg.pick.player;
                        _myTurn = true;
                    }
                    else
                    {
                        var ms = new MSG
                        {
                            type = MessageType.RESPONSE,
                            response = new Response
                            {
                                success = false
                            }
                        };
                        t.Send(ms);
                    }
                }
                else
                {
                    var ms = new MSG
                    {
                        type = MessageType.RESPONSE,
                        response = new Response
                        {
                            success = false
                        }
                    };
                    t.Send(ms);
                }
            }
        }

        public void OnDebug(string msg, Transfer<MSG> t)
        {
            //throw new NotImplementedException();
        }
    }
}
