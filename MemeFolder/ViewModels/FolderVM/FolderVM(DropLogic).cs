using GongSolutions.Wpf.DragDrop;
using MemeFolder.Domain.Models;
using System;
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
                try
                {
                    var files = dataObject.GetFileDropList();
                    foreach (string pathToObject in files)
                    {
                        if (File.Exists(pathToObject))
                        {
                            await this.SaveMeme(pathToObject, this.Model);
                        }
                        else if (Directory.Exists(pathToObject))
                        {
                            await this.SaveFolder(pathToObject, this.Model);
                        }

                    }
                }
                catch (Exception ex)
                {
                  
                }
                
            }
        }

        private async Task SaveMeme(string path, Folder parentFolder)
        {
            Meme meme = new Meme()
            {
                Title = Path.GetFileName(path),
                Folder = parentFolder,
                ImagePath = path
            };

            await _dataStorage.AddMeme(meme, parentFolder);
        }

        private async Task SaveFolder(string path, Folder parentFolder)
        {
            Folder folder = new Folder()
            {
                Title = Path.GetFileNameWithoutExtension(path),
                ParentFolder = this.Model,
                FolderPath = path
            };

            Folder dbCreatedFolder = await _dataStorage.AddFolder(folder, parentFolder);
            await GetAllFiles(Model.Folders.FirstOrDefault(f => f.Id == dbCreatedFolder.Id), path);
        }


        private async Task GetAllFiles(Folder rootFolder, string oldFolderPath)
        {
            string[] directories = Directory.GetDirectories(oldFolderPath);

            string[] files = Directory.EnumerateFiles(oldFolderPath, "*.*", SearchOption.TopDirectoryOnly)
                    .Where(s => s.EndsWith(".png") || s.EndsWith(".jpg") || s.EndsWith(".jpeg")).ToArray();

            foreach (string path in files)
            {
                await this.SaveMeme(path, rootFolder);
            }

            foreach (string path in directories)
            {
                await this.SaveFolder(path, rootFolder);
            } 
        }

        #endregion

    }
}
