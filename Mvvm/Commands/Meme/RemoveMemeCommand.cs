using MemeFolder.Abstractions;
using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using System;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.Commands.Memes
{
    public class RemoveMemeCommand : AsyncCommandBase
    {
        private readonly IFolderVM _folderVM;
        private readonly IMemeDataService _memeDataService;

        protected override async Task ExecuteAsync(object parameter)
        {
            var meme = parameter as Meme;

            if (await _memeDataService.Delete(meme.Id))
            {
                _folderVM.FolderObjects.Remove(meme);
            }
        }

        public RemoveMemeCommand(IFolderVM folderVM,
            IMemeDataService memeDataService,
            Action<Exception> onException = null) : base(onException)
        {
            _folderVM = folderVM;
            _memeDataService = memeDataService;
        }

    }
}
