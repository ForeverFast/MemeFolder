using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.CommandsBase;
using System.Windows.Input;

namespace MemeFolder.ViewModels
{
    public partial class FolderVM
    {
        #region Поля
        private bool _isDialogOpen;
        private string _tempImageUri;
        #endregion

        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set
            { 
                SetProperty(ref _isDialogOpen, value);
                if (value == false)
                    TempImageUri = "";
            }
        }

        public string TempImageUri { get => _tempImageUri; set => SetProperty(ref _tempImageUri, value); }

        #region Логика

        public ICommand OpenDialogCommand
        {
            get => new AsyncRelayCommand(async (o) => {

                switch (o.ToString())
                {
                    case "Meme":

                        var memeModel = new Meme();
                        var memeVM = new MemeVM(memeModel, _navigationService, _dialogService);

                        object meme = await MaterialDesignThemes.Wpf.DialogHost.Show(memeVM, "RootDialog");

                        if (meme == null)
                            break;

                        var CreatedMemeEnitiy = await _memeDataService.Create(meme as Meme);
                        FolderObjects.Add(CreatedMemeEnitiy);

                        break;

                    case "Folder":

                        var folderModel = new Folder();
                        var folderVM = new FolderVM(folderModel, _dataService);

                        object folder = await MaterialDesignThemes.Wpf.DialogHost.Show(folderVM, "RootDialog");

                        if (folder == null)
                            break;

                        var CreatedFolderEnitiy = await _folderDataService.Create(folder as Folder);
                        FolderObjects.Add(CreatedFolderEnitiy);
                        Children.Add(folderVM);

                        break;
                }

            });
        }

       
        

        #endregion
    }
}
