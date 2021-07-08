using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using MemeFolder.ViewModels.DialogVM;
using System;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.Commands
{
    public class OpenAddMemeTagDialogCommand : AsyncCommandBase
    {
        private readonly DataStorage _dataStorage;

        private readonly string _dialogId;

        protected override async Task ExecuteAsync(object parameter)
        {
            DialogTagVM dialogMemeTagVM = new DialogTagVM(new MemeTag(), "Создание тега");

            MemeTag memeTag = (MemeTag)await MaterialDesignThemes.Wpf.DialogHost.Show(dialogMemeTagVM, _dialogId);
            if (memeTag == null)
                return;

            await _dataStorage.AddMemeTag(memeTag);
        }

        public OpenAddMemeTagDialogCommand(DataService dataService,
           Action<Exception> onException = null, string dialogId = "RootDialog") : base(onException)
        {
            _dataStorage = dataService._dataStorage;

            _dialogId = dialogId;
        }
    }
}
