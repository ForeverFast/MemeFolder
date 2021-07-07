using MemeFolder.Domain.Models;
using MemeFolder.ViewModels.Abstractions;

namespace MemeFolder.ViewModels.DialogVM
{
    public class DialogTagVM : BaseDialogViewModel
    {
        #region Поля
        private MemeTag _model;
        private string _dialogTitle;
        #endregion

        public MemeTag Model { get => _model; set => SetProperty(ref _model, value); }
        public string DialogTitle { get => _dialogTitle; set => SetProperty(ref _dialogTitle, value); }


        #region Конструкторы
        public DialogTagVM(MemeTag model,
                           string dialogTitle) : base()
        {
            Model = model;

            DialogTitle = dialogTitle;
        }

        #endregion
    }
}

