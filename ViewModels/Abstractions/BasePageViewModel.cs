using MemeFolder.Navigation;

namespace MemeFolder.ViewModels.Abstractions
{
    public class BasePageViewModel : BaseNavigationViewModel
    {
        #region Конструкторы

        public BasePageViewModel(INavigationService navigationService) : base(navigationService) 
        {
          
        }

        #endregion
    }
}
