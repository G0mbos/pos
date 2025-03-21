using Battleships.App.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Battleships.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            DataContext = this.ViewModel = viewModel;
            InitializeComponent();

            this.KeyDown += MainWindow_KeyDown;
        }

        public MainWindowViewModel ViewModel { get; }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.R && !this.ViewModel.IsGameReady)
            {
                this.ViewModel.IsHorizontal = !this.ViewModel.IsHorizontal;
            }
        }
    }
}