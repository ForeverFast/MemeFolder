using Egor92.MvvmNavigation.Abstractions;
using MemeFolder.Abstractions;
using MemeFolder.Domain.Models;
using MemeFolder.Domain.Models.AbstractModels;
using MemeFolder.Mvvm.Commands;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Navigation;
using MemeFolder.Pages;
using MemeFolder.Services;
using MemeFolder.ViewModels.Abstractions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace MemeFolder.ViewModels
{
    public partial class FolderVM : BasePageViewModel, INavigatedToAware, IMemeWorker
    {
        #region Поля
        private Folder _model;
        private readonly DataService _dataService;
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

        private void DataStorage_OnAddMeme(Meme meme)
        {
            
        }

        private void _dataStorage_OnRemoveMeme(Meme meme)
        {
            
        }

        #endregion


        #region Тест

        public ICommand TestCommand
        {
            get => new RelayCommand((o) => {

                Memes.Add(new Meme() { Title = "chlen" });

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

        private FolderVM(DataService dataService) : base(dataService._navigationService)
        {
            _dataService = dataService;
            _dataStorage = dataService._dataStorage;
            _dataStorage.OnAddMeme += DataStorage_OnAddMeme;
            _dataStorage.OnRemoveMeme += _dataStorage_OnRemoveMeme;

            AddMemeCommand = new AddMemeCommand(dataService);
            OpenMemePictureCommand = new OpenMemePictureCommand();
            OpenAddMemeDialogCommand = new OpenAddMemeDialogCommand(dataService);
            OpenEditMemeDialogCommand = new OpenEditMemeDialogCommand(dataService);
            CopyMemeInBufferCommand = new CopyMemeInBufferCommand();
            RemoveMemeCommand = new RemoveMemeCommand(dataService);

            NavigationToFolderCommand = new RelayCommand(NavigationToFolderExecute);

            PageLoadedCommand = new RelayCommand(PageLoadedExecuteAsync);
        }

     

        public FolderVM(Folder model, DataService dataService) : this(dataService)
        {
            Model = model;
        }

        public void OnNavigatedTo(object arg)
        {

        }

        #endregion
    }
}
