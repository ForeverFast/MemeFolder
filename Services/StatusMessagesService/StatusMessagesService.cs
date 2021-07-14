using MemeFolder.Domain.Models;
using MemeFolder.Navigation;
using MemeFolder.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemeFolder.Services.StatusMessagesService
{
    public class StatusMessagesService : IStatusMessagesService
    {
        private readonly IStatusMessagesProvider _mainWindowV;

        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        private readonly IMemeTagDataService _memeTagDataService;
        private readonly IFolderDataService _folderDataService;
        private readonly IMemeDataService _memeDataService;
        private readonly DataStorage _dataStorage;
       
        public event StatusMessageDelegate NewMessage;

        public StatusMessagesService(ServiceCollectionClass services, MainWindow mainWindowV)
        {
            _mainWindowV = (IStatusMessagesProvider)mainWindowV;
            _navigationService = services._navigationService;
            _dialogService = services._dialogService;

            _memeTagDataService = services._memeTagDataService;
            _folderDataService = services._folderDataService;
            _memeDataService = services._memeDataService;

            //_dataStorage = services._dataStorage;
            //_dataStorage.OnAddFolder += _dataStorage_OnAddFolder;
            //_dataStorage.OnEditFolder += _dataStorage_OnEditFolder;
            //_dataStorage.OnRemoveFolder += _dataStorage_OnRemoveFolder;

            //_dataStorage.OnAddMeme += _dataStorage_OnAddMeme;
            //_dataStorage.OnEditMeme += _dataStorage_OnEditMeme;
            //_dataStorage.OnRemoveMeme += _dataStorage_OnRemoveMeme;

            //_dataStorage.OnAddMemeTag += _dataStorage_OnAddMemeTag;
            //_dataStorage.OnEditMemeTag += _dataStorage_OnEditMemeTag;
            //_dataStorage.OnRemoveMemeTag += _dataStorage_OnRemoveMemeTag;

        }

        private void _dataStorage_OnAddMeme(Meme meme)
        {
            
        }

        private void _dataStorage_OnEditMeme(Meme meme)
        {
            throw new NotImplementedException();
        }
        private void _dataStorage_OnRemoveMeme(Meme meme)
        {
            throw new NotImplementedException();
        }

        private void _dataStorage_OnAddFolder(Folder folder)
        {
            throw new NotImplementedException();
        }

        private void _dataStorage_OnEditFolder(Folder folder)
        {
            throw new NotImplementedException();
        }

        private void _dataStorage_OnRemoveFolder(Folder folder)
        {
            throw new NotImplementedException();
        }

        private void _dataStorage_OnAddMemeTag(MemeTag memeTag)
        {
            throw new NotImplementedException();
        }

        private void _dataStorage_OnEditMemeTag(MemeTag memeTag)
        {
            throw new NotImplementedException();
        }

        private void _dataStorage_OnRemoveMemeTag(MemeTag memeTag)
        {
            throw new NotImplementedException();
        }

    }
}
