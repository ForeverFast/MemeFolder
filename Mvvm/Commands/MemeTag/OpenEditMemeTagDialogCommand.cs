using MemeFolder.Domain.Models;
using MemeFolder.Extentions;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using MemeFolder.ViewModels.DialogVM;
using System;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.Commands
{
    public class OpenEditMemeTagDialogCommand : AsyncCommandBase
    {
        private readonly DataStorage _dataStorage;

        private readonly string _dialogId;

        protected override async Task ExecuteAsync(object parameter)
        {
            MemeTag memeTag = (MemeTag)parameter;

            DialogTagVM dialogMemeTagVM = new DialogTagVM(memeTag, "Редактирование папки");

            MemeTag editedMemeTag = (MemeTag)await MaterialDesignThemes.Wpf.DialogHost.Show(dialogMemeTagVM, _dialogId);
            if (editedMemeTag == null)
                return;

            await _dataStorage.EditMemeTag(memeTag);
        }

        public OpenEditMemeTagDialogCommand(ServiceCollectionClass services,
           Action<Exception> onException = null, string dialogId = "RootDialog") : base(onException)
        {
            _dataStorage = services._dataStorage;

            _dialogId = dialogId;
        }
    }
}
