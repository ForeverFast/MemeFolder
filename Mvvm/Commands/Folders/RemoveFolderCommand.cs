using MemeFolder.Abstractions;
using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using System;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.Commands.Folders
{

    public class RemoveFolderCommand : AsyncCommandBase
    {
        private readonly IFolderVM _folderVM;
        private readonly IFolderDataService _folderDataService;


        protected override async Task ExecuteAsync(object parameter)
        {
            if (await _folderDataService.Delete((parameter as Folder).Id))
            {
                _folderVM.FolderObjects.Remove((parameter as Folder));
            }
        }

        public RemoveFolderCommand(IFolderVM folderVM,
          IFolderDataService folderDataService,
          Action<Exception> onException = null) : base(onException)
        {
            _folderVM = folderVM;
            _folderDataService = folderDataService;
        }
    }
}
