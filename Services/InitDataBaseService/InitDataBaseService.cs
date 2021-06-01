using MemeFolder.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MemeFolder.Services
{
    public class InitDataBaseService : IInitDataBaseService
    {
        private readonly IFolderDataService _folderDataService;
        private readonly Guid rootGuid = Guid.Parse("BBBA3651-35D9-465E-BDA2-CAD811E35865");


        public async Task<bool> Init()
        {
            var rootFolder = await _folderDataService.GetFoldersByTitle("root");
            if (rootFolder.Count == 0)
            {
                if (await _folderDataService.CreateRootFolder(new Folder() { Id = rootGuid, Title = "root" })  != null)
                    return true;
                else
                    return false;
            }
            else if (rootFolder.Count > 1)
            {
                rootFolder.ToList().ForEach(x =>
                {
                    if (x.Id != rootGuid)
                    {
                        _folderDataService.Delete(x.Id);
                    }
                });
                return true;
            }
            else if (rootFolder.Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public InitDataBaseService(IFolderDataService folderDataService)
        {
            _folderDataService = folderDataService;
        }
    }
}
