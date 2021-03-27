using Egor92.MvvmNavigation;
using Egor92.MvvmNavigation.Abstractions;
using MemeFolder.EntityFramework;
using MemeFolder.MVVM.ViewModels;
using MemeFolder.MVVM.Views;
using MemeFolder.MVVM.Views.Pages;
using MemeFolder.Navigation;
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
using System.Windows.Controls;

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

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("conf.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();



            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            #region NavManager
           
            var mainWindow = ServiceProvider.GetRequiredService<MainWindowV>();
            var navigationService = ServiceProvider.GetRequiredService<INavigationService>();

            navigationService.Register<FolderPage>("root", ServiceProvider.GetRequiredService<FolderVM>());
            navigationService.Register<SettingsPage>("settings", ServiceProvider.GetRequiredService<SettingsPageVM>());
            navigationService.Navigate("root", NavigationType.Root);

            #endregion

            mainWindow.Show();
        }


        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<INavigationService, NavigationService>();
            
            services.AddSingleton(typeof(MemeFolderDbContextFactory));
            services.AddSingleton(typeof(DataService));
            services.AddSingleton<IFolderDataService, FolderDataService>();
            services.AddSingleton<IMemeDataService, MemeDataService>();
            services.AddSingleton<IDialogService, DialogService>();

            services.AddSingleton(service => Configuration);
            services.AddSingleton(typeof(ClientConfigService));

            services.AddSingleton(typeof(FolderVM));
            services.AddSingleton(typeof(SettingsPageVM));
           
            services.AddSingleton(typeof(ContentControl), (s) => s.GetRequiredService<MainWindowV>().FrameContent);

            services.AddSingleton(typeof(MainWindowVM));
            services.AddSingleton(typeof(MainWindowV));
        }
    }
}
