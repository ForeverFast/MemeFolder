using MemeFolder.Abstractions;
using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.Commands.Folders
{
    /// <summary> Команда удаления Folder </summary>
    public class RemoveFolderCommand : AsyncCommandBase
    {
        private readonly IObjectWorker _folderVM;
        private readonly IFolderDataService _folderDataService;

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
            if (await _folderDataService.Delete((parameter as Folder).Id))
            {
                ObservableCollection<Folder> folders = (ObservableCollection<Folder>)_folderVM.GetWorkerCollection(ObjectType.Folder);
                folders.Remove(parameter as Folder);
            }
        }

        public RemoveFolderCommand(IObjectWorker folderVM,
          IFolderDataService folderDataService,
          Action<Exception> onException = null) : base(onException)
        {
            _folderVM = folderVM;
            _folderDataService = folderDataService;
        }
    }
}
