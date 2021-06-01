using Egor92.MvvmNavigation.Abstractions;
using MemeFolder.Abstractions;
using MemeFolder.Data;
using MemeFolder.Domain.Models;
using MemeFolder.Domain.Models.AbstractModels;
using MemeFolder.Extentions.Collections;
using MemeFolder.Mvvm.Commands.Folders;
using MemeFolder.Mvvm.Commands.Memes;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Pages;
using MemeFolder.Services;
using MemeFolder.ViewModels.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MemeFolder.ViewModels
{
    public partial class FolderVM : BasePageViewModel, INavigatedToAware, IMemeWorker, IFolderVM
    {
        #region Поля
        private Folder _model;
        private readonly DataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly IMemeDataService _memeDataService;
        private readonly IFolderDataService _folderDataService;
        #endregion

        public Folder Model { get => _model; set => SetProperty(ref _model, value); }
        public ObservableCollection<FolderVM> Children { get; set; }
        public ObservableCollection<FolderObject> FolderObjects { get; set; }
        public ObservableCollection<Meme> Memes { get; set; }
        public PagingCollectionView PMemes { get; set; }

      

        #region Команды - Папки

        public ICommand AddFolderCommand { get; }

        public ICommand RemoveFolderCommand { get; }

        #endregion


        #region Команды - Мемы

        public ICommand OpenMemePictureCommand { get; }
        public ICommand CopyMemeInBufferCommand { get; }
        public ICommand AddMemeCommand { get; }
        public ICommand RemoveMemeCommand { get; }

        #endregion


        #region Команды - Навигациия

        private void NavigationToFolderExecute(object parameter)
            => _navigationService.Navigate<FolderPage>((parameter as Folder).Id.ToString(),
                                                        Children.FirstOrDefault(x => x.Model == (parameter as Folder)), null);

        #endregion


        #region Команды - Редактирование

        private bool _isOpenForEditTitle;
        public bool IsOpenForEditTitle
        {
            get => _isOpenForEditTitle;
            set => SetProperty(ref _isOpenForEditTitle, value);
        }


        public ICommand OpenEditTitleCommand { get; }
        public ICommand CloseEditTitleCommand { get; }

        private void OpenEditTitleExecute(object parameter)
            => IsOpenForEditTitle = true;
        private void CloseEditTitleExecute(object parameter)
           => IsOpenForEditTitle = false;


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

        private void FolderObjects_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //if (e.Action == NotifyCollectionChangedAction.Add && IsLoaded == true)
            //{
            //    foreach (var item in /*(List<FolderObject>)*/e.NewItems)
            //    {
            //        if (item is Folder folder)
            //        {
            //            Model.Folders.Add(folder);
            //            Children.Add(new FolderVM(folder, _dataService));
            //        }
            //        else if (item is Meme meme)
            //            Model.Memes.Add(meme);
            //        OnPropertyChanged(nameof(this.FolderObjects));
            //    }

            //}
        }


        #endregion


        #region Тест

        public async Task GetData(SearchData searchData)
        {
            //foreach (var item in FolderObjects)
            //    searchData.SearchResult.Add(item);
            foreach (var item in Children)
            {
                searchData.NavigationData.Add(item);
                await item.GetData(searchData);
            }

        }

        public ICommand TestCommand { get => new RelayCommand((o) => {

            FolderObjects.Add(new Meme() { Title = "chlen" });

        }); } 
        public ICommand GoBCommand { get => new RelayCommand((o) => {

            
            //PMemes.MoveToPreviousPage();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();


        }); }
        public ICommand GoFCommand { get => new RelayCommand((o) => {

            PMemes.MoveToNextPage();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

        }); }



        #endregion


        #region Конструкторы

        private FolderVM(DataService dataService) : base(dataService._navigationService)
        {
            _dataService = dataService;
            _dialogService = dataService._dialogService;
            _memeDataService = dataService._memeDataService;
            _folderDataService = dataService._folderDataService;
         
            OpenEditTitleCommand = new RelayCommand(OpenEditTitleExecute);
            CloseEditTitleCommand = new RelayCommand(CloseEditTitleExecute);

            AddFolderCommand = new AddFolderCommand(this, _folderDataService);
            RemoveFolderCommand = new RemoveFolderCommand(this, _folderDataService);

            OpenMemePictureCommand = new OpenMemePictureCommand();
            CopyMemeInBufferCommand = new CopyMemeInBufferCommand();
            AddMemeCommand = new AddMemeCommand(this, _dataService);
            RemoveMemeCommand = new RemoveMemeCommand(this, _memeDataService);



            NavigationToFolderCommand = new RelayCommand(NavigationToFolderExecute);
            PageLoadedCommand = new AsyncRelayCommand(PageLoadedExecuteAsync);
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


/*

                Model.PropertyChanged += Model_PropertyChanged;
                foreach (var item in Model.Folders)
                {

                    Children.Add(new FolderVM(item,
                                        _dataService));
                    item.PropertyChanged += Model_PropertyChanged;
                    FolderObjects.Add(item);


                }

                foreach (var item in Model.Memes)
                {

                    item.PropertyChanged += Model_PropertyChanged;
                    FolderObjects.Add(item);

                }

                var uiContext = SynchronizationContext.Current;
                Stopwatch myStopwatch = new Stopwatch();
                myStopwatch.Start(); //запуск
                await Task.Run(() =>
                {
                    //Model.PropertyChanged += Model_PropertyChanged;

                    foreach (var item in Model.Folders)
                    {
                        uiContext.Post(x =>
                        {
                            Children.Add(new FolderVM(item, _dataService));
                            //item.PropertyChanged += Model_PropertyChanged;
                            FolderObjects.Add(item);
                            OnPropertyChanged(nameof(Children));
                            OnPropertyChanged(nameof(FolderObjects));
                        }, null);

                    }

                    foreach (var item in Model.Memes)
                    {
                        uiContext.Post(x =>
                        {
                            //item.PropertyChanged += Model_PropertyChanged;
                            FolderObjects.Add(item);
                            OnPropertyChanged(nameof(FolderObjects));
                           
                        }, null);
                       
                    }

                  
                });

                myStopwatch.Stop();

                */