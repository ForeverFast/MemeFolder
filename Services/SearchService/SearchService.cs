using MemeFolder.Domain.Models;
using MemeFolder.Domain.Models.AbstractModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemeFolder.Services
{
    public delegate void SearchServiceEvent(IEnumerable<FolderObject> folderObjects);

    public class SearchService : ISearchService
    {
        private readonly IFolderDataService _folderDataService;

        public event SearchServiceEvent NewRequest;

        public ObservableCollection<FolderObject> FolderObjects { get; }

        public void GetWhere(Func<FolderObject,bool> func)
        {
            NewRequest?.Invoke(FolderObjects.Where(func));
        }

        public SearchService(IFolderDataService folderDataService)
        {
            _folderDataService = folderDataService;

            FolderObjects = new ObservableCollection<FolderObject>();

            foreach (Folder folder in _folderDataService.GetAll().Result)
            {
                foreach (Meme meme in folder.Memes)
                    FolderObjects.Add(meme);
                FolderObjects.Add(folder);
            }

        }
    }
}
