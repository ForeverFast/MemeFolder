using Egor92.MvvmNavigation.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemeFolder.Pc.Mvvm.ViewModels.Abstractions
{
    public class BasePageViewModel : BaseViewModel
    {
        public BasePageViewModel(INavigationManager navigationManager) : base(navigationManager) { }

        public override void InitiailizeCommands()
        {
            throw new NotImplementedException();
        }
    }
}
