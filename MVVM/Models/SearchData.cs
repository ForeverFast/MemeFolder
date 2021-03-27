using MemeFolder.Domain.Models.AbstractModels;
using MemeFolder.MVVM.ViewModels;
using System.Collections.ObjectModel;

namespace MemeFolder.MVVM.Models
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
