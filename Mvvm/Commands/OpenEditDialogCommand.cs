using MemeFolder.Abstractions;
using MemeFolder.Domain.Models;
using MemeFolder.Extentions;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using MemeFolder.ViewModels.DialogVM;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.Commands
{
    /// <summary> Команда открытия диалога изменения производных FolderObject </summary>
    public class OpenEditDialogCommand : AsyncCommandBase
    {
        private readonly IObjectWorker _folderVM;
        private readonly IDialogService _dialogService;
        private readonly IMemeDataService _memeDataService;
        private readonly IFolderDataService _folderDataService;

        public override bool CanExecute(object parameter)
        {
            if (parameter is Folder folder)
            {
                if (folder.Title == "root")
                    return false;
            }

            return base.CanExecute(parameter);
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            if (parameter is Meme meme)
            {
                DialogMemeVM memeVM = new DialogMemeVM(meme, _dialogService, "Редактирование мема");

                Meme editedMeme = (Meme)await MaterialDesignThemes.Wpf.DialogHost.Show(memeVM, "RootDialog");
                if (editedMeme == null || string.IsNullOrEmpty(editedMeme.ImagePath))
                    return;

                editedMeme.ImageData = MemeExtentions.ConvertImageToByteArray(editedMeme.ImagePath);
                Meme UpdatedMemeEnitiy = await _memeDataService.Update(editedMeme.Id, editedMeme);
                if (UpdatedMemeEnitiy != null)
                {
                    UpdatedMemeEnitiy.Image = MemeExtentions.ConvertByteArrayToImage(UpdatedMemeEnitiy.ImageData);
                    ObservableCollection<Meme> memes = (ObservableCollection<Meme>)_folderVM.GetWorkerCollection(ObjectType.Meme);
                    MemeExtentions.ReplaceReference(memes, UpdatedMemeEnitiy, x => x.Id == UpdatedMemeEnitiy.Id);
                    UpdatedMemeEnitiy.OnAllPropertyChanged();
                }
                   
            }
            else if (parameter is Folder folder)
            {
                DialogFolderVM folderVM = new DialogFolderVM(folder, _dialogService, "Редактирование папки");

                Folder editedFolder = (Folder)await MaterialDesignThemes.Wpf.DialogHost.Show(folderVM, "RootDialog");
                if (editedFolder == null)
                    return;

                Folder CreatedFolderEnitiy = await _folderDataService.Update(editedFolder.Id, editedFolder);
                if (CreatedFolderEnitiy != null)
                {
                    ObservableCollection<Folder> folders = (ObservableCollection<Folder>)_folderVM.GetWorkerCollection(ObjectType.Folder);
                    MemeExtentions.ReplaceReference(folders, CreatedFolderEnitiy, x => x.Id == CreatedFolderEnitiy.Id);
                    CreatedFolderEnitiy.OnAllPropertyChanged();
                }
                   
            }
        }

        public OpenEditDialogCommand(IObjectWorker folderVM,
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
