using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using MemeFolder.ViewModels.DialogVM;
using System;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.Commands
{
    public class OpenAddMemeDialogCommand : AsyncCommandBase
    {
        private readonly ServiceCollectionClass _serviceCollectionClass;
        private readonly DataStorage _dataStorage;

        private readonly string _dialogId;

        protected override async Task ExecuteAsync(object parameter)
        {
            Folder folder = (Folder)parameter;

            DialogMemeVM dialogMemeVM = new DialogMemeVM(new Meme(), folder, _serviceCollectionClass, "Создание мема");

            Meme meme = (Meme)await MaterialDesignThemes.Wpf.DialogHost.Show(dialogMemeVM, _dialogId);
            dialogMemeVM.Dispose();
            dialogMemeVM = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (meme == null || string.IsNullOrEmpty(meme.ImagePath))
                return;

            meme.Folder = folder;
            await _dataStorage.AddMeme(meme, folder);
        }

        public OpenAddMemeDialogCommand(ServiceCollectionClass services,
           Action<Exception> onException = null, string dialogId = "RootDialog") : base(onException)
        {
            _serviceCollectionClass = services;
            _dataStorage = services._dataStorage;

            _dialogId = dialogId;
        }
    }
}
