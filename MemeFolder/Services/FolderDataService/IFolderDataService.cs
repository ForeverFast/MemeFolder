using MemeFolder.Domain.Models;
using MemeFolder.EntityFramework;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MemeFolder.Services
{
    public interface IFolderDataService : IGenericDataService<Folder> 
    {
        Task<ObservableCollection<Folder>> GetFoldersByTitle(string Title);

        Task<Folder> CreateRootFolder(Folder folder);
    }
}
