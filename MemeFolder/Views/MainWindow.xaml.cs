using MemeFolder.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MemeFolder.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IServiceProvider _serviceProvider;

        public MainWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = _serviceProvider.GetService(typeof(MainWindowVM));

        }

        private void empListBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer sv = (ScrollViewer)sender;
            double offset = sv.ContentHorizontalOffset + (e.Delta / 120);
            sv.ScrollToHorizontalOffset(offset);
        }
    }
}
