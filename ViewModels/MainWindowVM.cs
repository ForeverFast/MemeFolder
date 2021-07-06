using MemeFolder.Data;
using MemeFolder.Domain.Models.AbstractModels;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Navigation;
using MemeFolder.Pages;
using MemeFolder.Services;
using MemeFolder.ViewModels.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MemeFolder.ViewModels
{
    public class MainWindowVM : BaseWindowViewModel
    {
        #region Поля
        private FolderVM _model;
        private DataService _dataService;
        #endregion

        public FolderVM RootVM { get => _model; set => SetProperty(ref _model, value); }
        public ObservableCollection<FolderVM> Folders { get; set; }


        #region Команды - Навигация

        public ICommand SearchCommand { get; }

        public ICommand OpenSettingsCommand { get; }

        private async Task SearchExecuteAsync(object parameter)
        {
            if (!IsBusy)
            {
                IsBusy = true;
                string SearchText = parameter.ToString();

                if (string.IsNullOrEmpty(SearchText))
                {
                    IsBusy = false;
                    _navigationService.Navigate("root", NavigationType.Root, null);
                    return;
                }

                var searchResult = new SearchData();

               
                await RootVM.GetData(searchResult);

                searchResult.SearchResult = new ObservableCollection<FolderObject>(searchResult.SearchResult.Where(p => p.Title.Contains(SearchText)));
                searchResult.NavigationData = new ObservableCollection<FolderVM>(searchResult.NavigationData.Where(p => p.Model.Title.Contains(SearchText)));

                string navKey = "searchPage " + Guid.NewGuid().ToString();
                SearchPageVM searchPageVM = new SearchPageVM(searchResult, _dataService);
                _navigationService.Navigate<SearchPage>(navKey, searchPageVM, null);

                IsBusy = false;
            }
           
        }

        private void OpenSettingsExecute(object parameter)
        {
            _navigationService.Navigate("settings", NavigationType.Default);
        }

        #endregion


        #region Конструкторы

        public MainWindowVM(FolderVM model,
                            DataService dataService) : base(dataService._navigationService)
        {
            RootVM = model;
            Folders = model.Children;
            _dataService = dataService;

            SearchCommand = new AsyncRelayCommand(SearchExecuteAsync);
            OpenSettingsCommand = new RelayCommand(OpenSettingsExecute);
        }

        #endregion
    }
}
