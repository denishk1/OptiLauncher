using Avalonia.Controls;
using Avalonia.Interactivity;
using OptiLauncher.ViewModels;

namespace OptiLauncher.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // Подключение ViewModel для обеспечения работоспособности биндингов
            DataContext = new MainWindowViewModel();
        }

        private void MinimizeWindow(object? sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeWindow(object? sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized 
                ? WindowState.Normal 
                : WindowState.Maximized;
        }

        private void CloseWindow(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}