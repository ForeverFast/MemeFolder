using MemeFolder.Domain.Models;
using MemeFolder.Domain.Models.AbstractModels;
using System.Collections.ObjectModel;

namespace MemeFolder.Abstractions
{
    public interface IFolderObjectWorker
    {
        Folder GetModel();
        ObservableCollection<FolderObject> GetWorkerCollection();
    }
}
