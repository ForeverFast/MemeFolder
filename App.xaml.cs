﻿using MemeFolder.Domain.Models;
using MemeFolder.EntityFramework;
using MemeFolder.Navigation;
using MemeFolder.Pages;
using MemeFolder.Services;
using MemeFolder.ViewModels;
using MemeFolder.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
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

            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("sxo-settings.json", optional: false, reloadOnChange: true);
               
            //Configuration = builder.Build();


            try
            {
                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);

                ServiceProvider = serviceCollection.BuildServiceProvider();

                #region NavManager

                var initDataService = ServiceProvider.GetRequiredService<IInitDataBaseService>();
                if (!initDataService.Init().Result)
                    throw new Exception("Ошибка создания/получения root-папки.");

                var mainWindow = ServiceProvider.GetRequiredService<MainWindowV>();
                var navigationService = ServiceProvider.GetRequiredService<INavigationService>();

                navigationService.Register<FolderPage>("root", ServiceProvider.GetRequiredService<FolderVM>());
                navigationService.Register<SettingsPage>("settings", ServiceProvider.GetRequiredService<SettingsPageVM>());
                navigationService.Register<SearchPage>("searchPage", ServiceProvider.GetRequiredService<SearchPageVM>());
                navigationService.Navigate("root", NavigationType.Root);

                #endregion

                mainWindow.Show();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"{ex.Message}\r\n{ex.StackTrace}");
            }
            
        }


        private void ConfigureServices(IServiceCollection services)
        {
            

            services.AddSingleton<IMemeDataService, MemeDataService>();
            services.AddSingleton<IFolderDataService, FolderDataService>();
            services.AddSingleton<IMemeTagDataService, MemeTagDataService>();
            services.AddSingleton(typeof(DataStorage));
            services.AddSingleton(typeof(DataService));
            services.AddSingleton(typeof(MemeFolderDbContextFactory));

            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IInitDataBaseService, InitDataBaseService>();

            services.AddSingleton(service => Configuration);
            services.AddSingleton(typeof(ClientConfigService));

            services.AddSingleton(typeof(Folder), (s) => s.GetRequiredService<DataStorage>().RootFolder);
            services.AddSingleton(typeof(FolderVM));
            services.AddSingleton(typeof(SearchPageVM));
            services.AddSingleton(typeof(SettingsPageVM));
            services.AddSingleton(typeof(MainWindowVM));
            services.AddSingleton(typeof(MainWindowV));

            services.AddSingleton(typeof(ContentControl), (s) => s.GetRequiredService<MainWindowV>().FrameContent);

        }
    }
}
