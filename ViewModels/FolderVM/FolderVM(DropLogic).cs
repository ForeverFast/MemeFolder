﻿using GongSolutions.Wpf.DragDrop;
using MemeFolder.Domain.Models;
using MemeFolder.Extentions;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MemeFolder.ViewModels
{
    public partial class FolderVM : IDropTarget
    {

        #region Логика

        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;

            var dataObject = dropInfo.Data as IDataObject;

            dropInfo.Effects = dataObject != null && dataObject.GetDataPresent(DataFormats.FileDrop)
                ? DragDropEffects.Copy
                : DragDropEffects.Move;
        }

        public async void Drop(IDropInfo dropInfo)
        {
            var dataObject = dropInfo.Data as DataObject;
            if (dataObject != null && dataObject.ContainsFileDropList())
            {
                var files = dataObject.GetFileDropList();
                foreach (var file in files)
                {
                    if (File.Exists(file))
                    {
                        Meme meme = new Meme()
                        {
                            Title = Path.GetFileName(file),
                            Folder = this.Model,
                            ImagePath = file
                        };

                        await _dataStorage.AddMeme(meme, this.Model);
                    }
                    else if (Directory.Exists(file))
                    {
                        Folder folder = new Folder()
                        {
                            Title = new DirectoryInfo(file).Name,
                            ParentFolder = this.Model,
                            ImageFolderPath = file
                        };

                        Folder dbCreatedFolder = await _dataStorage.AddFolder(folder, this.Model);
                        await GetAllFiles(Model.Folders.FirstOrDefault(f => f.Id == dbCreatedFolder.Id));
                    }

                }
            }
        }

        public async Task<Folder> GetAllFiles(Folder rootFolder)
        {
            string[] directories = Directory.GetDirectories(rootFolder.ImageFolderPath);

            var files = Directory.EnumerateFiles(rootFolder.ImageFolderPath, "*.*", SearchOption.TopDirectoryOnly)
                    .Where(s => s.EndsWith(".png") || s.EndsWith(".jpg")).ToArray();

            if (files.Length != 0)
                foreach (var path in files)
                {
                    Meme meme = new Meme()
                    {
                        Title = Path.GetFileName(path),
                        Folder = rootFolder,
                        ImagePath = path
                    };

                    await _dataStorage.AddMeme(meme, rootFolder);
                }

            if (directories.Length != 0)
                foreach (string path in directories)
                {
                    Folder folder = new Folder()
                    {
                        Title = new DirectoryInfo(path).Name,
                        ParentFolder = rootFolder,
                        ImageFolderPath = path
                    };

                    Folder dbCreatedFolder = await _dataStorage.AddFolder(folder, rootFolder);
                    await GetAllFiles(rootFolder.Folders.FirstOrDefault(f => f.Id == dbCreatedFolder.Id));

                }
            return rootFolder;
        }

        #endregion

    }
}
