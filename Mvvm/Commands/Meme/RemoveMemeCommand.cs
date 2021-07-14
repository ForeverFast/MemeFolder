using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using System;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.Commands
{
    /// <summary> Команда удаления Meme </summary>
    public class RemoveMemeCommand : AsyncCommandBase
    {
        private readonly DataStorage _dataStorage;

        protected override async Task ExecuteAsync(object parameter)
        {
            Meme meme = (Meme)parameter;
            await _dataStorage.RemoveMeme(meme);
        }

        public RemoveMemeCommand(ServiceCollectionClass services,
            Action<Exception> onException = null) : base(onException)
        {
            _dataStorage = services._dataStorage;
        }

    }
}
