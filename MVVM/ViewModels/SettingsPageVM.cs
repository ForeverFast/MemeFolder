using MemeFolder.Navigation;
using MemeFolder.Pc.Mvvm;
using MemeFolder.Pc.Mvvm.ViewModels.Abstractions;
using MemeFolder.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MemeFolder.MVVM.ViewModels
{
    public class SettingsPageVM : BasePageViewModel
    {
        private ClientConfigService ClientConfigService;
        private readonly IDialogService _dialogService;

        private string _filesPath;

        public string FilesPath
        {
            get => _filesPath;
            set
            {
                ClientConfigService.FilesPath = value;
                SetProperty(ref _filesPath, value);
            }
        }

        public ICommand SetNewFilesPathCommand { get; }

        private void SetNewFilesPathExecute(object parameter)
            => FilesPath = _dialogService.FolderBrowserDialog();
        

        public SettingsPageVM(INavigationService navigationService,
                              IDialogService dialogService,
                              ClientConfigService clientConfigService) : base(navigationService)
        {
            ClientConfigService = clientConfigService;
            _dialogService = dialogService;

            FilesPath = ClientConfigService.FilesPath;

            SetNewFilesPathCommand = new RelayCommand(SetNewFilesPathExecute);
        }
    }
}
