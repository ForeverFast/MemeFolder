using MemeFolder.Domain.Models;
using MemeFolder.Extentions;
using MemeFolder.Mvvm.Commands;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Navigation;
using MemeFolder.Pages;
using MemeFolder.Services;
using MemeFolder.ViewModels.Abstractions;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MemeFolder.ViewModels
{
    public class MainWindowVM : BaseWindowViewModel
    {
        #region Поля
        private Folder _model;
        private DataStorage dataStorage;
        private DataService _dataService;

        private ObservableCollection<Folder> _folders;
        private ObservableCollection<MemeTag> _memeTags;
        #endregion

        public Folder Model { get => _model; set => SetProperty(ref _model, value); }

        public ObservableCollection<Folder> Folders { get => _folders; set => SetProperty(ref _folders, value); }
        public ObservableCollection<MemeTag> MemeTags { get => _memeTags; set => SetProperty(ref _memeTags, value); }

        #region Команды - Папки

        public ICommand AddFolderCommand { get; }
        public ICommand OpenAddFolderDialogCommand { get; }
        public ICommand OpenEditFolderDialogCommand { get; }
        public ICommand RemoveFolderCommand { get; }

        #endregion


        #region Команды - Теги

        public ICommand OpenAddMemeTagDialogCommand { get; }
        public ICommand OpenEditMemeTagDialogCommand { get; }
        public ICommand RemoveMemeTagCommand { get; }

        #endregion


        #region Команды - Навигация

        public ICommand SearchCommand { get; }

        public ICommand EmptySearchTextCheckCommand { get; }

        public ICommand OpenSettingsCommand { get; }

        public ICommand NavigateByMemeTagCommand { get; }

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
                _dataService._dataStorage.NavigateByRequest(fo => fo.Title.Contains(SearchText));

                IsBusy = false;
            }
           
        }

        private void EmptySearchTextCheckExecute(object parameter)
        {
            if (!IsBusy)
            {
                CheckInputNavKey(parameter);
                IsBusy = false;
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

        private void NavigateByMemeTagExecute(object parameter)
        {
            if (!IsBusy)
            {
                IsBusy = true;

                MemeTag memeTag = (MemeTag)parameter;

                string navKey = "searchPage";
                _navigationService.Navigate(navKey, NavigationType.Default, null);
                _dataService._dataStorage.NavigateByMemeTag(memeTag.Id);

                IsBusy = false;
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
            

            return false;
        }

        #region События

        private void DataStorage_OnAddFolder(Folder folder)
        {
            //Folder sf = MemeExtentions.SelectRecursive(Folders, f => f.Folders).FirstOrDefault(f => f.Id == folder.Id);
            //sf.ParentFolder.OnAllPropertyChanged();
        }

        #endregion


        #region Конструкторы

        public MainWindowVM(DataService dataService) : base(dataService._navigationService)
        {
            _dataService = dataService;
            dataStorage = dataService._dataStorage;
            dataStorage.OnAddFolder += DataStorage_OnAddFolder;

            Model = dataService._dataStorage.RootFolder;
            MemeTags = dataService._dataStorage.MemeTags;
            Folders = new ObservableCollection<Folder>();
            Folders.Add(Model);

            AddFolderCommand = new AddFolderCommand(dataService);
            OpenAddFolderDialogCommand = new OpenAddFolderDialogCommand(dataService);
            OpenEditFolderDialogCommand = new OpenEditFolderDialogCommand(dataService);
            RemoveFolderCommand = new RemoveFolderCommand(dataService);

            OpenAddMemeTagDialogCommand = new OpenAddMemeTagDialogCommand(dataService);
            OpenEditMemeTagDialogCommand = new OpenEditMemeTagDialogCommand(dataService);
            RemoveMemeTagCommand = new RemoveMemeTagCommand(dataService);

            SearchCommand = new RelayCommand(SearchExecuteAsync);
            EmptySearchTextCheckCommand = new RelayCommand(EmptySearchTextCheckExecute);
            OpenSettingsCommand = new RelayCommand(OpenSettingsExecute);

            NavigationToFolderCommand = new RelayCommand(NavigationToFolderExecute);
            NavigateByMemeTagCommand = new RelayCommand(NavigateByMemeTagExecute);
        }

       

        #endregion
    }
}
