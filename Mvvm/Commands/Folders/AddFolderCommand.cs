using MemeFolder.Abstractions;
using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using System;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.Commands.Folders
{
    public class AddFolderCommand : AsyncCommandBase
    {
        private readonly IFolderVM _folderVM;
        private readonly IFolderDataService _folderDataService;


        protected override async Task ExecuteAsync(object parameter)
        {
            Folder NewFolder = new Folder()
            {
                ParentFolder = _folderVM.Model,
                Title = "Новая папка"
            };

            var createdEntity = await _folderDataService.Create(NewFolder);
            if (createdEntity != null)
            {
                _folderVM.FolderObjects.Add(createdEntity);
            }
        }

        public AddFolderCommand(IFolderVM folderVM,
           IFolderDataService folderDataService,
           Action<Exception> onException = null) : base(onException)
        {
            _folderVM = folderVM;
            _folderDataService = folderDataService;
        }
    }
}
