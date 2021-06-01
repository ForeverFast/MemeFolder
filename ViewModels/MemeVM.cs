using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Navigation;
using MemeFolder.Services;
using MemeFolder.ViewModels.Abstractions;
using System.Windows.Input;

namespace MemeFolder.ViewModels
{
    public class MemeVM : BasePageViewModel
    {
        #region Поля
        private Meme _model;
        private IDialogService _dialogService;
        #endregion

        public Meme Model { get => _model; set => SetProperty(ref _model, value); }

        #region Команды - Мемы

        public ICommand SetImage
        {
            get => new RelayCommand((o) => {
                Model.ImagePath = _dialogService.FileBrowserDialog("*.jpg;*.png");
            });
        }

        

        #endregion

        #region Конструкторы
        public MemeVM(Meme memeModel,
                      INavigationService navigationService,
                      IDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;

            Model = memeModel;
        }

        #endregion
    }
}
