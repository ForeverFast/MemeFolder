using MemeFolder.MVVM.ViewModels;
using System;
using System.Windows;


namespace MemeFolder.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindowV : Window
    {
        private IServiceProvider _serviceProvider;

        public MainWindowV(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            Loaded += MainWindowV_Loaded;
        }

        private void MainWindowV_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = _serviceProvider.GetService(typeof(MainWindowVM));
        }
    }
}
