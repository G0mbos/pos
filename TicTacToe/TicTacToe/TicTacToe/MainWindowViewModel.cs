using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NetworkLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;


namespace TicTacToe;

public partial class MainWindowViewModel : ObservableObject, INetworkListener<Msg>
{

    //TODO: XML Serialize(Save/Load Game), DataTemplate, SHOW X/O and Check Winner
    private bool isClient = false;

    private bool _isMyTurn = true;

    public bool IsMyTurn
    {
        get => _isMyTurn;
        set
        {
            _isMyTurn = value;
            OnPropertyChanged();
            CellClickCommand.NotifyCanExecuteChanged();
        }
    }

    private bool _isGameOver = false;
    public bool IsGameOver
    {
        get => _isGameOver;
        set
        {
            _isGameOver = value;
            OnPropertyChanged();
            CellClickCommand.NotifyCanExecuteChanged();
        }
    }

    public ObservableCollection<Cell> Fields { get; set; } = [];
    public int Size { get; set; } = 3;
    public RelayCommand<Cell> CellClickCommand { get; set; }

    public RelayCommand SaveCommand { get; set; }
    public RelayCommand LoadCommand { get; set; }

    private Transfer<Msg> t;

    public MainWindowViewModel()
    {
        CellClickCommand = new(OnClickCell, CanClickCell);
        SaveCommand = new(OnSaveCommand);
        LoadCommand = new(OnLoadCommand);

        Fields = BuildMap();

        if (isClient)
        {
            TcpClient client = new TcpClient("localhost", 12345);
            t = new Transfer<Msg>(client, this);
        }
        else
        {
            Debug.WriteLine("Server started");
            TcpListener listener = new(IPAddress.Any, 12345);
            listener.Start();
            ThreadPool.QueueUserWorkItem(x =>
            {
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    MessageBox.Show("Client connected");
                    t = new Transfer<Msg>(client, this);
                }
            });
            IsMyTurn = false;
        }
    }

    private ObservableCollection<Cell> BuildMap() {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                Cell c = new Cell();
                c.X = i;
                c.Y = j;
                c.Value = -1;
                Fields.Add(c);
            }
        }
        return Fields;
    }


    private bool CanClickCell(Cell? obj)
    {
        return obj is not null && IsMyTurn && obj.Value == -1;
    }

    private void OnClickCell(Cell? cell)
    {
        if (isClient)
        {
            cell.Value = 0;
            t.Send(new Msg { X = cell.X, Y = cell.Y, Value = cell.Value });
            IsMyTurn = false;
        }
        else
        {
            cell.Value = 1;
            t.Send(new Msg { X = cell.X, Y = cell.Y, Value = cell.Value });
            IsMyTurn = false;
        }

        int winner = checkWinner();

        if (winner == -1)
        {
            MessageBox.Show("Draw");
        }
        else if (winner == 0 || winner == 1)
        {
            MessageBox.Show("Winner is: " + winner);
        }
    }

    //write a function to check winner in tic tac toe
    public int checkWinner() {
        // Check rows and columns
        for (int i = 0; i < Size; i++)
        {
            // Check row
            if (Fields.Where(c => c.X == i).All(c => c.Value == 0))
                return 0;
            if (Fields.Where(c => c.X == i).All(c => c.Value == 1))
                return 1;

            // Check column
            if (Fields.Where(c => c.Y == i).All(c => c.Value == 0))
                return 0;
            if (Fields.Where(c => c.Y == i).All(c => c.Value == 1))
                return 1;
        }

        // Check main diagonal
        if (Fields.Where(c => c.X == c.Y).All(c => c.Value == 0))
            return 0;
        if (Fields.Where(c => c.X == c.Y).All(c => c.Value == 1))
            return 1;

        // Check anti-diagonal
        if (Fields.Where(c => c.X + c.Y == Size - 1).All(c => c.Value == 0))
            return 0;
        if (Fields.Where(c => c.X + c.Y == Size - 1).All(c => c.Value == 1))
            return 1;

        // Check for a draw
        if (Fields.All(c => c.Value != -1))
            return -1; // Indicating a draw

        return -2;
    }


    public void OnDisconnected(Transfer<Msg> t)
    {
        //throw new NotImplementedException();
    }

    public void OnMSG(Msg msg, Transfer<Msg> t)
    {
        int winner = checkWinner();

        if (winner == -1)
        {
            MessageBox.Show("Draw");
        }
        else if (winner == 0 || winner == 1)
        {
            MessageBox.Show("Winner is: " + winner);
        }
        else {
            Cell c = Fields.First(x => x.X == msg.X && x.Y == msg.Y);
            c.Value = msg.Value;

            Application.Current.Dispatcher.Invoke(() =>
            {
                IsMyTurn = true;
            });
        }
    }

    public void OnDebug(string msg, Transfer<Msg> t)
    {
       Debug.WriteLine(msg);
    }


    public void OnSaveCommand()
    {
        Debug.WriteLine("===============save");
        string filePath = "../MineSweeper.xml";
        XmlSerializer serializer = new(typeof(ObservableCollection<Cell>));
        using StreamWriter writer = new(filePath);
        serializer.Serialize(writer, Fields);
    }

    public void OnLoadCommand()
    {
        string filePath = "../MineSweeper.xml";
        if (!File.Exists(filePath)) return;

        XmlSerializer serializer = new(typeof(ObservableCollection<Cell>));
        using StreamReader reader = new(filePath);
        Fields = (ObservableCollection<Cell>)serializer.Deserialize(reader) ?? new();
    }
}
