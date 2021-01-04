using MemeFolder.Domain.Models;
using MemeFolder.EntityFramework;
using MemeFolder.EntityFramework.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MemeFolder.Services
{
    public class FolderDataService : GenericDataService<Folder>, IFolderDataService
    {

        public override async Task<IEnumerable<Folder>> GetAll()
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                IEnumerable<Folder> entities = await context.Set<Folder>().Include(f => f.Memes).ToListAsync();
                return entities;
            }
        }

        public async Task<ObservableCollection<Folder>> GetFoldersByTitle(string Title)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                Expression<Func<Folder, bool>> expression = x => x.Title == Title;
                IEnumerable<Folder> entities = await context.Set<Folder>().Where(expression)//.ToListAsync();
                //IEnumerable<Folder> entities = await context.Folders.Where(expression)
                    //.Include(x => x.Folders)
                    //.ThenInclude(x => x.Folders)
                    .Include(x => x.Memes)
                    .ToListAsync();

                
                return new ObservableCollection<Folder>(entities);
            }
        }

        #region Конструкторы

        public FolderDataService(MemeFolderDbContextFactory dbf) : base(dbf)
        {
        
        }

        #endregion
    }
}
