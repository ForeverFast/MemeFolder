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
                ParentFolder = parentFolder,
                Title = "Новая папка"
            };

            await _dataStorage.AddFolder(newFolder, parentFolder);
        }

        public AddFolderCommand(DataService dataService,
           Action<Exception> onException = null) : base(onException)
        {
            _dataStorage = dataService._dataStorage;
        }
    }
}
