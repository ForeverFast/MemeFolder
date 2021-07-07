﻿using MemeFolder.Abstractions;
using MemeFolder.Domain.Models;
using MemeFolder.Extentions;
using MemeFolder.Mvvm.CommandsBase;
using MemeFolder.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MemeFolder.Mvvm.Commands.Memes
{
    /// <summary> Команда добавления Meme </summary>
    public class AddMemeCommand : AsyncCommandBase
    {
        private readonly IObjectWorker _folderVM;
        private readonly IDialogService _dialogService;
        private readonly IMemeDataService _memeDataService;

        protected override async Task ExecuteAsync(object parameter)
        {
            var path = _dialogService.FileBrowserDialog();
            Meme meme = new Meme()
            {
                Title = "Новый мемчик",
                ImagePath = path
            };

            Meme createdEntity = await _memeDataService.Create(meme);
            if (createdEntity != null)
            {
                createdEntity.Image = MemeExtentions.ConvertByteArrayToImage(createdEntity.ImageData);
                createdEntity.ImageData = null;

                ObservableCollection<Meme> memes = (ObservableCollection<Meme>)_folderVM.GetWorkerCollection(ObjectType.Meme);
                memes.Add(meme);
            }
        }

        public AddMemeCommand(IObjectWorker folderVM,
            DataService dataService,
            Action<Exception> onException = null) : base(onException)
        {
            _folderVM = folderVM;
            _memeDataService = dataService._memeDataService;
             _dialogService = dataService._dialogService;
        }
    }
}
