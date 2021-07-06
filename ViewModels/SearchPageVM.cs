using MemeFolder.Abstractions;
using MemeFolder.Data;
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

namespace MemeFolder.ViewModels
{
    public class SearchPageVM : BasePageViewModel, IFolderObjectWorker
    {
        private readonly SearchData Model;
       
        #region Поля
        private IFolderDataService _folderDataService;
        private IMemeDataService _memeDataService;

        private DataService _dataService;
        #endregion


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
            if (FolderObjects == null)
            {
                IsBusy = true;

                FolderObjects = new ObservableCollection<FolderObject>();

                BackgroundWorker bgW = new BackgroundWorker();
                bgW.DoWork += BgW_DoWork;
                bgW.RunWorkerCompleted += BgW_RunWorkerCompleted;
                bgW.RunWorkerCompleted += (o, e) => bgW.Dispose();
                bgW.RunWorkerAsync(Model);
            }
        }

        private void BgW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OnPropertyChanged(nameof(FolderObjects));
            foreach (var item in (List<FolderObject>)e.Result)
                FolderObjects.Add(item);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            IsLoaded = true;
            IsBusy = false;
        }

        private void BgW_DoWork(object sender, DoWorkEventArgs e)
        {
            var model = (Folder)e.Argument;

            List<FolderObject> folderObjects = new List<FolderObject>();

            foreach (var item in model.Folders)
                folderObjects.Add(item);

            foreach (var item in model.Memes)
            {
                item.Image = MemeExtentions.ConvertByteArrayToImage(item.ImageData);
                item.ImageData = null;
                if (item.Image != null)
                    item.Image.Freeze();
                folderObjects.Add(item);
            }

            e.Result = folderObjects;
        }

        #endregion


        private SearchPageVM(INavigationService navigationService) : base(navigationService)
        {

            OpenMemePictureCommand = new OpenMemePictureCommand();
            CopyMemeInBufferCommand = new CopyMemeInBufferCommand();

           
            OpenAddDialogCommand = new OpenAddDialogCommand(this, _dataService);
            OpenEditDialogCommand = new OpenEditDialogCommand(this, _dataService);

            NavigationToFolderCommand = new RelayCommand(NavigationToFolderExecute);

            PageLoadedCommand = new RelayCommand(PageLoadedExecute);
        }

        public SearchPageVM(SearchData searchData,
                            DataService dataService) : this(dataService._navigationService)
        {
            _dataService = dataService;
            _folderDataService = dataService._folderDataService;
            _memeDataService = dataService._memeDataService;

            Model = searchData;
        }

        
    }
}
