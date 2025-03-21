using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using Minesweeper.Net;
using System.Net.Sockets;
using System.Windows;
using System.Diagnostics;

namespace Minesweeper.App;

public partial class MainWindowViewModel : ObservableObject, INetworkListener<Msg>
{
    private int gameID;

    private TcpClient? _client;
    private Transfer<Msg>? _transfer;

    public ObservableCollection<Field>? Fields { get; set; } = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StartGameCommand))]
    private int size;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StartGameCommand))]
    private int minesCount;

    [RelayCommand(CanExecute = nameof(CanStartGame))]
    public void StartGame()
    {
        Debug.WriteLine("Debug meine Eier");

        Clear();

        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                Fields!.Add(new Field
                {
                    Column = i,
                    Row = j,
                    Value = -1
                });
            }
        }

        
        _client = new TcpClient("localhost", 12345);
        _transfer = new Transfer<Msg>(_client, this);

        var msg = new Msg
        {
            Type = MessageTypes.StartGame,
            StartGame = new StartGame
            {
                Size = Size,
                MinesCount = MinesCount
            }
        };

        _transfer.Send(msg);

    }

    [RelayCommand]
    public void ClickField(Field field)
    {
        Debug.WriteLine(field.Row);
        Debug.WriteLine(field.Column);

        if (field.Value >= 0)
        {
            return;
        }
        else
        {
            var msg = new Msg
            {
                Type = MessageTypes.Dig,
                Dig = new Dig
                {
                    GameId = gameID,
                    Column = field.Column,
                    Row = field.Row
                }
            };

            _transfer!.Send(msg);
        }
    }

    private bool CanStartGame()
    {
        return Size > 0 && MinesCount > 0;
    }

    public void OnDisconnected(Transfer<Msg> t)
    {
        throw new NotImplementedException();
    }

    public void OnMSG(Msg msg, Transfer<Msg> t)
    {
        if(msg.Type == MessageTypes.CreatedGame)
        {
            gameID = msg.CreatedGame!.GameId;
        }
        
        if (msg.Type == MessageTypes.Response)
        {
            var response = msg.Response!;
            var field = Fields!.First(f => f.Column == response.Column && f.Row == response.Row);
            if (response.NearbyMines == 9)
            {
                MessageBox.Show("Game Over");
            }
            else
                field.Value = response.NearbyMines;
        }
    }

    public void OnDebug(string msg, Transfer<Msg> t)
    {
        Debug.WriteLine(msg);
    }

    public void Clear()
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            Fields!.Clear();
            _transfer?.Close();
            _client?.Close();
        });
    }
}
