using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using System;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.Commands
{
    /// <summary> Команда добавления Meme </summary>
    public class AddMemeCommand : AsyncCommandBase
    {
        private readonly DataStorage _dataStorage;
        private readonly IDialogService _dialogService;
      
        protected override async Task ExecuteAsync(object parameter)
        {
            Folder folder = (Folder)parameter;
            
            var path = _dialogService.FileBrowserDialog();
            Meme meme = new Meme()
            {
                Title = "Новый мемчик",
                ImagePath = path,
                Folder = folder
            };

            await _dataStorage.AddMeme(meme, folder);
        }

        public AddMemeCommand(DataService dataService,
            Action<Exception> onException = null) : base(onException)
        {
            _dataStorage = dataService._dataStorage;
            _dialogService = dataService._dialogService;
        }
    }
}
