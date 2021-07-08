using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.Commands;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using MemeFolder.ViewModels.Abstractions;
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
        #endregion

        public Meme Model { get => _model; set => SetProperty(ref _model, value); }

        #region Команды - Мемы

        public ICommand SetImage
        {
            get => new RelayCommand((o) => {
                Model.ImagePath = _dialogService.FileBrowserDialog("*.jpg;*.png");
            });
        }

        public ICommand OpenAddMemeTagDialogCommand { get; }

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

            OpenAddMemeTagDialogCommand = new OpenAddMemeTagDialogCommand(dataService);
        }

        #endregion
    }
}
