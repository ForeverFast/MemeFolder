using MemeFolder.Domain.Models;
using MemeFolder.Domain.Models.AbstractModels;
using MemeFolder.Extentions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemeFolder.Services
{
    public delegate void SearchServiceEvent(IEnumerable<Meme> folderObjects);
    public delegate void FolderEvent(Folder folder);
    public delegate void MemeEvent(Meme meme);

    public class DataStorage
    {
        private readonly IFolderDataService _folderDataService;
        private readonly IMemeDataService _memeDataService;
        private readonly IMemeTagDataService _memeTagDataService;

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


        public async Task<Folder> AddFolder(Folder folder, Folder parentFolder)
        {
            Folder createdFolderEntity = await _folderDataService.Create(folder);
            if (createdFolderEntity != null)
            {
                createdFolderEntity.ParentFolder = parentFolder;
                parentFolder.Folders.Add(createdFolderEntity);
                Folders.Add(createdFolderEntity);
                OnAddFolder?.Invoke(createdFolderEntity);
                
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
                OnEditFolder?.Invoke(updatedFolderEnitiy);
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
                        OnRemoveMeme?.Invoke(m);
                    });
                    Folders.Remove(folderFR);
                    OnRemoveFolder?.Invoke(folderFR);
                });
               
                OnRemoveFolder?.Invoke(folder);
            }
        }

        public async Task AddMeme(Meme meme, Folder folder)
        {
            meme.ImageData = MemeExtentions.ConvertImageToByteArray(meme.ImagePath);
            Meme createdMemeEntity = await _memeDataService.Create(meme);
            if (createdMemeEntity != null)
            {
                createdMemeEntity.Image = MemeExtentions.ConvertByteArrayToImage(createdMemeEntity.ImageData);
                createdMemeEntity.Image.Freeze();
                createdMemeEntity.ImageData = null;
                folder.Memes.Add(createdMemeEntity);
                folder.OnPropertyChanged(nameof(folder.Memes));
                Memes.Add(createdMemeEntity);

                OnAddMeme?.Invoke(createdMemeEntity);
            }
        }

        public async Task EditMeme(Meme meme)
        {
            meme.ImageData = MemeExtentions.ConvertImageToByteArray(meme.ImagePath);
            Meme updatedMemeEnitiy = await _memeDataService.Update(meme.Id, meme);
            if (updatedMemeEnitiy != null)
            {
                updatedMemeEnitiy.Image = MemeExtentions.ConvertByteArrayToImage(updatedMemeEnitiy.ImageData);
                updatedMemeEnitiy.Image.Freeze();
                updatedMemeEnitiy.ImageData = null;
                updatedMemeEnitiy.OnAllPropertyChanged();

                OnEditMeme?.Invoke(updatedMemeEnitiy);
            }
        }

        public async Task RemoveMeme(Meme meme)
        {
            if (await _memeDataService.Delete(meme.Id))
            {
                Folder folder = Folders.FirstOrDefault(f => f.Id == meme.Folder.Id);
                folder.Memes.Remove(meme);
                Memes.Remove(meme);

                OnRemoveMeme?.Invoke(meme);
            }
        }

        public async Task AddMemeTag(MemeTag memeTag)
        {
            MemeTag createdMemeTagEnitiy = await _memeTagDataService.Create(memeTag);
            if (createdMemeTagEnitiy != null)
            {
                MemeTags.Add(createdMemeTagEnitiy);
            }
        }

        public async Task EditMemeTag(MemeTag memeTag)
        {
            MemeTag updatedMemeTagEnitiy = await _memeTagDataService.Update(memeTag.Id, memeTag);
            if (updatedMemeTagEnitiy != null)
            {
                updatedMemeTagEnitiy.OnAllPropertyChanged();
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
                }

            }
        }

        public DataStorage(IMemeDataService memeDataService,
            IFolderDataService folderDataService,
            IMemeTagDataService memeTagDataService)
           
        {
            _memeDataService = memeDataService;
            _folderDataService = folderDataService;
            _memeTagDataService = memeTagDataService;
            
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
