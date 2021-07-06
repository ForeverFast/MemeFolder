using MemeFolder.Abstractions;
using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using System;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.Commands.Folders
{
    /// <summary> Команда удаления Folder </summary>
    public class RemoveFolderCommand : AsyncCommandBase
    {
        private readonly IFolderObjectWorker _folderVM;
        private readonly IFolderDataService _folderDataService;


        protected override async Task ExecuteAsync(object parameter)
        {
            if (await _folderDataService.Delete((parameter as Folder).Id))
            {
                _folderVM.GetWorkerCollection().Remove(parameter as Folder);
            }
        }

        public RemoveFolderCommand(IFolderObjectWorker folderVM,
          IFolderDataService folderDataService,
          Action<Exception> onException = null) : base(onException)
        {
            _folderVM = folderVM;
            _folderDataService = folderDataService;
        }
    }
}
