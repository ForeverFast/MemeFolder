using MemeFolder.Abstractions;
using MemeFolder.Domain.Models;
using MemeFolder.Extentions;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using MemeFolder.ViewModels.DialogVM;
using System;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.Commands
{
    /// <summary> Команда открытия диалога добавления производных FolderObject </summary>
    public class OpenAddDialogCommand : AsyncCommandBase
    {
        private readonly IFolderObjectWorker _folderVM;
        private readonly IDialogService _dialogService;
        private readonly IMemeDataService _memeDataService;
        private readonly IFolderDataService _folderDataService;

        protected override async Task ExecuteAsync(object parameter)
        {
            switch (parameter.ToString())
            {
                case "Meme":

                    DialogMemeVM memeVM = new DialogMemeVM(new Meme(), _dialogService, "Создание мема");

                    Meme meme = (Meme)await MaterialDesignThemes.Wpf.DialogHost.Show(memeVM, "RootDialog");
                    if (meme == null || string.IsNullOrEmpty(meme.ImagePath))
                        break;

                    meme.Folder = _folderVM.GetModel();
                    meme.ImageData = MemeExtentions.ConvertImageToByteArray(meme.ImagePath);
                    Meme CreatedMemeEnitiy = await _memeDataService.Create(meme);
                    if (CreatedMemeEnitiy != null)
                    {
                        CreatedMemeEnitiy.Image = MemeExtentions.ConvertByteArrayToImage(CreatedMemeEnitiy.ImageData);
                        _folderVM.GetWorkerCollection().Add(CreatedMemeEnitiy);
                    }
                        
                    break;

                case "Folder":

                    DialogFolderVM folderVM = new DialogFolderVM(new Folder(), _dialogService, "Создание папки");

                    Folder folder = (Folder)await MaterialDesignThemes.Wpf.DialogHost.Show(folderVM, "RootDialog");
                    if (folder == null)
                        break;

                    Folder CreatedFolderEnitiy = await _folderDataService.Create(folder);
                    if (CreatedFolderEnitiy != null)
                        _folderVM.GetWorkerCollection().Add(CreatedFolderEnitiy);

                    break;
            }

        }

        public OpenAddDialogCommand(IFolderObjectWorker folderVM,
            DataService dataService,
            Action<Exception> onException = null) : base(onException)
        {
            _folderVM = folderVM;
            _dialogService = dataService._dialogService;
            _memeDataService = dataService._memeDataService;
            _folderDataService = dataService._folderDataService;
        }

    }
}
