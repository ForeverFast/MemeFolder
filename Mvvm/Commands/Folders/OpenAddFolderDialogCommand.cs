using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using MemeFolder.ViewModels.DialogVM;
using System;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.Commands
{
    public class OpenAddFolderDialogCommand : AsyncCommandBase
    {
        private readonly DataStorage _dataStorage;

        private readonly string _dialogId;

        protected override async Task ExecuteAsync(object parameter)
        {
            Folder parentFolder = (Folder)parameter;

            DialogFolderVM dialogFolderVM = new DialogFolderVM(new Folder(), "Создание папки");

            Folder newFolder = (Folder)await MaterialDesignThemes.Wpf.DialogHost.Show(dialogFolderVM, _dialogId);
            if (newFolder == null)
                return;

            newFolder.ParentFolder = parentFolder;

            await _dataStorage.AddFolder(newFolder, parentFolder);
        }

        public OpenAddFolderDialogCommand(DataService dataService,
           Action<Exception> onException = null, string dialogId = "RootDialog") : base(onException)
        {
            _dataStorage = dataService._dataStorage;

            _dialogId = dialogId;
        }
    }
}
