using MemeFolder.Abstractions;
using MemeFolder.Data;
using MemeFolder.Domain.Models;
using MemeFolder.Domain.Models.AbstractModels;
using MemeFolder.Mvvm.Commands;
using MemeFolder.Mvvm.Commands.Folders;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Navigation;
using MemeFolder.Pages;
using MemeFolder.Services;
using MemeFolder.ViewModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace MemeFolder.ViewModels
{
    public class MainWindowVM : BaseWindowViewModel, IObjectWorker
    {
        #region Поля
        private Folder _model;
        private DataService _dataService;
        #endregion

        public Folder Model { get => _model; set => SetProperty(ref _model, value); }

        public ObservableCollection<Folder> Folders { get; set; }
        public ObservableCollection<MemeTag> MemeTags { get; set; }


        public Folder GetModel() => Model;

        public object GetWorkerCollection(ObjectType objectType)
        {
            switch (objectType)
            {
                case ObjectType.Folder:
                    return Folders;
                case ObjectType.MemeTag:
                    return MemeTags;
                case ObjectType.Meme:
                    return null;
                default:
                    return null;
            }
            
        }
        

        #region Команды - Общее

        public ICommand OpenAddDialogCommand { get; }
        public ICommand OpenEditDialogCommand { get; }

        #endregion

        #region Команды - Папки

        public ICommand AddFolderCommand { get; }

        public ICommand RemoveFolderCommand { get; }

        #endregion


        #region Команды - Навигация

        public ICommand SearchCommand { get; }

        public ICommand EmptySearchTextCheckCommand { get; }

        public ICommand OpenSettingsCommand { get; }

        private void SearchExecuteAsync(object parameter)
        {
            if (!IsBusy)
            {
                IsBusy = true;
              
                string SearchText = parameter.ToString();

                if (CheckInputNavKey(parameter))
                    return;

                string navKey = "searchPage";
                _navigationService.Navigate(navKey, NavigationType.Default, null);
                _dataService._searchService.GetWhere(fo => fo.Title.Contains(SearchText));

                IsBusy = false;
            }
           
        }

        private void EmptySearchTextCheckExecute(object parameter)
        {
            if (!IsBusy)
            {
                CheckInputNavKey(parameter);
            }

        }

        private void OpenSettingsExecute(object parameter)
        {
            _navigationService.Navigate("settings", NavigationType.Default);
        }

        protected void NavigationToFolderExecute(object parameter)
        {
            Folder folder = (Folder)parameter;

            string navKey = string.Empty;
            if (folder.Title == "root")
                navKey = "root";
            else
                navKey = folder.Id.ToString();

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


        private bool CheckInputNavKey(object key)
        {
            IsBusy = true;

            string SearchText = key.ToString();

            if (string.IsNullOrEmpty(SearchText))
            {
                GC.WaitForPendingFinalizers();
                GC.Collect();
                _navigationService.Navigate("root", NavigationType.Root, null);
                IsBusy = false;
                return true;
            }
            IsBusy = false;

            return false;
        }

        
        #region Конструкторы

        public MainWindowVM(FolderVM model,
                            DataService dataService) : base(dataService._navigationService)
        {

            Model = model.Model;
            
            _dataService = dataService;

            OpenAddDialogCommand = new OpenAddDialogCommand(this, _dataService);
            OpenEditDialogCommand = new OpenEditDialogCommand(this, _dataService);

            AddFolderCommand = new AddFolderCommand(this, _dataService._folderDataService);
            RemoveFolderCommand = new RemoveFolderCommand(this, _dataService._folderDataService);

            SearchCommand = new RelayCommand(SearchExecuteAsync);
            EmptySearchTextCheckCommand = new RelayCommand(EmptySearchTextCheckExecute);
            OpenSettingsCommand = new RelayCommand(OpenSettingsExecute);

            NavigationToFolderCommand = new RelayCommand(NavigationToFolderExecute);

            MemeTags = new ObservableCollection<MemeTag>();
            Folders = new ObservableCollection<Folder>();
            Folders.Add(Model);

        }

        #endregion
    }
}
