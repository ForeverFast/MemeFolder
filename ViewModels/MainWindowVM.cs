using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.Commands;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Navigation;
using MemeFolder.Pages;
using MemeFolder.Services;
using MemeFolder.ViewModels.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MemeFolder.ViewModels
{
    public class MainWindowVM : BaseWindowViewModel, IStatusMessagesProvider
    {
        #region Поля
        private Folder _model;
        private DataStorage _dataStorage;
        private ServiceCollectionClass _dataService;
        private string _systemMessage;

        private ObservableCollection<Folder> _folders;
        private ObservableCollection<MemeTag> _memeTags;
        #endregion

        public Folder Model { get => _model; set => SetProperty(ref _model, value); }

        public ObservableCollection<Folder> Folders { get => _folders; set => SetProperty(ref _folders, value); }
        public ObservableCollection<MemeTag> MemeTags { get => _memeTags; set => SetProperty(ref _memeTags, value); }

        public string SystemMessage { get => _systemMessage; set => SetProperty(ref _systemMessage, value); }


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

                if (CheckInputSearchText(SearchText))
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
                CheckInputSearchText(parameter.ToString());
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



        private bool CheckInputSearchText(string text)
        {
            IsBusy = true;

            if (string.IsNullOrEmpty(text))
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                _navigationService.Navigate("root", NavigationType.Root, null);

                IsBusy = false;
                return true;
            }
            
            return false;
        }

        #endregion


        #region События

        private void _dataStorage_OnFolderEvent(Folder folder, string message)
        {
            this.SystemMessage = message;
        }

        private void _dataStorage_OnMemeEvent(Meme meme, string message)
        {
            this.SystemMessage = message;
            
        }

        private void _dataStorage_OnMemeTagEvent(MemeTag memeTag, string message)
        {
            this.SystemMessage = message;
        }

        #endregion


        #region Конструкторы

        public MainWindowVM(ServiceCollectionClass services) : base(services._navigationService)
        {
            _dataService = services;
            _dataStorage = services._dataStorage;

            _dataStorage.OnAddFolder += _dataStorage_OnFolderEvent;
            _dataStorage.OnEditFolder += _dataStorage_OnFolderEvent;
            _dataStorage.OnRemoveFolder += _dataStorage_OnFolderEvent;

            _dataStorage.OnAddMeme += _dataStorage_OnMemeEvent;
            _dataStorage.OnEditMeme += _dataStorage_OnMemeEvent;
            _dataStorage.OnRemoveMeme += _dataStorage_OnMemeEvent;

            _dataStorage.OnAddMemeTag += _dataStorage_OnMemeTagEvent;
            _dataStorage.OnEditMemeTag += _dataStorage_OnMemeTagEvent;
            _dataStorage.OnRemoveMemeTag += _dataStorage_OnMemeTagEvent;

            Model = services._dataStorage.RootFolder;
            MemeTags = services._dataStorage.MemeTags;
            Folders = new ObservableCollection<Folder>();
            Folders.Add(Model);

            AddFolderCommand = new AddFolderCommand(services);
            OpenAddFolderDialogCommand = new OpenAddFolderDialogCommand(services);
            OpenEditFolderDialogCommand = new OpenEditFolderDialogCommand(services);
            RemoveFolderCommand = new RemoveFolderCommand(services);

            OpenAddMemeTagDialogCommand = new OpenAddMemeTagDialogCommand(services);
            OpenEditMemeTagDialogCommand = new OpenEditMemeTagDialogCommand(services);
            RemoveMemeTagCommand = new RemoveMemeTagCommand(services);

            SearchCommand = new RelayCommand(SearchExecuteAsync);
            EmptySearchTextCheckCommand = new RelayCommand(EmptySearchTextCheckExecute);
            OpenSettingsCommand = new RelayCommand(OpenSettingsExecute);

            NavigationToFolderCommand = new RelayCommand(NavigationToFolderExecute);
            NavigateByMemeTagCommand = new RelayCommand(NavigateByMemeTagExecute);

            SystemMessage = "Загрузка окна завершена";
        }

       

        #endregion
    }
}
