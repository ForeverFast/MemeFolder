using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.Commands;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using MemeFolder.ViewModels.Abstractions;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace MemeFolder.ViewModels.DialogVM
{
    public class DialogMemeVM : BaseDialogViewModel
    {
        #region Поля
        private Meme _model;
        private DataService _dataService;
        private DataStorage _dataStorage;
        private readonly IDialogService _dialogService;

        private ObservableCollection<MemeTag> _memeTags;
        #endregion



        public Meme Model { get => _model; set => SetProperty(ref _model, value); }

        public ObservableCollection<MemeTag> MemeTags { get => _memeTags; private set => SetProperty(ref _memeTags, value); }

        #region Команды - Мемы

        public ICommand SetImage
        {
            get => new RelayCommand((o) => {
                Model.ImagePath = _dialogService.FileBrowserDialog("*.jpg;*.png");
            });
        }

        public ICommand OpenAddMemeTagDialogCommand { get; }

        public ICommand MemeTagCheckFlagChangedCommand { get; }

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


        #region Конструкторы
        public DialogMemeVM(Meme model,
                            DataService dataService,
                            string dialogTitle) : base()
        {
            Model = model;

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

            OpenAddMemeTagDialogCommand = new OpenAddMemeTagDialogCommand(dataService);
            MemeTagCheckFlagChangedCommand = new RelayCommand(MemeTagCheckFlagChangedExecute);
        }

        #endregion
    }
}
