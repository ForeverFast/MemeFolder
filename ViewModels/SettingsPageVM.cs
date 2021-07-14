using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Navigation;
using MemeFolder.Services;
using MemeFolder.ViewModels.Abstractions;
using System.Windows.Input;

namespace MemeFolder.ViewModels
{
    public class SettingsPageVM : BasePageViewModel
    {
        private readonly IDialogService _dialogService;

        private readonly ClientConfigService ClientConfigService;


        private string _filesPath;

        public string FilesPath { get => _filesPath; set => SetProperty(ref _filesPath, value); }


        public ICommand SetNewFilesPathCommand { get; }

        private void SetNewFilesPathExecute(object parameter)
            => FilesPath = ClientConfigService.FilesPath = _dialogService.FolderBrowserDialog();
        

        public SettingsPageVM(ServiceCollectionClass serviceCollectionClass) : base(serviceCollectionClass._navigationService)
        {
            ClientConfigService = serviceCollectionClass._clientConfigService;
            _dialogService = serviceCollectionClass._dialogService;

            FilesPath = ClientConfigService.FilesPath;

            SetNewFilesPathCommand = new RelayCommand(SetNewFilesPathExecute);
        }
    }
}
