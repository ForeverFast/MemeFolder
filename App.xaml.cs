using Egor92.MvvmNavigation;
using Egor92.MvvmNavigation.Abstractions;
using MemeFolder.EntityFramework;
using MemeFolder.MVVM.ViewModels;
using MemeFolder.MVVM.Views;
using MemeFolder.MVVM.Views.Pages;
using MemeFolder.Services;
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

         
            //var fds = ServiceProvider.GetRequiredService<IFolderDataService>();
            //var t = Task.Run(() => fds.GetFoldersByTitle("root")).Result;

            

            #region NavManager
            var navigationManager = ServiceProvider.GetRequiredService<INavigationManager>();  
            //var RootFolder = ServiceProvider.GetRequiredService<IFolderDataService>();
            //navigationManager.Register<FolderPage>("root", ServiceProvider.GetRequiredService<FolderVM>());

            var mainWindow = ServiceProvider.GetRequiredService<MainWindowV>();
            navigationManager.FrameControl = mainWindow.FrameContent;
            navigationManager.Register<FolderPage>("root", ServiceProvider.GetRequiredService<FolderVM>());
            navigationManager.Navigate("root", NavigateType.Root, null);

            #endregion

            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<INavigationManager, NavigationManager>();

            services.AddSingleton(typeof(MemeFolderDbContextFactory));
            services.AddSingleton<IFolderDataService, FolderDataService>();
            services.AddSingleton<IMemeDataService, MemeDataService>();
            services.AddSingleton<IDialogService, DialogService>();
            
            services.AddSingleton(typeof(FolderVM));

            services.AddSingleton(typeof(MainWindowVM));
            services.AddSingleton(typeof(MainWindowV));
        }
    }
}
