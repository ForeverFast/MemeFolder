using MemeFolder.Navigation;

namespace MemeFolder.Pc.Mvvm.ViewModels.Abstractions
{
    public class BasePageViewModel : BaseViewModel
    {
        #region Конструкторы

        public BasePageViewModel(INavigationService navigationService) : base(navigationService) 
        {
          
        }

        #endregion
    }
}
