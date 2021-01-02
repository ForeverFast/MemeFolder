using Egor92.MvvmNavigation.Abstractions;
using MemeFolder.Domain.Models;
using MemeFolder.Pc.Mvvm.ViewModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MemeFolder.MVVM.ViewModels
{
    public class FolderVM : BasePageViewModel
    {
        #region Поля
        private Folder _model;
        #endregion

        public Folder Model { get => _model; set => SetProperty(ref _model, value); }
        public ObservableCollection<FolderVM> Children { get; private set; }

        public FolderVM(Folder model,
                        INavigationManager navigationManager) : base(navigationManager)
        {

            Model = model;
        }
    }
}
