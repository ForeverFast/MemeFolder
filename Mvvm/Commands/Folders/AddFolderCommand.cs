using MemeFolder.Abstractions;
using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.Commands.Folders
{
    /// <summary> Команда добавления Folder </summary>
    public class AddFolderCommand : AsyncCommandBase
    {
        private readonly IObjectWorker _folderVM;
        private readonly IFolderDataService _folderDataService;

        protected override async Task ExecuteAsync(object parameter)
        {
            Folder NewFolder = new Folder()
            {
                ParentFolder = (Folder)parameter,
                Title = "Новая папка"
            };

            Folder createdEntity = await _folderDataService.Create(NewFolder);
            if (createdEntity != null)
            {
                ObservableCollection<Folder> folders = (ObservableCollection<Folder>)_folderVM.GetWorkerCollection(ObjectType.Folder);
                folders.Add(createdEntity);
            }
        }

        public AddFolderCommand(IObjectWorker folderVM,
           IFolderDataService folderDataService,
           Action<Exception> onException = null) : base(onException)
        {
            _folderVM = folderVM;
            _folderDataService = folderDataService;
        }
    }
}
