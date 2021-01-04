using MemeFolder.Domain.Models;
using MemeFolder.EntityFramework.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace MemeFolder.Services
{
    public interface IFolderDataService : IGenericDataService<Folder> 
    {
        Task<ObservableCollection<Folder>> GetFoldersByTitle(string Title);
    }
}
