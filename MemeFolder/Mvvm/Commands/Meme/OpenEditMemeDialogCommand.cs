using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using MemeFolder.ViewModels.DialogVM;
using System;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.Commands
{
    public class OpenEditMemeDialogCommand : AsyncCommandBase
    {
        private readonly ServiceCollectionClass _dataService;
        private readonly DataStorage _dataStorage;

        private readonly string _dialogId;

        protected override async Task ExecuteAsync(object parameter)
        {
            Meme meme = (Meme)parameter;

            DialogMemeVM dialogMemeVM = new DialogMemeVM(meme, meme.Folder, _dataService, "Редактирование мема");

            Meme editedMeme = (Meme)await MaterialDesignThemes.Wpf.DialogHost.Show(dialogMemeVM, _dialogId);
            dialogMemeVM.Dispose();
            dialogMemeVM = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (editedMeme == null || string.IsNullOrEmpty(editedMeme.ImagePath))
                return;

            await _dataStorage.EditMeme(editedMeme);
        }

        public OpenEditMemeDialogCommand(ServiceCollectionClass services,
           Action<Exception> onException = null, string dialogId = "RootDialog") : base(onException)
        {
            _dataService = services;
            _dataStorage = services._dataStorage;

            _dialogId = dialogId;
        }
    }
}
