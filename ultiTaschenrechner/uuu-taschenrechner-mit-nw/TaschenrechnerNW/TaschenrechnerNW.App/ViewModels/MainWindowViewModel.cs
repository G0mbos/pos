using Battleship.UI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using TaschenrechnerNW.API;
using TaschenrechnerNW.API.Models;

namespace TaschenrechnerNW.App.ViewModels
{
    public class MainWindowViewModel : ObservableObject, INetworkListener<Msg>
    {
        private string _input = "";

        public string Input
        {
            get => _input;
            set
            {
                SetProperty(ref _input, value);
                CalculateCommand.NotifyCanExecuteChanged();
            }
        }

        private string _output = "";

        public string Output
        {
            get => _output;
            set => SetProperty(ref _output, value);
        }

        public RelayCommand CalculateCommand { get; }

        private Transfer<Msg> _transfer;

        public MainWindowViewModel()
        {
            CalculateCommand = new RelayCommand(Calculate);
        }

        public void Calculate()
        {
            if (string.IsNullOrEmpty(Input))
            {
                MessageBox.Show("Please enter a calculation.");
                return;
            }
            MessageBox.Show($"Calculating: {Input}");

            try
            {
                if (_transfer == null)
                {
                    _transfer = new(new("localhost", 12345), this);
                }
                _transfer.Send(new Msg { Type = MessageType.Calculation, Content = Input });
            }
            catch
            {
                MessageBox.Show("Failed to connect to server.");
                return;
            }

            Output = "Calculating...";
        }

        public void OnMSG(Msg msg, Transfer<Msg> transfer)
        {
            if (msg.Type == MessageType.Response)
            {
                Application.Current.Dispatcher.Invoke(() => Output = msg.Content);
            }
            else if (msg.Type == MessageType.VariableRequest)
            {
                bool isInputValid = false;
                string dialogOutput = "";

                Application.Current.Dispatcher.Invoke(() => 
                {
                    while (!isInputValid)
                    {
                        ConfDialog confDialog = new ConfDialog("Enter Variable Value for " + msg.Content, "69");

                        confDialog.ShowDialog();
                        if (!confDialog.Canceled)
                        {
                            dialogOutput = confDialog.InputText;
                        }

                        isInputValid = double.TryParse(dialogOutput, out _);
                    }
                });
                transfer.Send(new Msg { Type = MessageType.VariableCompletion, Content = dialogOutput });
            }
        }

        public void OnDebug(string msg, Transfer<Msg> transfer)
        {
            Console.WriteLine(msg);
        }

        public void OnDisconnected(Transfer<Msg> transfer)
        {
            MessageBox.Show("Disconnected from server.");
        }
    }
}
