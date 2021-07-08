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
        #endregion

        public Folder Model { get => _model; set => SetProperty(ref _model, value); }
       


        #region Команды - Мемы



        #endregion


        #region Конструкторы
        public DialogFolderVM(Folder model,
                             string dialogTitle) : base()
        {
            Model = model;
            DialogTitle = dialogTitle;
        }

        #endregion
    }
}
