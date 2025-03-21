using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CSharpCalculator.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _inputField;
        public string InputField
        {
            get { return _inputField; }
            set
            {
                _inputField = value;
                OnPropertyChanged(nameof(InputField));
            }
        }

        public ICommand InputCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand CalculateCommand { get; }
        private List<Token> tokens;

        public MainWindowViewModel()
        {
            InputCommand = new RelayCommand<string>(OnInput);
            BackCommand = new RelayCommand<object>(OnBack);
            CalculateCommand = new RelayCommand<object>(OnCalculate);
        }

        private void OnInput(string input)
        {
            InputField += input;
        }

        private void OnBack(object obj)
        {
            if (InputField.Length > 0)
                InputField = InputField.Remove(InputField.Length - 1);
        }

        private void OnCalculate(object obj)
        {
            tokens = Tokenizer.tokenize(InputField);
            for (int i = 0; i < tokens.Count; i++)
            {
                Console.WriteLine(tokens[i].type + " " + tokens[i].value);
            }
            Parser parser = new Parser(tokens);
            CSharpCalculator.Expressions.Expression ex = parser.Parse();
            if(ex != null)
            {
                InputField = ex.Calculate().ToString();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
