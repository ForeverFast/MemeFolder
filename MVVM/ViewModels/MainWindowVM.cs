using Egor92.MvvmNavigation.Abstractions;
using GongSolutions.Wpf.DragDrop;
using MemeFolder.Domain.Models;
using MemeFolder.Domain.Models.AbstractModels;
using MemeFolder.EntityFramework.Services;
using MemeFolder.Mvvm.Commands;
using MemeFolder.MVVM.Models;
using MemeFolder.MVVM.Views.Pages;
using MemeFolder.Navigation;
using MemeFolder.Pc.Mvvm;
using MemeFolder.Pc.Mvvm.ViewModels.Abstractions;
using MemeFolder.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MemeFolder.MVVM.ViewModels
{
    public class MainWindowVM : BaseWindowViewModel
    {
        #region Поля
        private FolderVM _model;
        private DataService _dataService;
        #endregion


        public FolderVM TreeRoot { get => _model; set => SetProperty(ref _model, value); }
        public ObservableCollection<FolderVM> Folders { get; set; }


        #region Команды - Навигация

        public ICommand SearchCommand { get; }

        public ICommand OpenSettingsCommand { get; }

        private async Task SearchExecuteAsync(object parameter)
        {
            string SearchText = parameter.ToString();

            if (string.IsNullOrEmpty(SearchText))
            {
                _navigationService.Navigate("root", NavigationType.Root);
                return;
            }
          
            var searchResult = new SearchData();

            await TreeRoot.GetData(searchResult);
            searchResult.SearchResult = new ObservableCollection<FolderObject>(searchResult.SearchResult.Where(p => p.Title.Contains(SearchText)));
            searchResult.NavigationData = new ObservableCollection<FolderVM>(searchResult.NavigationData.Where(p => p.Model.Title.Contains(SearchText)));

            _navigationService.Navigate<SearchPage>("searchPage " + Guid.NewGuid().ToString(),
                                                    new SearchPageVM(searchResult,
                                                                     _dataService),
                                                    null);
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
            TreeRoot = model;
            Folders = model.Children;
            _dataService = dataService;

            SearchCommand = new AsyncRelayCommand(SearchExecuteAsync);
            OpenSettingsCommand = new RelayCommand(OpenSettingsExecute);
        }

        #endregion
    }
}
