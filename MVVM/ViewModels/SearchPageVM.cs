using Egor92.MvvmNavigation.Abstractions;
using MemeFolder.Domain.Models;
using MemeFolder.Domain.Models.AbstractModels;
using MemeFolder.Mvvm.Commands;
using MemeFolder.MVVM.Models;
using MemeFolder.MVVM.Views.Pages;
using MemeFolder.Navigation;
using MemeFolder.Pc.Mvvm;
using MemeFolder.Pc.Mvvm.ViewModels.Abstractions;
using MemeFolder.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MemeFolder.MVVM.ViewModels
{
    public class SearchPageVM : BasePageViewModel
    {

        #region Поля
        private IFolderDataService _folderDataService;
        private IMemeDataService _memeDataService;
        #endregion

        public ObservableCollection<FolderVM> NavigationData { get; }
        public ObservableCollection<FolderObject> SearchResult { get; }


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

            NavigationToFolderCommand = new RelayCommand(NavigationToFolderExecute, null);   
        }

        public SearchPageVM(SearchData searchData,
                            DataService dataService) : this(dataService._navigationService)
        {
            //NavigationData = searchData.NavigationData;
            //SearchResult = searchData.SearchResult;

            //OnAllPropertyChanged();
            NavigationData = new ObservableCollection<FolderVM>();
            SearchResult = new ObservableCollection<FolderObject>();
            foreach (var item in searchData.NavigationData)
                NavigationData.Add(item);
            foreach (var item in searchData.SearchResult)
                SearchResult.Add(item);

            _folderDataService = dataService._folderDataService;
            _memeDataService = dataService._memeDataService;
        }
    }
}
