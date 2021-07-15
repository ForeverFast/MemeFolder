using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using System;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.Commands
{
    /// <summary> Команда добавления Folder </summary>
    public class AddFolderCommand : AsyncCommandBase
    {
        private readonly DataStorage _dataStorage;

        protected override async Task ExecuteAsync(object parameter)
        {
            Folder parentFolder = (Folder)parameter;
            Folder newFolder = new Folder()
            {
                ParentFolder = parentFolder
            };

            await _dataStorage.AddFolder(newFolder, parentFolder);
        }

        public AddFolderCommand(ServiceCollectionClass services,
           Action<Exception> onException = null) : base(onException)
        {
            _dataStorage = services._dataStorage;
        }
    }
}
