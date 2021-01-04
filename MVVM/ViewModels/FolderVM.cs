using Egor92.MvvmNavigation.Abstractions;
using MemeFolder.Domain.Models;
using MemeFolder.Domain.Models.AbstractModels;
using MemeFolder.Mvvm.Commands;
using MemeFolder.MVVM.Views.Pages;
using MemeFolder.Pc.Mvvm;
using MemeFolder.Pc.Mvvm.ViewModels.Abstractions;
using MemeFolder.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MemeFolder.MVVM.ViewModels
{
    public class FolderVM : BasePageViewModel
    {
        #region Поля
        private Folder _model;
        private IFolderDataService _folderDataService;
        private IMemeDataService _memeDataService;
        private IDialogService _dialogService;
        #endregion

        public Folder Model { get => _model; set => SetProperty(ref _model, value); }
        public ObservableCollection<FolderVM> Children { get; }
        public ObservableCollection<FolderObject> FolderObjects { get; }


        #region Команды - Папки

        public ICommand AddFolder { get; }

        public ICommand RemoveFolder { get; }

        public async Task AddFolderExecute(object parameter)
        {
            IsDialogOpen = false;
            var parameters = (object[])parameter;
            Folder t = new Folder()
            {
                ParentFolder = Model,
                Title = parameters[0].ToString(),
                Description = parameters[1].ToString()
            };
            Model.Folders.Add(t);
            var res = await _folderDataService.Update(Model.Id, Model);
            FolderObjects.Add(t);
        }

        public async Task RemoveFolderExecute(object parameter)
        {
            await _folderDataService.Delete((parameter as Folder).Id);
            FolderObjects.Remove((parameter as Folder));
        }

        #region Логика - Диалоговое окно

        public ICommand OpenDialogCommand
        {
            get => new AsyncRelayCommand( async (o) => {

                switch(o.ToString())
                {
                    case "Meme":

                        var memeModel = new Meme();
                        memeModel.Folder = Model;

                        object meme = await MaterialDesignThemes.Wpf.DialogHost.Show(new MemeVM(memeModel,
                                                                                                _navigationManager,
                                                                                                _dialogService), "RootDialog");

                        Model.Memes.Add(meme as Meme);
                        await _folderDataService.Update(Model.Id, Model);
                        FolderObjects.Add(meme as Meme);
                        break;

                    case "Folder":
                        object folder = await MaterialDesignThemes.Wpf.DialogHost.Show(this, "RootDialog");
                        break;
                }

            });
        }

        public ICommand SetPlayListImage
        {
            get => new RelayCommand((o) => {
                TempImageUri = _dialogService.FileBrowserDialog("*.jpg;*.png");
            });
        }


        private bool _isDialogOpen;
        private string _tempImageUri;
        public bool IsDialogOpen { get => _isDialogOpen; set { SetProperty(ref _isDialogOpen, value); if (value == false) TempImageUri = ""; } }
        public string TempImageUri { get => _tempImageUri; set => SetProperty(ref _tempImageUri, value); }

        #endregion

        #endregion

        #region Команды - Мемы


        public ICommand OpenMemePicture { get; }
        public ICommand CopyMemeInBuffer { get; }
        public ICommand RemoveMeme { get; }

        private void CopyMemeInBufferExecute(object parameter)
        {
            Clipboard.SetImage(new BitmapImage(new Uri((parameter as Meme).ImagePath)));
        }

        private void OpenMemePictureExecute(object parameter)
        {
            Process.Start((parameter as Meme).ImagePath);
        }

        private void RemoveMemeExecute(object parameter)
        {
            
        }


        #endregion

        #region Команды - Навигациия

        private void NavigationToFolderExecute(object parameter)
            => _navigationManager.Navigate<FolderPage>((parameter as Folder).Id.ToString(),
                                                        Children.FirstOrDefault(x => x.Model == (parameter as Folder)),null);

        #endregion


        #region Конструкторы
        private FolderVM(INavigationManager navigationManager,
                         IDialogService dialogService) : base(navigationManager)
        {
            _dialogService = dialogService;

            Children = new ObservableCollection<FolderVM>();
            FolderObjects = new ObservableCollection<FolderObject>();

            AddFolder = new AsyncRelayCommand(AddFolderExecute);
            RemoveFolder = new AsyncRelayCommand(RemoveFolderExecute);

            OpenMemePicture = new RelayCommand(OpenMemePictureExecute);
            CopyMemeInBuffer = new RelayCommand(CopyMemeInBufferExecute);
            RemoveMeme = new RelayCommand(RemoveMemeExecute);

            //Clipboard.SetImage(new BitmapImage(new Uri()));
            //AddMeme = new AsyncRelayCommand(AddMemeExecute);


            NavigationToFolderCommand = new RelayCommand(NavigationToFolderExecute, null);
           
        }

        public FolderVM(INavigationManager navigationManager,
                        IFolderDataService folderDataService,
                        IMemeDataService memeDataService,
                        IDialogService dialogService) : this(navigationManager, dialogService)
        {
            _folderDataService = folderDataService;
            _memeDataService = memeDataService;

            var temp = Task.Run(() => _folderDataService.GetAll());
            Model = temp.Result.FirstOrDefault(f => f.Title == "root");

            DownloadData();
        }

        public FolderVM(Folder model,
                        INavigationManager navigationManager,
                        IFolderDataService folderDataService,
                        IMemeDataService memeDataService,
                        IDialogService dialogService) : this(navigationManager, dialogService)
        {
            _folderDataService = folderDataService;
            _memeDataService = memeDataService;

            Model = model;

            DownloadData();
        }

        private void DownloadData()
        {
            foreach (var item in Model.Folders)
            {
                Children.Add(new FolderVM(item,
                                          _navigationManager,
                                          _folderDataService,
                                          _memeDataService,
                                          _dialogService));
                FolderObjects.Add(item);
            }

            foreach (var item in Model.Memes)
                FolderObjects.Add(item);
        }

        #endregion
    }
}
