using MemeFolder.Abstractions;
using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using System;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.Commands.Memes
{
    /// <summary> Команда удаления Meme </summary>
    public class RemoveMemeCommand : AsyncCommandBase
    {
        private readonly IFolderObjectWorker _folderVM;
        private readonly IMemeDataService _memeDataService;

        protected override async Task ExecuteAsync(object parameter)
        {
            var meme = parameter as Meme;

            if (await _memeDataService.Delete(meme.Id))
            {
                _folderVM.GetWorkerCollection().Remove(meme);
            }
        }

        public RemoveMemeCommand(IFolderObjectWorker folderVM,
            IMemeDataService memeDataService,
            Action<Exception> onException = null) : base(onException)
        {
            _folderVM = folderVM;
            _memeDataService = memeDataService;
        }

    }
}
