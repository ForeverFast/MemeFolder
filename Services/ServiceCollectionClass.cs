using MemeFolder.Navigation;

namespace MemeFolder.Services
{
    public class ServiceCollectionClass
    {
        public readonly INavigationService _navigationService;
        public readonly IDialogService _dialogService;

        public readonly IMemeTagDataService _memeTagDataService;
        public readonly IFolderDataService _folderDataService;
        public readonly IMemeDataService _memeDataService;
        public readonly DataStorage _dataStorage;

        public readonly ClientConfigService _clientConfigService;

        public ServiceCollectionClass(IFolderDataService folderDataService,
            IMemeDataService memeDataService,
            IDialogService dialogService,
            INavigationService navigationService,
            DataStorage searchService,
            IMemeTagDataService memeTagDataService,
            ClientConfigService clientConfigService)
        {
            _folderDataService = folderDataService;
            _memeDataService = memeDataService;
            _dialogService = dialogService;
            _navigationService = navigationService;
            _dataStorage = searchService;
            _memeTagDataService = memeTagDataService;

            _clientConfigService = clientConfigService;
        }
    }
}
