using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NetworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CSharpRobotClient.ViewModels
{
    public class MainWindowViewModel : ObservableObject, NetworkListener<NetworkLibrary.MSG>
    {
        public RelayCommand ConnectCommand { get; }
        public RelayCommand DisconnectCommand { get; }
        public RelayCommand SendCommand { get; }
        public RelayCommand ResetCommand { get; }
        private bool _canSend = false;
        public bool CanSend
        {
            get { return _canSend; }
            set
            {
                SetProperty(ref _canSend, value);
                SendCommand.NotifyCanExecuteChanged();
            }
        }
        private bool _canDisconnect = false;
        public bool CanDisconnect
        {
            get { return _canDisconnect; }
            set
            {
                SetProperty(ref _canDisconnect, value);
                DisconnectCommand.NotifyCanExecuteChanged();
            }
        }
        private bool _canReset = false;
        public bool CanReset
        {
            get { return _canReset; }
            set
            {
                SetProperty(ref _canReset, value);
                ResetCommand.NotifyCanExecuteChanged();
            }
        }
        private string _ip = "localhost:9669";
        public string IP
        {
            get { return _ip; }
            set { SetProperty(ref _ip, value); }
        }
        private string _input;
        public string Input
        {
            get { return _input; }
            set { SetProperty(ref _input, value); }
        }
        public string _status = "Not Connected";
        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }
        Transfer<NetworkLibrary.MSG> _transfer;
        public MainWindowViewModel()
        {
            ConnectCommand = new RelayCommand(OnConnect);
            DisconnectCommand = new RelayCommand(OnDisconnect, CanExecuteDisconnect);
            SendCommand = new RelayCommand(OnSend, CanExecuteSend);
            ResetCommand = new RelayCommand(OnReset, CanExecuteReset);
        }

        private bool CanExecuteDisconnect()
        {
            return CanDisconnect;
        }

        private bool CanExecuteSend()
        {
            return CanSend;
        }

        private bool CanExecuteReset()
        {
            return CanReset;
        }

        private void OnConnect()
        {
            if (!IP.Contains(":"))
            {
                MessageBox.Show("Invalid IP address. Please enter the IP address in the format 'xxx.xxx.xxx.xxx:yyyy'.");
                return;
            }

            string[] ipParts = IP.Split(':');
            if (ipParts.Length != 2)
            {
                MessageBox.Show("Invalid IP address. Please enter the IP address in the format 'xxx.xxx.xxx.xxx:yyyy'.");
                return;
            }
            try
            {
                TcpClient client = new TcpClient(ipParts[0], int.Parse(ipParts[1]));
                _transfer = new Transfer<NetworkLibrary.MSG>(client, this);
                Status = "Connected";
                CanSend = true;
                CanReset = true;
                CanDisconnect = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to the server: " + ex.Message);
                return;
            }
        }

        private void OnDisconnect()
        {
            _transfer.Close();
            Application.Current.Dispatcher.Invoke(() => //to not block the UI thread
            {
                CanSend = false;
                CanReset = false;
                CanDisconnect = false;
            });
        }

        private void OnSend()
        {
            if(string.IsNullOrEmpty(Input))
            {
                MessageBox.Show("Please enter commands to send to the robot.");
                return;
            }
            NetworkLibrary.MSG msg = new NetworkLibrary.MSG();
            msg.type = NetworkLibrary.MSGType.COMMANDS;
            msg.command = Input;
            _transfer.Send(msg);
            CanSend = false;
            CanReset = false;
        }

        private void OnReset()
        {
            NetworkLibrary.MSG msg = new NetworkLibrary.MSG();
            msg.type = NetworkLibrary.MSGType.RESET;
            _transfer.Send(msg);
            Status = "Connected";
        }

        public void OnDisconnected(Transfer<MSG> t)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                CanReset = false;
                CanSend = false;
                Status = "Not Connected";
            });
        }


        public void OnMSG(MSG msg, Transfer<MSG> t)
        {
            if(msg.type == NetworkLibrary.MSGType.RESPONSE)
            {
                Application.Current.Dispatcher.Invoke(() => //to not block the UI thread
                {
                    CanReset = true;
                    CanSend = true;
                    Status = msg.command;
                });
            }
        }

        public void OnDebug(string msg, Transfer<MSG> t)
        {
            Console.WriteLine(msg);
        }
    }
}
