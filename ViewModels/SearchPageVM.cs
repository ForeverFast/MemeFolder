using MemeFolder.Data;
using MemeFolder.Domain.Models;
using MemeFolder.Domain.Models.AbstractModels;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Navigation;
using MemeFolder.Pages;
using MemeFolder.Services;
using MemeFolder.ViewModels.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MemeFolder.ViewModels
{
    public class SearchPageVM : BasePageViewModel
    {
        private readonly SearchData Model;

        #region Поля
        private IFolderDataService _folderDataService;
        private IMemeDataService _memeDataService;

        private DataService _dataService;
        #endregion

        public ObservableCollection<FolderVM> NavigationData { get; private set; }
        public ObservableCollection<FolderObject> SearchResult { get; private set; }


        #region Команды - Мемы

        public ICommand OpenMemePictureCommand { get; }
        public ICommand CopyMemeInBufferCommand { get; }
        public ICommand RemoveMemeCommand { get; }

        private void CopyMemeInBufferExecute(object parameter)
        {
            Clipboard.SetImage(new BitmapImage(new Uri((parameter as Meme).ImagePath)));
        }

        private void OpenMemePictureExecute(object parameter)
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo((parameter as Meme).ImagePath)
            {
                UseShellExecute = true
            };
            p.Start();
        }

        private async Task RemoveMemeExecuteAsync(object parameter)
        {
            var meme = parameter as Meme;

            if(await _memeDataService.Delete(meme.Id))
            {
                SearchResult.Remove(meme);
            }
        }


        #endregion

        #region Команды - Папки

        public ICommand RemoveFolderCommand { get; }

        public async Task RemoveFolderExecuteAsync(object parameter)
        {
            if (await _folderDataService.Delete((parameter as Folder).Id))
            {
                SearchResult.Remove((parameter as Folder));
            }

        }

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

        #region Команды - Навигациия

        private void NavigationToFolderExecute(object parameter)
            => _navigationService.Navigate<FolderPage>((parameter as Folder).Id.ToString(),
                                                        NavigationData.FirstOrDefault(x => x.Model == (parameter as Folder)), null);

        #endregion

        


        private SearchPageVM(INavigationService navigationService) : base(navigationService)
        {
            OpenEditTitleCommand = new RelayCommand(OpenEditTitleExecute);
            CloseEditTitleCommand = new RelayCommand(CloseEditTitleExecute);

            RemoveFolderCommand = new AsyncRelayCommand(RemoveFolderExecuteAsync);

            OpenMemePictureCommand = new RelayCommand(OpenMemePictureExecute);
            CopyMemeInBufferCommand = new RelayCommand(CopyMemeInBufferExecute);
            RemoveMemeCommand = new AsyncRelayCommand(RemoveMemeExecuteAsync);

            NavigationToFolderCommand = new RelayCommand(NavigationToFolderExecute);
            PageLoadedCommand = new AsyncRelayCommand(PageLoadedExecuteAsync);
        }

        public SearchPageVM(SearchData searchData,
                            DataService dataService) : this(dataService._navigationService)
        {
            _dataService = dataService;
            _folderDataService = dataService._folderDataService;
            _memeDataService = dataService._memeDataService;

            Model = searchData;
        }

        public ICommand PageLoadedCommand { get; }

        private async Task PageLoadedExecuteAsync(object parameter)
        {
            NavigationData = new ObservableCollection<FolderVM>();
            SearchResult = new ObservableCollection<FolderObject>();

            var uiContext = SynchronizationContext.Current;
            await Task.Run(() => {
                
                foreach (var item in Model.SearchResult)
                {
                    if(item is Folder folder)
                    {
                        uiContext.Send(x => {
                            NavigationData.Add(new FolderVM(folder,
                                          _dataService));
                            folder.PropertyChanged += Model_PropertyChanged;
                            SearchResult.Add(item);
                        }, null);
                       
                    } else if (item is Meme meme)
                    {
                        uiContext.Send(x => {
                            meme.PropertyChanged += Model_PropertyChanged;
                            SearchResult.Add(item);
                        }, null);
                       
                    }

                   
                }
            });
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
}
