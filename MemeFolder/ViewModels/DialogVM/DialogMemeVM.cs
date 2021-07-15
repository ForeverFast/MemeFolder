using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.Commands;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using MemeFolder.ViewModels.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace MemeFolder.ViewModels.DialogVM
{
    public class DialogMemeVM : BaseDialogViewModel, IDisposable
    {
        #region Поля
        private Meme _model;
        private Folder _parentFolder;
        private ServiceCollectionClass _dataService;
        private DataStorage _dataStorage;
        private IDialogService _dialogService;

        private ObservableCollection<MemeTag> _memeTags;
        #endregion



        public Meme Model { get => _model; set => SetProperty(ref _model, value); }

        public ObservableCollection<MemeTag> MemeTags { get => _memeTags; private set => SetProperty(ref _memeTags, value); }

        public bool CanSave
        {
            get {

                if (Model == null)
                    return false;

                if (string.IsNullOrEmpty(Model.Title))
                    return false;

                foreach (Meme meme in _parentFolder.Memes)
                {
                    if (meme.Id != Model.Id && meme.Title == Model.Title)
                        return false;
                }

                if (string.IsNullOrEmpty(Model.ImagePath))
                    return false;

                return true;
            }
        }


        #region Команды - Мемы

        public ICommand SetImageCommand { get; private set; }

        public ICommand TitleChangedCommand { get; private set; }

        public ICommand OpenAddMemeTagDialogCommand { get; private set; }

        public ICommand MemeTagCheckFlagChangedCommand { get; private set; }

        private void SetImageExecute(object parameter)
        {
            Model.ImagePath = _dialogService.FileBrowserDialog("*.jpg;*.png");
            OnPropertyChanged(nameof(CanSave));
        }

        private void TitleChangedExecute(object parameter)
        {
            OnPropertyChanged(nameof(CanSave));
        }

        private void MemeTagCheckFlagChangedExecute(object parameter)
        {
            MemeTag memeTag = (MemeTag)parameter;

            if (memeTag.CheckFlag)
            {
                MemeTagNode memeTagNode = new MemeTagNode()
                {
                    Meme = Model,
                    MemeTag = memeTag
                };
                Model.Tags.Add(memeTagNode);
            }
            else
            {
                MemeTagNode rMemeTagNode = Model.Tags.FirstOrDefault(mtn => mtn.MemeTag.Id == memeTag.Id);
                Model.Tags.Remove(rMemeTagNode);
            }
           
        }

        #endregion

        public void Dispose()
        {
            _dataService = null;
            _dataStorage = null;
            _dialogService = null;

            Model = null;
            MemeTags = null;

            SetImageCommand = null;
            TitleChangedCommand = null;
            OpenAddMemeTagDialogCommand = null;
            MemeTagCheckFlagChangedCommand = null;
        }

        #region Конструкторы

        public DialogMemeVM(Meme model,
            Folder parentFolder,
            ServiceCollectionClass dataService,
            string dialogTitle) : base()
        {
            Model = model;
            _parentFolder = parentFolder;

            _dataService = dataService;
            _dataStorage = dataService._dataStorage;
            _dialogService = dataService._dialogService;

            DialogTitle = dialogTitle;

            MemeTags = dataService._dataStorage.MemeTags;
            MemeTags.ToList().ForEach(mt =>
            {
                MemeTag memeTag = Model.Tags.FirstOrDefault(mtn => mtn.MemeTag.Id == mt.Id)?.MemeTag;
                if (memeTag != null)
                    mt.CheckFlag = true;
                else
                    mt.CheckFlag = false;
            });

            SetImageCommand = new RelayCommand(SetImageExecute);
            TitleChangedCommand = new RelayCommand(TitleChangedExecute);

            OpenAddMemeTagDialogCommand = new OpenAddMemeTagDialogCommand(dataService);
            MemeTagCheckFlagChangedCommand = new RelayCommand(MemeTagCheckFlagChangedExecute);
        }

        #endregion
    }
}
