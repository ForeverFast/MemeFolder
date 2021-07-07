using Egor92.MvvmNavigation.Abstractions;
using MemeFolder.Abstractions;
using MemeFolder.Data;
using MemeFolder.Domain.Models;
using MemeFolder.Domain.Models.AbstractModels;
using MemeFolder.Mvvm.Commands;
using MemeFolder.Mvvm.Commands.Folders;
using MemeFolder.Mvvm.Commands.Memes;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Navigation;
using MemeFolder.Pages;
using MemeFolder.Services;
using MemeFolder.ViewModels.Abstractions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MemeFolder.ViewModels
{
    public partial class FolderVM : BasePageViewModel, INavigatedToAware, IMemeWorker, IObjectWorker
    {
        #region Поля
        private Folder _model;
        private readonly DataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly IMemeDataService _memeDataService;
        private readonly IFolderDataService _folderDataService;
        #endregion

        public Folder Model { get => _model; set => SetProperty(ref _model, value); }
        public ObservableCollection<FolderObject> FolderObjects { get; set; }


        #region Имплементация - IObjectWorker

        public Folder GetModel() => Model;

        public object GetWorkerCollection(ObjectType collectionType) => FolderObjects;

        #endregion


        #region Команды - Общее

        public ICommand OpenAddDialogCommand { get; }
        public ICommand OpenEditDialogCommand { get; }

        #endregion


        

        #region Команды - Мемы

        public ICommand OpenMemePictureCommand { get; }
        public ICommand CopyMemeInBufferCommand { get; }
        public ICommand AddMemeCommand { get; }
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

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!IsBusy)
            {
                if (sender is Meme)
                {
                    var memeObj = (Meme)sender;
                    _memeDataService.Update(memeObj.Id, memeObj);
                }
                else if (sender is Folder)
                {
                    var folderObj = (Folder)sender;
                    _folderDataService.Update(folderObj.Id, folderObj);
                }
            }
        }

        #endregion


        #region Тест

        public ICommand TestCommand
        {
            get => new RelayCommand((o) => {

                FolderObjects.Add(new Meme() { Title = "chlen" });

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
            _dialogService = dataService._dialogService;
            _memeDataService = dataService._memeDataService;
            _folderDataService = dataService._folderDataService;

           

            OpenMemePictureCommand = new OpenMemePictureCommand();
            CopyMemeInBufferCommand = new CopyMemeInBufferCommand();
            AddMemeCommand = new AddMemeCommand(this, _dataService);
            RemoveMemeCommand = new RemoveMemeCommand(this, _memeDataService);

            OpenAddDialogCommand = new OpenAddDialogCommand(this, _dataService);
            OpenEditDialogCommand = new OpenEditDialogCommand(this, _dataService);

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
