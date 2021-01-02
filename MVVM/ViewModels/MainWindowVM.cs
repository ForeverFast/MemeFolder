using Egor92.MvvmNavigation.Abstractions;
using MemeFolder.Pc.Mvvm;
using MemeFolder.Pc.Mvvm.ViewModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MemeFolder.MVVM.ViewModels
{
    public class MainWindowVM : BaseWindowViewModel
    {


        #region Команды - Навигациия

        public ICommand NavigationToCommand { get; private set; }
        public ICommand NavigationBackCommand { get; private set; }
        public ICommand NavigationForwardCommand { get; private set; }
        public ICommand NavigationToPlayListCommand { get; private set; }

        private void NavigationToExecute(object parameter)
            => _navigationManager.Navigate(parameter.ToString(), NavigateType.Default);

        private void NavigationBackExecute(object parameter)
            => _navigationManager.GoBack();

        private void NavigationForwardExecute(object parameter)
            => _navigationManager.GoForward();

        //private void NavigationToPlayListExecute(object parameter)
        //    => _navigationManager.Navigate<PlaylistPage>((parameter as PlayList).Id, new PlaylistVM(parameter as PlayList),
        //                                                 _musicStorage.PlayLists,
        //                                                 _dialogService,
        //                                                 _musicPlayerService);

        #endregion


        #region Конструкторы

        public MainWindowVM(INavigationManager navigationManager) : base(navigationManager)
        {
            InitiailizeCommands();

            NavigationToCommand = new RelayCommand(NavigationToExecute, (o) => _navigationManager.CanNavigate(o.ToString()));
            NavigationBackCommand = new RelayCommand(NavigationBackExecute, (o) => _navigationManager.CanGoBack());
            NavigationForwardCommand = new RelayCommand(NavigationForwardExecute, (o) => _navigationManager.CanGoForward());
            //NavigationToPlayListCommand = new RelayCommand(NavigationToPlayListExecute, null);
        }

        #endregion
    }
}
