using Egor92.MvvmNavigation.Abstractions;
using MemeFolder.Abstractions;
using MemeFolder.Domain.Models;
using MemeFolder.Domain.Models.AbstractModels;
using MemeFolder.Extentions;
using MemeFolder.Mvvm.Commands;
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
    public class SearchPageVM : BasePageViewModel, IDisposable, INavigatingFromAware
    {
        private IEnumerable<FolderObject> Model;
        private readonly DataService _dataService;
        private readonly DataStorage _searchService;

        public ObservableCollection<Meme> Memes { get; private set; }


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
            IEnumerable<Meme> model = (IEnumerable<Meme>)e.Argument;

            List<Meme> memes = new List<Meme>();
            foreach (Meme meme in model)
            {
                if (meme.Image == null)
                    if (meme.ImageData != null)
                    {
                        meme.Image = MemeExtentions.ConvertByteArrayToImage(meme.ImageData);
                        meme.ImageData = null;
                        if (meme.Image != null)
                            meme.Image.Freeze();
                    }

                memes.Add(meme);
            }

            e.Result = memes;
        }

        private void BgW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            foreach (var item in (List<Meme>)e.Result)
                Memes.Add(item);
            OnPropertyChanged(nameof(Memes));

            GC.Collect();
            GC.WaitForPendingFinalizers();
           

            IsLoaded = true;
            IsBusy = false;
        }

        #endregion


        public void Dispose()
        {
            Memes.Clear();
            OnAllPropertyChanged();
        }

        private void _searchService_NewRequest(IEnumerable<Meme> memes)
        {
            if (memes != null && !IsBusy)
            {
                Model = memes;
                App.Current.Dispatcher.BeginInvoke(() => {
                    Memes.Clear();
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
            _searchService = dataService._dataStorage;
            _searchService.NewRequest += _searchService_NewRequest;

            Memes = new ObservableCollection<Meme>();

            OpenMemePictureCommand = new OpenMemePictureCommand();
            CopyMemeInBufferCommand = new CopyMemeInBufferCommand();

            NavigationToFolderCommand = new RelayCommand(NavigationToFolderExecute);

            PageLoadedCommand = new RelayCommand(PageLoadedExecute);
        }

       
    }
}
