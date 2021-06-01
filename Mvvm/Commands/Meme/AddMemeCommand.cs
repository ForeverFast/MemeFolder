using MemeFolder.Abstractions;
using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using System;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.Commands.Memes
{
    public class AddMemeCommand : AsyncCommandBase
    {
        private readonly IFolderVM _folderVM;
        private readonly IDialogService _dialogService;
        private readonly IMemeDataService _memeDataService;

        protected override async Task ExecuteAsync(object parameter)
        {
            var URL = _dialogService.FileBrowserDialog();
            Meme meme = new Meme()
            {
                Title = "Новый мемчик",
                ImagePath = URL
            };
            var createdEntity = await _memeDataService.Create(meme, _folderVM.Model.Id);
            if (createdEntity != null)
            {
                _folderVM.FolderObjects.Add(meme);
            }
        }

        public AddMemeCommand(IFolderVM folderVM,
            DataService dataService,
            Action<Exception> onException = null) : base(onException)
        {
            _folderVM = folderVM;
            _memeDataService = dataService._memeDataService;
             _dialogService = dataService._dialogService;
        }
    }
}
