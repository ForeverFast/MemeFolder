using Egor92.MvvmNavigation.Abstractions;
using MemeFolder.Domain.Models;
using MemeFolder.Mvvm.Commands;
using MemeFolder.Pc.Mvvm;
using MemeFolder.Pc.Mvvm.ViewModels.Abstractions;
using MemeFolder.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MemeFolder.MVVM.ViewModels
{
    public class MemeVM : BasePageViewModel
    {
        #region Поля
        private Meme _model;
        private IFolderDataService _dataService;
        private IDialogService _dialogService;
        #endregion

        public Meme Model { get => _model; set => SetProperty(ref _model, value); }

        #region Команды - Навигациия

        //private void NavigationToFolderExecute(object parameter)
        //    => _navigationManager.Navigate<FolderPage>((parameter as Folder).Id.ToString(),
        //                                                Model, null);

        #endregion

        #region Команды - Мемы

        public ICommand SetImage
        {
            get => new RelayCommand((o) => {
                Model.ImagePath = _dialogService.FileBrowserDialog("*.jpg;*.png");
            });
        }

        private async Task AddMemeExecute(object parameter)
        {
            //IsDialogOpen = false;
            //var parameters = (object[])parameter;
            //Meme meme = new Meme()
            //{
            //    Folder = Model,
            //    Title = parameters[0].ToString(),
            //    Description = parameters[1].ToString(),
            //    ImagePath = parameters[2].ToString()
            //};
            //// Model.Memes.Add(meme);
            //await _dataService.Update(Model.Id, Model);
            //FolderObjects.Add(meme);
        }

        #endregion

        #region Конструкторы
        public MemeVM(Meme memeModel,
                      INavigationManager navigationManager,
                      IDialogService dialogService) : base(navigationManager)
        {
            _dialogService = dialogService;

            Model = memeModel;


            //AddMeme = new AsyncRelayCommand(AddMemeExecute);

            //NavigationToFoldertCommand = new RelayCommand(NavigationToFolderExecute, null);

        }

        #endregion
    }
}
