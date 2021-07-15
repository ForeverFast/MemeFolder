using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace MemeFolder.Navigation
{
    public interface INavigationService
    {
        void Navigate(string navigationKey, NavigationType navigateType, object arg = null);
        void Navigate<TView>(object viewModel, object arg) where TView : class, new();
        void Navigate<TView>(string navigationKey, object viewModel, object arg) where TView : class, new();
        bool CanNavigate(string navigationKey);

        void Register<TView>([NotNull] string navigationKey, object viewModel) where TView : class, new();


        bool CanGoBack();
        void GoBack();
        bool CanGoForward();
        void GoForward();

    }
}
