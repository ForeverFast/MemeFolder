using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using MemeFolder.ViewModels.DialogVM;
using System;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.Commands
{
    public class OpenEditFolderDialogCommand : AsyncCommandBase
    {
        private readonly DataStorage _dataStorage;

        private readonly string _dialogId;

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

            DialogFolderVM folderVM = new DialogFolderVM(folder, "Редактирование папки");

            Folder editedFolder = (Folder)await MaterialDesignThemes.Wpf.DialogHost.Show(folderVM, _dialogId);
            if (editedFolder == null)
                return;


            await _dataStorage.EditFolder(editedFolder);
        }

        public OpenEditFolderDialogCommand(ServiceCollectionClass services,
           Action<Exception> onException = null, string dialogId = "RootDialog") : base(onException)
        {
            _dataStorage = services._dataStorage;

            _dialogId = dialogId;
        }
    }
}
