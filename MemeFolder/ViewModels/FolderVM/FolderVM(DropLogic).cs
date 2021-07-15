using GongSolutions.Wpf.DragDrop;
using MemeFolder.Domain.Models;
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
                foreach (string pathToObject in files)
                {
                    if (File.Exists(pathToObject))
                    {
                        Meme meme = new Meme()
                        {
                            Title = Path.GetFileName(pathToObject),
                            Folder = this.Model,
                            ImagePath = pathToObject
                        };

                        await _dataStorage.AddMeme(meme, this.Model).ConfigureAwait(false);
                    }
                    else if (Directory.Exists(pathToObject))
                    {
                        Folder folder = new Folder()
                        {
                            Title = Path.GetFileNameWithoutExtension(pathToObject),
                            ParentFolder = this.Model,
                            FolderPath = pathToObject
                        };

                        Folder dbCreatedFolder = await _dataStorage.AddFolder(folder, this.Model);
                        await GetAllFiles(Model.Folders.FirstOrDefault(f => f.Id == dbCreatedFolder.Id), pathToObject);
                    }

                }
            }
        }

        public async Task<Folder> GetAllFiles(Folder rootFolder, string oldFolderPath)
        {
            string[] directories = Directory.GetDirectories(oldFolderPath);

            var files = Directory.EnumerateFiles(oldFolderPath, "*.*", SearchOption.TopDirectoryOnly)
                    .Where(s => s.EndsWith(".png") || s.EndsWith(".jpg") || s.EndsWith(".jpeg")).ToArray();

            if (files.Length != 0)
                foreach (var path in files)
                {
                    Meme meme = new Meme()
                    {
                        Title = Path.GetFileNameWithoutExtension(path),
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
                        Title = Path.GetFileNameWithoutExtension(path),
                        ParentFolder = rootFolder,
                        FolderPath = path
                    };

                    Folder dbCreatedFolder = await _dataStorage.AddFolder(folder, rootFolder);
                    await GetAllFiles(rootFolder.Folders.FirstOrDefault(f => f.Id == dbCreatedFolder.Id), path);

                }
            return rootFolder;
        }

        #endregion

    }
}
