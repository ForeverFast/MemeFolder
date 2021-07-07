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
    /// <summary> Команда открытия диалога добавления производных FolderObject </summary>
    public class OpenAddDialogCommand : AsyncCommandBase
    {
        private readonly IObjectWorker _folderVM;
        private readonly IDialogService _dialogService;
        private readonly IMemeDataService _memeDataService;
        private readonly IFolderDataService _folderDataService;
        private readonly IMemeTagDataService _memeTagDataService;

        protected override async Task ExecuteAsync(object parameter)
        {
            object[] parameters = (object[])parameter;


            ObjectType objectType = (ObjectType)parameters[1];

            switch (objectType)
            {
                case ObjectType.Meme:

                    DialogMemeVM dialogMemeVM = new DialogMemeVM(new Meme(), _dialogService, "Создание мема");

                    Meme meme = (Meme)await MaterialDesignThemes.Wpf.DialogHost.Show(dialogMemeVM, "RootDialog");
                    if (meme == null || string.IsNullOrEmpty(meme.ImagePath))
                        break;

                    meme.Folder = (Folder)parameters[0];
                    meme.ImageData = MemeExtentions.ConvertImageToByteArray(meme.ImagePath);
                    Meme CreatedMemeEnitiy = await _memeDataService.Create(meme);
                    if (CreatedMemeEnitiy != null)
                    {
                        CreatedMemeEnitiy.Image = MemeExtentions.ConvertByteArrayToImage(CreatedMemeEnitiy.ImageData);
                        ObservableCollection<Meme> memes = (ObservableCollection<Meme>)_folderVM.GetWorkerCollection(ObjectType.Meme);
                        memes.Add(CreatedMemeEnitiy);
                    }

                    break;

                case ObjectType.Folder:

                    DialogFolderVM dialogFolderVM = new DialogFolderVM(new Folder(), _dialogService, "Создание папки");

                    Folder parentFolder = (Folder)parameters[0];

                    Folder folder = (Folder)await MaterialDesignThemes.Wpf.DialogHost.Show(dialogFolderVM, "RootDialog");
                    if (folder == null)
                        break;

                    folder.ParentFolder = parentFolder;
                    Folder CreatedFolderEnitiy = await _folderDataService.Create(folder);
                    if (CreatedFolderEnitiy != null)
                    {
                        parentFolder.Folders.Add(CreatedFolderEnitiy);
                    }
                       

                    break;
                case ObjectType.MemeTag:

                    DialogTagVM dialogMemeTagVM = new DialogTagVM(new MemeTag(), "Создание тега");

                    MemeTag memeTag = (MemeTag)await MaterialDesignThemes.Wpf.DialogHost.Show(dialogMemeTagVM, "RootDialog");
                    if (memeTag == null)
                        break;

                    MemeTag CreatedMemeTagEnitiy = await _memeTagDataService.Create(memeTag);
                    if (CreatedMemeTagEnitiy != null)
                    {
                        ObservableCollection<MemeTag> memeTags = (ObservableCollection<MemeTag>)_folderVM.GetWorkerCollection(ObjectType.MemeTag);
                        memeTags.Add(CreatedMemeTagEnitiy);
                    }


                    break;
            }

        }

        public OpenAddDialogCommand(IObjectWorker folderVM,
            DataService dataService,
            Action<Exception> onException = null) : base(onException)
        {
            _folderVM = folderVM;
            _dialogService = dataService._dialogService;
            _memeDataService = dataService._memeDataService;
            _folderDataService = dataService._folderDataService;
            _memeTagDataService = dataService._memeTagDataService;
        }

    }
}
