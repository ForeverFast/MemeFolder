using Egor92.MvvmNavigation.Abstractions;
using GongSolutions.Wpf.DragDrop;
using MemeFolder.Domain.Models;
using MemeFolder.Domain.Models.AbstractModels;
using MemeFolder.Mvvm.Commands;
using MemeFolder.MVVM.Models;
using MemeFolder.MVVM.Views.Pages;
using MemeFolder.Navigation;
using MemeFolder.Pc.Mvvm;
using MemeFolder.Pc.Mvvm.ViewModels.Abstractions;
using MemeFolder.Services;
using MemeFolder.Wpf.Services.ImageAsyncService;
using NAudio.Wave;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MemeFolder.MVVM.ViewModels
{
    public class FolderVM : BasePageViewModel, IDropTarget, INavigatedToAware
    {
        #region Поля
        private Folder _model;
        private readonly IFolderDataService _folderDataService;
        private readonly IMemeDataService _memeDataService;
        private readonly IDialogService _dialogService;

        private readonly DataService _dataService;
        #endregion

        public Folder Model { get => _model; set => SetProperty(ref _model, value); }
        public ObservableCollection<FolderVM> Children { get; private set; }
        //public ObservableCollection<FolderObject> FolderObjects { get; private set; }

        public ImageAsyncCollection<ImageAsync<FolderObject>> FolderObjects { get; private set; }

        #region Команды - Папки

        public ICommand AddFolderCommand { get; }

        public ICommand RemoveFolderCommand { get; }

        public async Task AddFolderExecuteAsync(object parameter)
        {
            Folder NewFolder = new Folder()
            {
                ParentFolder = Model,
                Title = "Новая папка"
            };

            var createdEntity = await _folderDataService.Create(NewFolder);
            if (createdEntity != null)
            {
                Model.Folders.Add(createdEntity);
                FolderObjects.Add(new ImageAsync<FolderObject>(createdEntity));

                Children.Add(new FolderVM(createdEntity,
                                         _dataService));
                createdEntity.PropertyChanged += Model_PropertyChanged;
            }
           
        }

        public async Task RemoveFolderExecuteAsync(object parameter)
        {
            var folder = (parameter as ImageAsync<FolderObject>).Content;
            if (await _folderDataService.Delete(folder.Id))
            {
                foreach (var item in FolderObjects)
                {
                    if (item.Content == folder)
                    {
                        FolderObjects.Remove(item);
                        break;
                    }
                }
               
            }
           
        }

       

        

        #endregion


        #region Команды - Мемы

        public ICommand OpenMemePictureCommand { get; }
        public ICommand CopyMemeInBufferCommand { get; }
        public ICommand AddMemeCommand { get; }
        public ICommand RemoveMemeCommand { get; }

        private void CopyMemeInBufferExecute(object parameter)
        {
            Clipboard.SetImage(new BitmapImage(new Uri((parameter as Meme).ImagePath)));
        }

        private void OpenMemePictureExecute(object parameter)
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo((parameter as Meme).ImagePath)
            {
                UseShellExecute = true
            };
            p.Start();

            WaveStream waveStream = new WaveFileReader(Properties.Resources.bababooey_sound_effect);
            var waveOut = new WaveOut();
            waveOut.Init(waveStream);
            waveOut.Volume = 0.1f;
            waveOut.Play();
        }

        private async Task AddMemeExecuteAsync(object parameter)
        {
            var URL = _dialogService.FileBrowserDialog();
            Meme meme = new Meme()
            {
                Title = "Новый мемчик",
                ImagePath = URL,
                Folder = Model
            };
           
            var createdMeme = await _memeDataService.Create(meme);
            if (createdMeme != null)
            {
                Model.Memes.Add(meme);
                FolderObjects.Add(new ImageAsync<FolderObject>(meme.ImagePath ,meme));
            }
        }

        private async Task RemoveMemeExecuteAsync(object parameter)
        {
            var meme = parameter as Meme;
           
            if (await _memeDataService.Delete(meme.Id))
            {
                foreach (var item in FolderObjects)
                {
                    if (item.Content == meme)
                    {
                        FolderObjects.Remove(item);
                        break;
                    }
                }
                
            }
        }


        #endregion


        #region Команды - Навигациия

        private void NavigationToFolderExecute(object parameter)
        {
            var folder = (parameter as ImageAsync<FolderObject>).Content;
            _navigationService.Navigate<FolderPage>(folder.Id.ToString(),
                                                    Children.FirstOrDefault(x => x.Model == folder),
                                                    null);
        }
            //=> _navigationService.Navigate<FolderPage>((parameter as ImageAsync<Folder>).Content.Id.ToString(),
            //                                            Children.FirstOrDefault(x => x.Model == (parameter as ImageAsync<Folder>).Content),null);

        #endregion


        #region Команды - Редактирование

        private bool _isOpenForEditTitle;
        public bool IsOpenForEditTitle
        {
            get => _isOpenForEditTitle;
            set => SetProperty(ref _isOpenForEditTitle, value);
        }


        public ICommand OpenEditTitleCommand { get; }
        public ICommand CloseEditTitleCommand { get; }

        private void OpenEditTitleExecute(object parameter)
            => IsOpenForEditTitle = true;
        private void CloseEditTitleExecute(object parameter)
           => IsOpenForEditTitle = false;


        #endregion


        #region Логика - Добавление через проводник

        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;

            var dataObject = dropInfo.Data as IDataObject;

            dropInfo.Effects = dataObject != null && dataObject.GetDataPresent(DataFormats.FileDrop)
                ? DragDropEffects.Copy
                : DragDropEffects.Move;

            
        }

        public async void Drop(IDropInfo dropInfo)
        {
            var dataObject = dropInfo.Data as DataObject;
            if (dataObject != null && dataObject.ContainsFileDropList())
            { 
                var files = dataObject.GetFileDropList();
                foreach (var file in files)
                {
                    if (File.Exists(file))
                    {
                        Meme meme = new Meme()
                        {
                            Title = Path.GetFileName(file),
                            ImagePath = file

                        };

                        var createdMeme = await _memeDataService.Create(meme);
                        if (createdMeme != null)
                        {
                            Model.Memes.Add(createdMeme);
                            FolderObjects.Add(new ImageAsync<FolderObject>(createdMeme));
                            createdMeme.PropertyChanged += Model_PropertyChanged;
                        }
                    }
                    else if (Directory.Exists(file)) {

                        Folder folder = new Folder();
                        folder.ParentFolder = Model;
                        folder.ImageFolderPath = file;
                        folder.Title = new DirectoryInfo(file).Name;

                        var createdFolder = await _folderDataService.Create(folder);
                        if (createdFolder != null)
                        {
                            var updatedFolder = await GetAllFiles(createdFolder);
                            Children.Add(new FolderVM(updatedFolder, _dataService));
                           
                            Model.Folders.Add(updatedFolder);
                            FolderObjects.Add(new ImageAsync<FolderObject>(updatedFolder));
                            updatedFolder.PropertyChanged += Model_PropertyChanged;
                        }
                    }

                }
            }
        }

        public async Task<Folder> GetAllFiles(Folder rootFolder)
        {
            string[] directories = Directory.GetDirectories(rootFolder.ImageFolderPath);
           
            var files = Directory.EnumerateFiles(rootFolder.ImageFolderPath, "*.*", SearchOption.TopDirectoryOnly)
                    .Where(s => s.EndsWith(".png") || s.EndsWith(".jpg")).ToArray();
      
            if (files.Length != 0)
                foreach (var path in files)
                {
                    Meme meme = new Meme()
                    {
                        Title = Path.GetFileName(path),
                        ImagePath = path,
                        Folder = rootFolder
                    };

                    var createdMeme = await _memeDataService.Create(meme);
                    if (createdMeme != null)
                    {
                        rootFolder.Memes.Add(createdMeme);
                    }
                }

            if (directories.Length != 0)
                foreach (string path in directories)
                {
                    Folder f = new Folder();
                    f.ImageFolderPath = path;
                    f.ParentFolder = rootFolder;
                    f.Title = new DirectoryInfo(path).Name;

                    var createdFolder = await _folderDataService.Create(f);
                    if (createdFolder != null)
                    {
                        var updatedFolder = await GetAllFiles(createdFolder);
                        rootFolder.Folders.Add(updatedFolder);
                    }

                }
            return rootFolder;
        }

        #endregion


        #region Логика - Диалоговое окно

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
                        FolderObjects.Add(new ImageAsync<FolderObject>(CreatedMemeEnitiy.ImagePath, CreatedMemeEnitiy));

                        break;

                    case "Folder":

                        var folderModel = new Folder();
                        var folderVM = new FolderVM(folderModel, _dataService);

                        object folder = await MaterialDesignThemes.Wpf.DialogHost.Show(folderVM, "RootDialog");

                        if (folder == null)
                            break;

                        var CreatedFolderEnitiy = await _folderDataService.Create(folder as Folder);

                        FolderObjects.Add(new ImageAsync<FolderObject>(CreatedFolderEnitiy));
                        Children.Add(folderVM);

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


        #region Тест

        public async Task GetData(SearchData searchData)
        {
            foreach (var item in FolderObjects)
                searchData.SearchResult.Add(item.Content);
            foreach (var item in Children)
            {
                searchData.NavigationData.Add(item);
                await item.GetData(searchData);
            }
                
        }

        public ICommand TestCommand { get => new RelayCommand((o) => {

            

        }); }



        #endregion


        #region Конструкторы

        private FolderVM(INavigationService navigationService,
                         IDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            OpenEditTitleCommand = new RelayCommand(OpenEditTitleExecute);
            CloseEditTitleCommand = new RelayCommand(CloseEditTitleExecute);

            AddFolderCommand = new AsyncRelayCommand(AddFolderExecuteAsync);
            RemoveFolderCommand = new AsyncRelayCommand(RemoveFolderExecuteAsync);

            OpenMemePictureCommand = new RelayCommand(OpenMemePictureExecute);
            CopyMemeInBufferCommand = new RelayCommand(CopyMemeInBufferExecute);
            AddMemeCommand = new AsyncRelayCommand(AddMemeExecuteAsync);
            RemoveMemeCommand = new AsyncRelayCommand(RemoveMemeExecuteAsync);



            NavigationToFolderCommand = new RelayCommand(NavigationToFolderExecute);
            PageLoadedCommand = new RelayCommand(PageLoadedExecute);
        }

        public FolderVM(DataService dataService) : this(dataService._navigationService,
                                                        dataService._dialogService)
        {
            _folderDataService = dataService._folderDataService;
            _memeDataService = dataService._memeDataService;
            _dataService = dataService;

            var temp = Task.Run(() => _folderDataService.GetAll());
            Model = temp.Result.FirstOrDefault(f => f.Title == "root");

            Children = new ObservableCollection<FolderVM>();
            FolderObjects = new ImageAsyncCollection<ImageAsync<FolderObject>>();
        }

        public FolderVM(Folder model,
                        DataService dataService) : this(dataService._navigationService,
                                                        dataService._dialogService)
        {
            _folderDataService = dataService._folderDataService;
            _memeDataService = dataService._memeDataService;
            _dataService = dataService;

            Model = model;

            Children = new ObservableCollection<FolderVM>();
            FolderObjects = new ImageAsyncCollection<ImageAsync<FolderObject>>();
        }

        private void DownloadData()
        {
            System.Diagnostics.Stopwatch myStopwatch = new System.Diagnostics.Stopwatch();
            myStopwatch.Start(); //запуск
         
            Model.PropertyChanged += Model_PropertyChanged;

            foreach (var item in Model.Folders)
            {
                Children.Add(new FolderVM(item,
                                          _dataService));
                item.PropertyChanged += Model_PropertyChanged;
                FolderObjects.Add(new ImageAsync<FolderObject>(item));
            }

            foreach (var item in Model.Memes)
            {
                item.PropertyChanged += Model_PropertyChanged;
                FolderObjects.Add(new ImageAsync<FolderObject>(item.ImagePath, item));
            }
            myStopwatch.Stop(); //остановить
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is Meme)
            {
                var memeObj = (Meme)sender;
                _memeDataService.Update(memeObj.Id, memeObj);
            }
            else if (sender is Folder)
            {
                var folderObj = (Folder)sender;
                _folderDataService.Update(folderObj.Id, folderObj);
            }
        }

        public void OnNavigatedTo(object arg)
        {
           
        }

        public ICommand PageLoadedCommand { get; }

        private void PageLoadedExecute(object parameter)
        {
           

            DownloadData();
        }

        #endregion
    }
}
