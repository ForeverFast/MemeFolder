using MemeFolder.Domain.Models;
using MemeFolder.ViewModels.Abstractions;

namespace MemeFolder.ViewModels.DialogVM
{
    public class DialogTagVM : BaseDialogViewModel
    {
        #region Поля
        private MemeTag _model;
        #endregion

        public MemeTag Model { get => _model; set => SetProperty(ref _model, value); }

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

