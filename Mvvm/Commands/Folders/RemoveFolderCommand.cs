using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using System;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.Commands
{
    /// <summary> Команда удаления Folder </summary>
    public class RemoveFolderCommand : AsyncCommandBase
    {
        private readonly DataStorage _dataStorage;

        public override bool CanExecute(object parameter)
        {
            if (parameter is Folder folder)
            {
                if (folder.Title == "root")
                    return false;
            }

            return base.CanExecute(parameter);
        }
        protected override async Task ExecuteAsync(object parameter)
        {
            Folder folder = (Folder)parameter;
            await _dataStorage.RemoveFolder(folder);
        }

        public RemoveFolderCommand(ServiceCollectionClass services,
          Action<Exception> onException = null) : base(onException)
        {
            _dataStorage = services._dataStorage;
        }
    }
}
