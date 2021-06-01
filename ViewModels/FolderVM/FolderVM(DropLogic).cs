using GongSolutions.Wpf.DragDrop;
using MemeFolder.Domain.Models;
using MemeFolder.Extentions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
                            ImagePath = file,
                            ImageData = MemeExtentions.ConvertImageToByteArray(file)
                        };

                        var createdMeme = await _memeDataService.Create(meme);
                        if (createdMeme != null)
                        {
                            Model.Memes.Add(createdMeme);

                            createdMeme.Image = MemeExtentions.ConvertByteArrayToImage(createdMeme.ImageData);//.ToImageSource();
                            createdMeme.Image.Freeze();

                            FolderObjects.Add(createdMeme);
                            createdMeme.PropertyChanged += Model_PropertyChanged;
                        }
                    }
                    else if (Directory.Exists(file))
                    {

                        Folder folder = new Folder();
                        folder.ParentFolder = this.Model;
                        folder.ImageFolderPath = file;
                        folder.Title = new DirectoryInfo(file).Name;

                        var createdFolder = await _folderDataService.Create(folder);
                        if (createdFolder != null)
                        {
                            var updatedFolder = await GetAllFiles(createdFolder);
                            Children.Add(new FolderVM(updatedFolder, _dataService));

                            Model.Folders.Add(updatedFolder);
                            FolderObjects.Add(updatedFolder);
                            updatedFolder.PropertyChanged += Model_PropertyChanged;
                        }
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
                        ImagePath = path,
                        ImageData = MemeExtentions.ConvertImageToByteArray(path)
                    };

                    var createdMeme = await _memeDataService.Create(meme);
                    if (createdMeme != null)
                    {
                        rootFolder.Memes.Add(createdMeme);
                    }
                }

            if (directories.Length != 0)
                foreach (string path in directories)
                {
                    Folder f = new Folder();
                    f.ImageFolderPath = path;
                    f.ParentFolder = rootFolder;
                    f.Title = new DirectoryInfo(path).Name;

                    var createdFolder = await _folderDataService.Create(f);
                    if (createdFolder != null)
                    {
                        var updatedFolder = await GetAllFiles(createdFolder);
                        rootFolder.Folders.Add(updatedFolder);
                    }

                }
            return rootFolder;
        }

        #endregion

    }
}
