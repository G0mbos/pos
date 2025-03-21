using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CSharpRobotServer.Grammatik.Tokenizer;
using CSharpRobotServer.Grammatik.Parser;
using CSharpRobotServer.Model;
using System.ComponentModel;
using NetworkLibrary;
using System.Net.Sockets;
using System.Net;

namespace CSharpRobotServer.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged, NetworkLibrary.NetworkListener<MSG>
    {
        private SpaceValue previousColor = SpaceValue.EMPTY;
        List<List<Space>> _map;
        (int x, int y) _robotPosition;
        int _mapSize = 10;
        public List<List<Space>> Map
        {
            get { return _map; }
            set
            {
                _map = value;
                OnPropertyChanged(nameof(Map));
            }
        }
        private string _manualInput;
        public string ManualInput
        {
            get { return _manualInput; }
            set
            {
                _manualInput = value;
            }
        }
        private bool _isStartEnabled = true;
        public bool IsStartEnabled
        {
            get { return _isStartEnabled; }
            set
            {
                _isStartEnabled = value;
                OnPropertyChanged(nameof(IsStartEnabled));
            }
        }
        private bool _isResetEnabled = true;
        public bool IsResetEnabled
        {
            get { return _isResetEnabled; }
            set
            {
                _isResetEnabled = value;
                OnPropertyChanged(nameof(IsResetEnabled));
            }
        }
        private string _status = "Collected: ";
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
        public ICommand StartCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand OpenConnectionCommand { get; }
        private IPAddress _ip = IPAddress.Loopback;
        private int _port = 9669;
        private Transfer<NetworkLibrary.MSG> _transfer;
        private bool _isConnected = false;

        public MainWindowViewModel()
        {
            Map = new List<List<Space>>();
            StartCommand = new RelayCommand(async (obj) => await OnStart(ManualInput));
            ResetCommand = new RelayCommand(OnReset);
            OpenConnectionCommand = new RelayCommand(OpenConnection);
            GenerateMap();
        }

        private void GenerateMap()
        {
            var newMap = new List<List<Space>>();
            _robotPosition = (1, 1);
            for (int i = 0; i < _mapSize; i++)
            {
                newMap.Add(new List<Space>());
                for (int j = 0; j < _mapSize; j++)
                {
                    Space space = new Space() { X = i, Y = j, Value = SpaceValue.EMPTY };
                    if (i == _robotPosition.y && j == _robotPosition.x) space.Value = SpaceValue.ROBOT;
                    if ((i == 0 || j == 0)
                        || (i == _mapSize -1 || j == _mapSize -1)
                        || i == 4 && (j == 1 || j == 2)
                        || (i == 1 || i == 2 || i == 4 || i == 8) && j == 4
                        || (i == 5 || i == 6) && j == 5) space.Value = SpaceValue.OBSTACLE;
                    if (i == 7 && j == 1) space.Value = SpaceValue.RED;
                    if (i == 4 && j == 5) space.Value = SpaceValue.GREEN;
                    if (i == 8 && j == 5) space.Value = SpaceValue.BLUE;
                    newMap[i].Add(space);
                }
            }
            Map = newMap;
        }

        public bool CheckDirection(Direction direction, SpaceValue spaceValue)
        {
            int x = _robotPosition.x;
            int y = _robotPosition.y;
            switch (direction)
            {
                case Direction.UP:
                    if (y > 0) y--;
                    break;
                case Direction.DOWN:
                    if (y < _mapSize -1) y++;
                    break;
                case Direction.LEFT:
                    if (x > 0) x--;
                    break;
                case Direction.RIGHT:
                    if (x < _mapSize -1) x++;
                    break;
            }
            if (Map[y][x].Value == spaceValue) return true;
            return false;
        }

        public bool Move(Direction direction)
        {
            int x = _robotPosition.x;
            int y = _robotPosition.y;
            switch (direction)
            {
                case Direction.UP:
                    if (y > 0) y--;
                    break;
                case Direction.DOWN:
                    if (y < _mapSize -1) y++;
                    break;
                case Direction.LEFT:
                    if (x > 0) x--;
                    break;
                case Direction.RIGHT:
                    if (x < _mapSize - 1) x++;
                    break;
            }
            if (Map[y][x].Value == SpaceValue.OBSTACLE) return false;
            if (previousColor != SpaceValue.EMPTY)
            {
                Map[_robotPosition.y][_robotPosition.x].Value = previousColor;
                previousColor = SpaceValue.EMPTY;
            }
            else
            {
                Map[_robotPosition.y][_robotPosition.x].Value = SpaceValue.EMPTY;
            }
            if (Map[y][x].Value == SpaceValue.RED || Map[y][x].Value == SpaceValue.GREEN || Map[y][x].Value == SpaceValue.BLUE)
            {
                previousColor = Map[y][x].Value;
            }
            _robotPosition = (x, y);
            Map[_robotPosition.y][_robotPosition.x].Value = SpaceValue.ROBOT;
            return true;
        }

        private async Task OnStart(string input)
        {
            Status = "Collected: ";
            IsStartEnabled = false;
            IsResetEnabled = false;
            Tokenizer tokenizer = new Tokenizer(input);
            List<Token> tokens = tokenizer.Tokenize();
            Parser parser = new Parser(tokens, this);
            parser.Parse();
            if(!await parser.EvaluateAsync())
            {
                Status = "Error";
            }
            IsStartEnabled = true;
            IsResetEnabled = true;
            if (_isConnected)
            {
                NetworkLibrary.MSG msg = new NetworkLibrary.MSG { type = MSGType.RESPONSE, command = Status };
                _transfer.Send(msg);
                Console.WriteLine("Sent: " + Status);
            }
        }

        private void OnReset(Object o)
        {
            Status = "Collected: ";
            GenerateMap();
        }

        private void OpenConnection(Object o)
        {
            MessageBox.Show("Starting Server, please hand over your IP address and port to your opponent.");
            TcpListener listener = new TcpListener(_ip, _port);
            listener.Start();
            ThreadPool.QueueUserWorkItem(x =>
            {
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    _transfer = new Transfer<NetworkLibrary.MSG>(client, this);
                    Console.WriteLine("Connected");
                    _isConnected = true;
                }
            });
        }

        public void Collect()
        {
            if (previousColor == SpaceValue.RED)
                Status += "Red ";
            else if (previousColor == SpaceValue.GREEN)
                Status += "Green ";
            else if (previousColor == SpaceValue.BLUE)
                Status += "Blue ";
            else
                MessageBox.Show("Nothing to collect!");
            if (previousColor != SpaceValue.EMPTY)
                previousColor = SpaceValue.EMPTY;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //------------------------------NetworkListener--------------------------------
        public void OnDisconnected(Transfer<MSG> t)
        {
            Console.WriteLine("Disconnected");
            _isConnected = false;
        }

        public void OnMSG(MSG msg, Transfer<MSG> t)
        {
            switch (msg.type)
            {
                case MSGType.RESET:
                    GenerateMap();
                    break;
                case MSGType.COMMANDS:
                    OnStart(msg.command);
                    break;
            }
        }

        public void OnDebug(string msg, Transfer<MSG> t)
        {
            Console.WriteLine(msg);
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
