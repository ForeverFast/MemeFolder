using MemeFolder.MVVM.ViewModels;
using System.Windows;


namespace MemeFolder.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindowV : Window
    {
        public MainWindowV(MainWindowVM mainWindowVM)
        {
            InitializeComponent();
            DataContext = mainWindowVM;
        }
    }
}
