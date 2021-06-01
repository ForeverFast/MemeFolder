using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Navigation;
using System.Windows;
using System.Windows.Input;

namespace MemeFolder.ViewModels.Abstractions
{
    public class BaseWindowViewModel : BaseNavigationViewModel
    {

        #region Управление окном

        public ICommand MinimizedWindowCommand { get; }
        public ICommand ResizeWindowCommand { get; }
        public ICommand CloseWindowCommand { get; }
        public ICommand DragWindowCommand { get; }

        public virtual void MinimizedWindowMethod(object parameter) => Application.Current.MainWindow.WindowState = WindowState.Minimized;
        public virtual void ResizeWindowMethod(object parameter)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Normal)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }
        public virtual void CloseWindowMethod(object parameter) => Application.Current.MainWindow.Close();
        public virtual void DragWindowMethod(object parameter) => Application.Current.MainWindow.DragMove();

        #endregion


        public BaseWindowViewModel(INavigationService navigationService) : base(navigationService)
        {
            MinimizedWindowCommand = new RelayCommand(MinimizedWindowMethod, null);
            ResizeWindowCommand = new RelayCommand(ResizeWindowMethod, null);
            CloseWindowCommand = new RelayCommand(CloseWindowMethod, null);
            DragWindowCommand = new RelayCommand(DragWindowMethod, null);
        }

    }
}
