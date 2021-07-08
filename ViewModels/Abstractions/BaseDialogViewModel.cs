namespace MemeFolder.ViewModels.Abstractions
{
    public class BaseDialogViewModel : BaseViewModel
    {
        private string _dialogTitle;

        public string DialogTitle { get => _dialogTitle; set => SetProperty(ref _dialogTitle, value); }
    }
}
