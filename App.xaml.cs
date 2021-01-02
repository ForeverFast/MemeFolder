using Egor92.MvvmNavigation;
using Egor92.MvvmNavigation.Abstractions;
using MemeFolder.MVVM.ViewModels;
using MemeFolder.MVVM.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MemeFolder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        public App()
        {

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //CurrentUser = new User() { Login = "ForeverFast" };

            //var builder = new ConfigurationBuilder()
            // .SetBasePath(Directory.GetCurrentDirectory());
            ////.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            //Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindowV>();

            #region
            var navigationManager = ServiceProvider.GetRequiredService<INavigationManager>();
            navigationManager.FrameControl = mainWindow.FrameContent;
            //var q = ServiceProvider.GetRequiredService<IServiceProvider>();
            //navigationManager.Register<HomePage>("home", new HomePageVM(navigationManager));
            //navigationManager.Register<ReviewPage>("review", new HomePageVM(navigationManager));
            //navigationManager.Register<ProfilePage>("profile", new HomePageVM(navigationManager));

            //navigationManager.Navigate("home", NavigateType.Root, null);
            #endregion

            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<INavigationManager, NavigationManager>();
            services.AddSingleton(typeof(MainWindowVM));
            services.AddSingleton(typeof(MainWindowV));


        }
    }
}
