using MemeFolder.Domain.Models;
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

        public Guid rootGuid { get; private set; } = Guid.Parse("00000000-0000-0000-0000-000000000001");

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


                #region DataBase

                IFolderDataService folderDataService = ServiceProvider.GetRequiredService<IFolderDataService>();

                if (folderDataService.Get(rootGuid).Result == null)
                {
                    ClientConfigService clientConfigService = ServiceProvider.GetRequiredService<ClientConfigService>();
                    Folder rootFolder = new Folder()
                    {
                        Id = rootGuid,
                        Title = "root",
                        FolderPath = clientConfigService.FilesPath
                    };
                    folderDataService.CreateRootFolder(rootFolder);
                }

                #endregion


                #region NavManager

                MainWindow mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                INavigationService navigationService = ServiceProvider.GetRequiredService<INavigationService>();

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
            services.AddSingleton(typeof(Guid), (s) => rootGuid);
            
            services.AddSingleton<IMemeDataService, MemeDataService>();
            services.AddSingleton<IFolderDataService, FolderDataService>();
            services.AddSingleton<IMemeTagDataService, MemeTagDataService>();
            services.AddSingleton<IMemeTagNodeDataService, MemeTagNodeDataService>();
            services.AddSingleton(typeof(DataStorage));
            services.AddSingleton(typeof(ServiceCollectionClass));
            services.AddSingleton(typeof(MemeFolderDbContextFactory));

            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<INavigationService, NavigationService>();
            
            services.AddSingleton(service => Configuration);
            services.AddSingleton(typeof(ClientConfigService));

            services.AddSingleton(typeof(Folder), (s) => s.GetRequiredService<DataStorage>().RootFolder);
            services.AddSingleton(typeof(FolderVM));
            services.AddSingleton(typeof(SearchPageVM));
            services.AddSingleton(typeof(SettingsPageVM));
            services.AddSingleton(typeof(MainWindowVM));
            services.AddSingleton(typeof(MainWindow));

            services.AddSingleton(typeof(ContentControl), (s) => s.GetRequiredService<MainWindow>().FrameContent);

        }
    }
}
