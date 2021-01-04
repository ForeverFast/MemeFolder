using Egor92.MvvmNavigation.Abstractions;
using MemeFolder.Domain.Models;
using MemeFolder.EntityFramework.Services;
using MemeFolder.MVVM.Views.Pages;
using MemeFolder.Pc.Mvvm;
using MemeFolder.Pc.Mvvm.ViewModels.Abstractions;
using MemeFolder.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MemeFolder.MVVM.ViewModels
{
    public class MainWindowVM : BaseWindowViewModel
    {
        public FolderVM Model { get; set; }
        private IServiceProvider _services;
        private IDialogService _dialogService;

        #region Команды - Навигациия

 

        //private void NavigationToFolderExecute(object parameter)
        //    => _navigationManager.Navigate<FolderPage>((parameter as Folder).Id,
        //                                                new FolderVM((parameter as Folder),
        //                                                             _navigationManager,
        //                                                             _services.GetRequiredService<IFolderDataService>()));

        #endregion


        

        #region Конструкторы

        public MainWindowVM(FolderVM model,
                            INavigationManager navigationManager,
                            IServiceProvider services,
                            IDialogService dialogService) : base(navigationManager)
        {
           
            Model = model;
            _services = services;

            //NavigationToFoldertCommand = new RelayCommand(NavigationToFolderExecute, null);

          
        }

        #endregion
    }
}
