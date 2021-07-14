using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.Commands
{
    public class RemoveMemeTagCommand : AsyncCommandBase
    {

        private readonly DataStorage _dataStorage;

        public override bool CanExecute(object parameter) => true;

        protected override async Task ExecuteAsync(object parameter)
        {
            MemeTag memeTag = (MemeTag)parameter;
            await _dataStorage.RemoveMemeTag(memeTag);

        }

        public RemoveMemeTagCommand(ServiceCollectionClass services,
            Action<Exception> onException = null) : base(onException)
        {
            _dataStorage = services._dataStorage;
        }
    }
}
