using MemeFolder.Abstractions;
using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using MemeFolder.ViewModels;
using MemeFolder.ViewModels.DialogVM;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.Commands.Memes
{
    public class OpenAddDialogCommand : AsyncCommandBase
    {
        private readonly IFolderVM _folderVM;
        private readonly IDialogService _dialogService;
        private readonly IMemeDataService _memeDataService;
        private readonly IFolderDataService _folderDataService;

        protected override async Task ExecuteAsync(object parameter)
        {
            switch (parameter.ToString())
            {
                case "Meme":

                    var memeModel = new Meme();
                    var memeVM = new DialogMemeVM(memeModel, _dialogService);

                    object meme = await MaterialDesignThemes.Wpf.DialogHost.Show(memeVM, "RootDialog");

                    if (meme == null)
                        break;

                    var CreatedMemeEnitiy = await _memeDataService.Create(meme as Meme);
                    if (CreatedMemeEnitiy != null)
                        _folderVM.FolderObjects.Add(CreatedMemeEnitiy);

                    break;

                case "Folder":

                    var folderModel = new Folder();
                    var folderVM = new DialogFolderVM(folderModel, _dialogService);

                    object folder = await MaterialDesignThemes.Wpf.DialogHost.Show(folderVM, "RootDialog");

                    if (folder == null)
                        break;

                    var CreatedFolderEnitiy = await _folderDataService.Create(folder as Folder);
                    if (CreatedFolderEnitiy != null)
                        _folderVM.FolderObjects.Add(CreatedFolderEnitiy);

                    break;
            }

        }

        public OpenAddDialogCommand(IFolderVM folderVM,
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
