using MemeFolder.Domain.Models;
using MemeFolder.Extentions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MemeFolder.Services
{
    public delegate void SearchServiceEvent(IEnumerable<Meme> folderObjects);
    public delegate void FolderEvent(Folder folder, string message);
    public delegate void MemeEvent(Meme meme, string message);
    public delegate void MemeTagEvent(MemeTag memeTag, string message);

    public class DataStorage
    {
        private readonly IFolderDataService _folderDataService;
        private readonly IMemeDataService _memeDataService;
        private readonly IMemeTagDataService _memeTagDataService;
        private readonly IMemeTagNodeDataService _memeTagNodeDataService;

        private readonly ClientConfigService _clientConfigService;
        

        public ObservableCollection<Folder> Folders { get; }
        public ObservableCollection<Meme> Memes { get; }
        public ObservableCollection<MemeTag> MemeTags { get; }

        public Folder RootFolder { get; }

        public event SearchServiceEvent NewRequest;

        public event FolderEvent OnAddFolder;
        public event FolderEvent OnEditFolder;
        public event FolderEvent OnRemoveFolder;

        public event MemeEvent OnAddMeme;
        public event MemeEvent OnEditMeme;
        public event MemeEvent OnRemoveMeme;

        public event MemeTagEvent OnAddMemeTag;
        public event MemeTagEvent OnEditMemeTag;
        public event MemeTagEvent OnRemoveMemeTag;


        public void NavigateByRequest(Func<Meme,bool> func)
        {
            IEnumerable<Meme> memes = Memes.Where(func);
            NewRequest?.Invoke(memes);
        }

        public void NavigateByMemeTag(Guid guid)
        {
            IEnumerable<Meme> memes = Memes.Where(meme => meme.Tags.FirstOrDefault(m => m.MemeTag.Id == guid) != null);
            NewRequest?.Invoke(memes);
        }

        private string GetFolderAnotherName(string rootPath, string title)
        {
            string newFolderPath = string.Empty;
            int num = 1;
            while (true)
            {
                string tempTitle = $"{title} ({num++})";

                newFolderPath = @$"{rootPath}\{tempTitle}";
                if (!Directory.Exists(newFolderPath))
                {
                    Directory.CreateDirectory(newFolderPath);
                    break;
                }
            }

            return newFolderPath;
        }

        private string GetMemeAnotherName(string rootPath, string title, string imagePath)
        {
            string newMemePath = string.Empty;
            int num = 1;
            while (true)
            {
                newMemePath = @$"{rootPath}\{title} ({num++}){Path.GetExtension(imagePath)}";
                if (!File.Exists(newMemePath))
                {
                    File.Copy(imagePath, newMemePath);
                    break;
                }
            }

            return newMemePath;
        }

        public async Task<Folder> AddFolder(Folder folder, Folder parentFolder)
        {
            string newFolderPath = string.Empty;
            if (string.IsNullOrEmpty(folder.Title))
            {
                newFolderPath = GetFolderAnotherName(parentFolder.FolderPath, "Новая папка");
                folder.Title = Path.GetDirectoryName(newFolderPath);
                folder.FolderPath = newFolderPath;
            }
            else
            {
                newFolderPath = @$"{parentFolder.FolderPath}\{folder.Title}";
                if (Directory.Exists(newFolderPath))
                {
                    newFolderPath = GetFolderAnotherName(parentFolder.FolderPath, folder.Title);
                }

                Directory.CreateDirectory(newFolderPath);
                folder.FolderPath = newFolderPath;
            }
            
            Folder createdFolderEntity = await _folderDataService.Create(folder);
            if (createdFolderEntity != null)
            {
                createdFolderEntity.ParentFolder = parentFolder;
                parentFolder.Folders.Add(createdFolderEntity);
                Folders.Add(createdFolderEntity);
                OnAddFolder?.Invoke(createdFolderEntity, "Папка успешно добавлена");
                
                return createdFolderEntity;
            }
            return null;
        }

        public async Task EditFolder(Folder folder)
        {
            Folder updatedFolderEnitiy = await _folderDataService.Update(folder.Id, folder);
            if (updatedFolderEnitiy != null)
            {
                updatedFolderEnitiy.OnAllPropertyChanged();
                OnEditFolder?.Invoke(updatedFolderEnitiy, "Папка успешно изменена");
            }
        }

        public async Task RemoveFolder(Folder folder)
        {
            if (await _folderDataService.Delete(folder.Id))
            {
                folder.ParentFolder.Folders.Remove(folder);
                Folders.Remove(folder);

                IEnumerable<Folder> foldersForRemove = MemeExtentions.SelectRecursive(folder.Folders, f => f.Folders);

                foldersForRemove.ToList().ForEach(folderFR =>
                {
                    folderFR.Memes.ToList().ForEach(m =>
                    {
                        Memes.Remove(m);
                        OnRemoveMeme?.Invoke(m, null);
                    });
                    Folders.Remove(folderFR);
                    OnRemoveFolder?.Invoke(folderFR, null);
                });

                GC.Collect();
                GC.WaitForPendingFinalizers();

                OnRemoveFolder?.Invoke(folder, "Папка успешно удалена");
            }
        }

        public async Task AddMeme(Meme meme, Folder folder)
        {
            try
            {
                string newMemePath = @$"{folder.FolderPath}\{meme.Title}{Path.GetExtension(meme.ImagePath)}";
                if (File.Exists(newMemePath))
                {
                    newMemePath = GetMemeAnotherName(folder.FolderPath, meme.Title, meme.ImagePath);
                    meme.Title = Path.GetFileNameWithoutExtension(newMemePath);
                    meme.ImagePath = newMemePath;
                }
                else
                {
                    File.Copy(meme.ImagePath, newMemePath);
                    meme.ImagePath = newMemePath;
                }

                meme.ImageData = MemeExtentions.ConvertImageToByteArray(meme.ImagePath);

                List<MemeTagNode> memeTagNodes = new List<MemeTagNode>();
                foreach(MemeTagNode mtn in meme.Tags.ToArray())
                {
                    memeTagNodes.Add(mtn);
                    meme.Tags.Remove(mtn);
                }

                Meme createdMemeEntity = await _memeDataService.Create(meme);
                if (createdMemeEntity != null)
                {
                    createdMemeEntity.Image = MemeExtentions.ConvertByteArrayToImage(createdMemeEntity.ImageData);
                    createdMemeEntity.Image.Freeze();
                    createdMemeEntity.ImageData = null;
                    folder.Memes.Add(createdMemeEntity);
                    folder.OnPropertyChanged(nameof(folder.Memes));
                    Memes.Add(createdMemeEntity);

                    memeTagNodes.ForEach(async (newMemeTagNode) =>
                    {
                        newMemeTagNode.Meme = createdMemeEntity;
                        MemeTagNode dbCreatedMemeTagNode = await _memeTagNodeDataService.Create(newMemeTagNode);
                        createdMemeEntity.Tags.Add(dbCreatedMemeTagNode);
                    });

                    OnAddMeme?.Invoke(createdMemeEntity, "Мем успешно добавлен");
                }
            }
            catch (Exception ex)
            {

            }
            
        }

        public async Task EditMeme(Meme meme)
        {
            meme.ImageData = MemeExtentions.ConvertImageToByteArray(meme.ImagePath);

            Meme oldMemeData = await _memeDataService.Get(meme.Id);
            
            List<MemeTagNode> oldMemeTagNodesForRemove = new List<MemeTagNode>();
            foreach (MemeTagNode memeTagNode in oldMemeData.Tags.ToArray())
            {
                bool flag = true;
                foreach (MemeTagNode newMemeTagNode in meme.Tags)
                {
                    if (memeTagNode.MemeTag.Id == newMemeTagNode.MemeTag.Id)
                    {
                        flag = false;
                        break;
                    }
                }

                if (flag)
                {
                    await _memeTagNodeDataService.Delete(memeTagNode.Id);
                    oldMemeData.Tags.Remove(memeTagNode);
                }
            }


            foreach (MemeTagNode newMemeTagNode in meme.Tags.ToArray())
            {
                bool flag = true;
                foreach(MemeTagNode memeTagNode in oldMemeData.Tags)
                {
                    if (memeTagNode.MemeTag.Id == newMemeTagNode.MemeTag.Id)
                    {
                        flag = false;
                        break;
                    }
                }

                if (flag)
                {
                    MemeTagNode dbCreatedMemeTagNode = await _memeTagNodeDataService.Create(newMemeTagNode);
                    MemeExtentions.ReplaceReference(meme.Tags, dbCreatedMemeTagNode, mtn => mtn == newMemeTagNode);
                }
                
            }

            Meme updatedMemeEnitiy = await _memeDataService.Update(meme.Id, meme);
            if (updatedMemeEnitiy != null)
            {
                updatedMemeEnitiy.Image = MemeExtentions.ConvertByteArrayToImage(updatedMemeEnitiy.ImageData);
                updatedMemeEnitiy.Image.Freeze();
                updatedMemeEnitiy.ImageData = null;
                updatedMemeEnitiy.OnAllPropertyChanged();

                OnEditMeme?.Invoke(updatedMemeEnitiy, "Мем успешно изменён");
            }
        }

        public async Task RemoveMeme(Meme meme)
        {
            if (await _memeDataService.Delete(meme.Id))
            {
                Folder folder = Folders.FirstOrDefault(f => f.Id == meme.Folder.Id);
                folder.Memes.Remove(meme);
                Memes.Remove(meme);

                OnRemoveMeme?.Invoke(meme, "Мем успешно удалён");
            }
        }

        public async Task AddMemeTag(MemeTag memeTag)
        {
            MemeTag createdMemeTagEnitiy = await _memeTagDataService.Create(memeTag);
            if (createdMemeTagEnitiy != null)
            {
                MemeTags.Add(createdMemeTagEnitiy);

                OnAddMemeTag?.Invoke(createdMemeTagEnitiy, "Тег успешно добавлен");
            }
        }

        public async Task EditMemeTag(MemeTag memeTag)
        {
            MemeTag updatedMemeTagEnitiy = await _memeTagDataService.Update(memeTag.Id, memeTag);
            if (updatedMemeTagEnitiy != null)
            {
                updatedMemeTagEnitiy.OnAllPropertyChanged();
                
                OnEditMemeTag?.Invoke(updatedMemeTagEnitiy, "Тег успешно изменён");
            }
        }

        public async Task RemoveMemeTag(MemeTag memeTag)
        {
            if (await _memeTagDataService.Delete(memeTag.Id))
            {
                IEnumerable<Meme> memes = Memes.Where(meme => meme.Tags.FirstOrDefault(m => m.MemeTag.Id == memeTag.Id) != null);
                foreach (Meme meme in memes)
                {
                    MemeTagNode memeTagNode = meme.Tags.FirstOrDefault(mtn => mtn.MemeTag.Id == memeTag.Id);
                    meme.Tags.Remove(memeTagNode);
                    await _memeTagNodeDataService.Delete(memeTagNode.Id);
                }
                MemeTags.Remove(memeTag);

                OnRemoveMemeTag?.Invoke(memeTag, "Тег успешно удалён");
            }      
        }

        public DataStorage(Guid rootGuid,
            IMemeDataService memeDataService,
            IFolderDataService folderDataService,
            IMemeTagDataService memeTagDataService,
            IMemeTagNodeDataService memeTagNodeDataService)
        {
            _memeDataService = memeDataService;
            _folderDataService = folderDataService;
            _memeTagDataService = memeTagDataService;
            _memeTagNodeDataService = memeTagNodeDataService;

            Folders = new ObservableCollection<Folder>();
            Memes = new ObservableCollection<Meme>();
            MemeTags = new ObservableCollection<MemeTag>();

            foreach (MemeTag memeTag in _memeTagDataService.GetAll().Result)
                MemeTags.Add(memeTag);

            RootFolder = _folderDataService.GetFoldersByTitle("root").Result[0];
            GetDataRecursively(RootFolder);
        }

        private void GetDataRecursively(Folder folder)
        {
            foreach (Meme meme in folder.Memes)
            {
                Memes.Add(meme);
                if (meme.Tags != null)
                    foreach (MemeTagNode memeTagNode in meme.Tags)
                    {
                        MemeTag memeTag = MemeTags.FirstOrDefault(mt => mt.Id == memeTagNode.MemeTag.Id);
                        if (!memeTag.CheckFlag)
                        {
                            MemeExtentions.ReplaceReference(MemeTags, memeTag, mt => mt.Id == memeTag.Id);
                            memeTag.CheckFlag = true;
                        }
                            
                    }
            }

            Folders.Add(folder);

            foreach (Folder innerFolder in folder.Folders)
                GetDataRecursively(innerFolder);
        }

    }
}
