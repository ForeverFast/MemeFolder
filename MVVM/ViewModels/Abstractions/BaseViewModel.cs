using Egor92.MvvmNavigation.Abstractions;
using MemeFolder.Abstractions;
using System.Windows.Input;
//using MemeFolder.Services;

namespace MemeFolder.Pc.Mvvm.ViewModels.Abstractions
{
    public abstract class BaseViewModel : OnPropertyChangedClass
    {
        public INavigationManager _navigationManager;

        #region Навигация
        public ICommand NavigationToCommand { get; }
        public ICommand NavigationBackCommand { get; }
        public ICommand NavigationForwardCommand { get; }
        public ICommand NavigationToFolderCommand { get; set; }

        protected virtual void NavigationToExecute(object parameter)
            => _navigationManager.Navigate(parameter.ToString(), NavigateType.Default);

        protected virtual void NavigationBackExecute(object parameter)
            => _navigationManager.GoBack();

        protected virtual void NavigationForwardExecute(object parameter)
            => _navigationManager.GoForward();

        #endregion

        #region Конструкторы 

        public BaseViewModel(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;

            NavigationToCommand = new RelayCommand(NavigationToExecute, (o) => _navigationManager.CanNavigate(o.ToString()));
            NavigationBackCommand = new RelayCommand(NavigationBackExecute, (o) => _navigationManager.CanGoBack());
            NavigationForwardCommand = new RelayCommand(NavigationForwardExecute, (o) => _navigationManager.CanGoForward());
        }

        #endregion
    }
}
