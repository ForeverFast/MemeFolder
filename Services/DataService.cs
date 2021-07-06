﻿using MemeFolder.Navigation;

namespace MemeFolder.Services
{
    public class DataService
    {
        public readonly INavigationService _navigationService;
        public readonly IFolderDataService _folderDataService;
        public readonly IMemeDataService _memeDataService;
        public readonly IDialogService _dialogService;
        public readonly ISearchService _searchService;

        public DataService(IFolderDataService folderDataService,
            IMemeDataService memeDataService,
            IDialogService dialogService,
            INavigationService navigationService,
            ISearchService searchService)
        {
            _folderDataService = folderDataService;
            _memeDataService = memeDataService;
            _dialogService = dialogService;
            _navigationService = navigationService;
            _searchService = searchService;
        }
    }
}
