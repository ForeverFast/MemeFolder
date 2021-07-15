using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemeFolder.Navigation
{
    public static class NavigationServiceExtentions
    {
        public static void Navigate(this INavigationService navigationService, [NotNull] string navigationKey, NavigationType navigateType)
        {
            navigationService.Navigate(navigationKey, navigateType, null);
        }

        //public static void Register<TView>([NotNull] this NavigationService navigationManager,
        //                                   [NotNull] string navigationKey,
        //                                   [NotNull] Func<object> getViewModel)
        //    where TView : class, new()
        //{
        //    Func<object> getView = Activator.CreateInstance<TView>;
        //    navigationManager.Register(navigationKey, getViewModel, getView);
        //}
    }
}
