using MemeFolder.Domain.Models.AbstractModels;
using MemeFolder.ViewModels;
using System.Collections.ObjectModel;

namespace MemeFolder.Data
{
    public class SearchData
    {
        public ObservableCollection<FolderVM> NavigationData { get; set; }
        public ObservableCollection<FolderObject> SearchResult { get; set; }

        public SearchData()
        {
            NavigationData = new ObservableCollection<FolderVM>();
            SearchResult = new ObservableCollection<FolderObject>();
        }
    }
}
