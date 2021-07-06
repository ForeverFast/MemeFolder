using MemeFolder.Abstractions;
using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using System;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.Commands.Folders
{
    /// <summary> Команда добавления Folder </summary>
    public class AddFolderCommand : AsyncCommandBase
    {
        private readonly IFolderObjectWorker _folderVM;
        private readonly IFolderDataService _folderDataService;

        protected override async Task ExecuteAsync(object parameter)
        {
            Folder NewFolder = new Folder()
            {
                ParentFolder = _folderVM.GetModel(),
                Title = "Новая папка"
            };

            Folder createdEntity = await _folderDataService.Create(NewFolder);
            if (createdEntity != null)
            {
                _folderVM.GetWorkerCollection().Add(createdEntity);
            }
        }

        public AddFolderCommand(IFolderObjectWorker folderVM,
           IFolderDataService folderDataService,
           Action<Exception> onException = null) : base(onException)
        {
            _folderVM = folderVM;
            _folderDataService = folderDataService;
        }
    }
}
