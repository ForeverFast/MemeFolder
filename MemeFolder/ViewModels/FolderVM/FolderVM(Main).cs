﻿using Egor92.MvvmNavigation.Abstractions;
using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.Commands;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Navigation;
using MemeFolder.Pages;
using MemeFolder.Services;
using MemeFolder.ViewModels.Abstractions;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MemeFolder.ViewModels
{
    public partial class FolderVM : BasePageViewModel, INavigatedToAware
    {
        #region Поля
        private Folder _model;
        private readonly ServiceCollectionClass _dataService;
        private readonly DataStorage _dataStorage;
        #endregion

        public Folder Model { get => _model; set => SetProperty(ref _model, value); }
        public ObservableCollection<Meme> Memes { get; set; }


        #region Команды - Мемы

        public ICommand AddMemeCommand { get; }
        public ICommand OpenAddMemeDialogCommand { get; }
        public ICommand OpenEditMemeDialogCommand { get; }
        public ICommand OpenMemePictureCommand { get; }
        public ICommand CopyMemeInBufferCommand { get; }
        public ICommand RemoveMemeCommand { get; }

        #endregion


        #region Команды - Навигациия

        private void NavigationToFolderExecute(object parameter)
        {
            Folder folder = (Folder)parameter;
            string navKey = folder.Id.ToString();

            if (_navigationService.CanNavigate(navKey))
            {
                _navigationService.Navigate(navKey, NavigationType.Default);
            }
            else
            {
                FolderVM folderVM = new FolderVM(folder, _dataService);
                _navigationService.Navigate<FolderPage>(navKey, folderVM, null);
            }
        }


        #endregion


        #region События

        #endregion


        #region Тест

        public ICommand TestCommand
        {
            get => new RelayCommand((o) => {

                

            });
        }

        public ICommand GoBCommand
        {
            get => new RelayCommand((o) => {

            });
        }

        public ICommand GoFCommand
        {
            get => new RelayCommand((o) => {

            });
        }



        #endregion


        #region Конструкторы

        private FolderVM(ServiceCollectionClass dataService) : base(dataService._navigationService)
        {
            _dataService = dataService;
            _dataStorage = dataService._dataStorage;
           
            AddMemeCommand = new AddMemeCommand(dataService);
            OpenMemePictureCommand = new OpenMemePictureCommand();
            OpenAddMemeDialogCommand = new OpenAddMemeDialogCommand(dataService);
            OpenEditMemeDialogCommand = new OpenEditMemeDialogCommand(dataService);
            CopyMemeInBufferCommand = new CopyMemeInBufferCommand();
            RemoveMemeCommand = new RemoveMemeCommand(dataService);

            NavigationToFolderCommand = new RelayCommand(NavigationToFolderExecute);

            PageLoadedCommand = new RelayCommand(PageLoadedExecuteAsync);
        }

     

        public FolderVM(Folder model, ServiceCollectionClass dataService) : this(dataService)
        {
            Model = model;
        }

        public void OnNavigatedTo(object arg)
        {

        }

        #endregion
    }
}
