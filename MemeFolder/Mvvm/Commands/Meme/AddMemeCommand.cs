using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using System;
using System.IO;
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
            
            string path = _dialogService.FileBrowserDialog();
            if (string.IsNullOrEmpty(path))
                return;

            Meme meme = new Meme()
            {
                Title = Path.GetFileNameWithoutExtension(path),
                ImagePath = path,
                Folder = folder
            };

            await _dataStorage.AddMeme(meme, folder);
        }

        public AddMemeCommand(ServiceCollectionClass services,
            Action<Exception> onException = null) : base(onException)
        {
            _dataStorage = services._dataStorage;
            _dialogService = services._dialogService;
        }
    }
}
