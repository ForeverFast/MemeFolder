using Egor92.MvvmNavigation.Abstractions;
using System.Windows.Input;

namespace MemeFolder.Pc.Mvvm.ViewModels.Abstractions
{
    public class BasePageViewModel : BaseViewModel
    {
        #region Конструкторы

        public BasePageViewModel(INavigationManager navigationManager) : base(navigationManager) 
        {
          
        }

        #endregion
    }
}
