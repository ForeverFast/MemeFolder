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
        private readonly IMemeDataService _memeDataService;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder">Сущность</param>
        /// <param name="folderGuid">Id родителя</param>
        /// <returns></returns>
        public override async Task<Folder> Create(Folder folder)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                var check = await context.Folders.Where(x => x.Id == folder.ParentFolder.Id).FirstOrDefaultAsync();
                if (check != null)
                    folder.ParentFolder = check;

                EntityEntry<Folder> createdResult = await context.Set<Folder>().AddAsync(folder);
                await context.SaveChangesAsync();

                return createdResult.Entity;
            }
        }

        public override async Task<bool> Delete(Guid guid)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {

                var entities = await GetAll();
                var entity = entities.ToList().FirstOrDefault(x => x.Id == guid);
                if (entity != null)
                {
                    RemoveAllData(entity, context);
                    context.Folders.Remove(entity);
                    await context.SaveChangesAsync();
                    return true;
                }
                else
                    return false;

            }
        }

        private void RemoveAllData(Folder folder, MemeFolderDbContext context)
        {
            foreach (var item in folder.Folders)
                RemoveAllData(item, context);
            foreach(var meme in folder.Memes)
            {
                var memeEntity = context.Memes.FirstOrDefault(x => x.Id == meme.Id);
                if (memeEntity != null)
                {
                    context.Memes.Remove(memeEntity);
                    
                }
            }
            folder.Memes.Clear();
            context.SaveChanges();
            foreach (var folder1 in folder.Folders)
            {
                var folderEntity = context.Folders.FirstOrDefault(x => x.Id == folder1.Id);
                if (folderEntity != null)
                {
                    context.Folders.Remove(folderEntity);
                   
                }
            }
            folder.Folders.Clear();
            context.SaveChanges();
        }

        #region Конструкторы

        public FolderDataService(MemeFolderDbContextFactory dbf,
                                 IMemeDataService memeDataService) : base(dbf)
        {
            _memeDataService = memeDataService;
        }

        #endregion
    }
}
