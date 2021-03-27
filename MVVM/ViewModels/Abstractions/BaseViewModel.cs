using MemeFolder.Abstractions;
using MemeFolder.Navigation;
using System.Windows.Input;

namespace MemeFolder.Pc.Mvvm.ViewModels.Abstractions
{
    public abstract class BaseViewModel : OnPropertyChangedClass
    {
        public INavigationService _navigationService;

        #region Навигация
        public ICommand NavigationToCommand { get; }
        public ICommand NavigationBackCommand { get; }
        public ICommand NavigationForwardCommand { get; }
        public ICommand NavigationToFolderCommand { get; set; }

        protected virtual void NavigationToExecute(object parameter)
            => _navigationService.Navigate(parameter.ToString(), NavigationType.Default);

        protected virtual void NavigationBackExecute(object parameter)
            => _navigationService.GoBack();

        protected virtual void NavigationForwardExecute(object parameter)
            => _navigationService.GoForward();

        #endregion

        #region Конструкторы 

        public BaseViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            NavigationToCommand = new RelayCommand(NavigationToExecute, (o) => _navigationService.CanNavigate(o.ToString()));
            NavigationBackCommand = new RelayCommand(NavigationBackExecute, (o) => _navigationService.CanGoBack());
            NavigationForwardCommand = new RelayCommand(NavigationForwardExecute, (o) => _navigationService.CanGoForward());
        }

        #endregion
    }
}
