using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using MemeFolder.ViewModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MemeFolder.ViewModels.DialogVM
{
    public class DialogMemeVM : BaseDialogViewModel
    {
        #region Поля
        private Meme _model;
        private readonly IDialogService _dialogService;
        private string _dialogTitle;
        #endregion

        public Meme Model { get => _model; set => SetProperty(ref _model, value); }
        public string DialogTitle { get => _dialogTitle; set => SetProperty(ref _dialogTitle, value); }

        #region Команды - Мемы

        public ICommand SetImage
        {
            get => new RelayCommand((o) => {
                Model.ImagePath = _dialogService.FileBrowserDialog("*.jpg;*.png");
            });
        }

        #endregion


        #region Конструкторы
        public DialogMemeVM(Meme model,
                            IDialogService dialogService,
                            string dialogTitle) : base()
        {
            Model = model;

            _dialogService = dialogService;

            DialogTitle = dialogTitle;
        }

        #endregion
    }
}
