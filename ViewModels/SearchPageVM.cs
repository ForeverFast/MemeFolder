using Egor92.MvvmNavigation.Abstractions;
using MemeFolder.Abstractions;
using MemeFolder.Domain.Models;
using MemeFolder.Domain.Models.AbstractModels;
using MemeFolder.Extentions;
using MemeFolder.Mvvm.Commands;
using MemeFolder.Mvvm.Commands.Memes;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Navigation;
using MemeFolder.Pages;
using MemeFolder.Services;
using MemeFolder.ViewModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Threading;

namespace MemeFolder.ViewModels
{
    public class SearchPageVM : BasePageViewModel, IFolderObjectWorker, IDisposable, INavigatingFromAware
    {
        private IEnumerable<FolderObject> Model;
        private readonly DataService _dataService;
        private readonly ISearchService _searchService;

        public ObservableCollection<FolderObject> FolderObjects { get; private set; }


        #region Имплементация - IFolderObjectWorker

        public Folder GetModel() => null;

        public ObservableCollection<FolderObject> GetWorkerCollection() => FolderObjects;

        #endregion


        #region Команды - Общее

        public ICommand OpenAddDialogCommand { get; }
        public ICommand OpenEditDialogCommand { get; }

        #endregion


        #region Команды - Мемы

        public ICommand OpenMemePictureCommand { get; }
        public ICommand CopyMemeInBufferCommand { get; }

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


        #region Логика - Загрузка

        public ICommand PageLoadedCommand { get; }

        private void PageLoadedExecute(object parameter)
        {
            if (!IsBusy)
            {
                IsBusy = true;
                BackgroundWorker bgW = new BackgroundWorker();
                bgW.DoWork += BgW_DoWork;
                bgW.RunWorkerCompleted += BgW_RunWorkerCompleted;
                bgW.RunWorkerCompleted += (o, e) => bgW.Dispose();
                bgW.RunWorkerAsync(Model);
            }
        }

        private void BgW_DoWork(object sender, DoWorkEventArgs e)
        {
            IEnumerable<FolderObject> model = (IEnumerable<FolderObject>)e.Argument;

            List<FolderObject> folderObjects = new List<FolderObject>();
            foreach(FolderObject item in model)
            {
                if (item is Meme meme)
                {
                    if (meme.ImageData != null)
                    {
                        meme.Image = MemeExtentions.ConvertByteArrayToImage(meme.ImageData);
                        meme.ImageData = null;
                        if (meme.Image != null)
                            meme.Image.Freeze();
                    }
                }
                folderObjects.Add(item);
            }

            e.Result = folderObjects;
        }

        private void BgW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OnPropertyChanged(nameof(FolderObjects));
            foreach (var item in (List<FolderObject>)e.Result)
                FolderObjects.Add(item);

            
            GC.Collect();
            GC.WaitForPendingFinalizers();
           

            IsLoaded = true;
            IsBusy = false;
        }

        #endregion


        public void Dispose()
        {
            FolderObjects.Clear();
            OnAllPropertyChanged();
        }

        private void _searchService_NewRequest(IEnumerable<FolderObject> folderObjects)
        {
            if (folderObjects != null && !IsBusy)
            {
                Model = folderObjects;
                App.Current.Dispatcher.BeginInvoke(() => {
                    FolderObjects.Clear();
                    PageLoadedCommand.Execute(null);
                });
                
            }
        }

        public void OnNavigatingFrom()
        {
            this.Dispose();
        }

        public SearchPageVM(DataService dataService) : base(dataService._navigationService)
        {
            _dataService = dataService;
            _searchService = dataService._searchService;
            _searchService.NewRequest += _searchService_NewRequest;

            FolderObjects = new ObservableCollection<FolderObject>();

            OpenMemePictureCommand = new OpenMemePictureCommand();
            CopyMemeInBufferCommand = new CopyMemeInBufferCommand();


            OpenAddDialogCommand = new OpenAddDialogCommand(this, _dataService);
            OpenEditDialogCommand = new OpenEditDialogCommand(this, _dataService);

            NavigationToFolderCommand = new RelayCommand(NavigationToFolderExecute);

            PageLoadedCommand = new RelayCommand(PageLoadedExecute);
        }

       
    }
}
