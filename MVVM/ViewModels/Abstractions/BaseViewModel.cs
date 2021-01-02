using Egor92.MvvmNavigation.Abstractions;
using MemeFolder.Abstractions;
//using MemeFolder.Services;

namespace MemeFolder.Pc.Mvvm.ViewModels.Abstractions
{
    public abstract class BaseViewModel : OnPropertyChangedClass
    {
        public INavigationManager _navigationManager;
   
        #region Конструкторы и методы инициализации

        public abstract void InitiailizeCommands();

        public BaseViewModel(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        #endregion
    }
}
