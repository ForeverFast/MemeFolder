using MemeFolder.Domain.Models;
using MemeFolder.Services;
using MemeFolder.ViewModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemeFolder.ViewModels.DialogVM
{
    public class DialogFolderVM : BaseDialogViewModel
    {
        #region Поля
        private Folder _model;
        private readonly IDialogService _dialogService;
        private string _dialogTitle;
        #endregion

        public Folder Model { get => _model; set => SetProperty(ref _model, value); }
        public string DialogTitle { get => _dialogTitle; set => SetProperty(ref _dialogTitle, value); }


        #region Команды - Мемы



        #endregion


        #region Конструкторы
        public DialogFolderVM(Folder model,
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
