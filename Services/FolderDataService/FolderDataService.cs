using MemeFolder.Domain.Models;
using MemeFolder.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MemeFolder.Services
{
    public class FolderDataService : IFolderDataService
    {
        private readonly MemeFolderDbContextFactory _contextFactory;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public virtual async Task<Folder> Get(Guid guid)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                Folder entity = await context.Folders.Include(f => f.Memes).FirstOrDefaultAsync(e => e.Id == guid);
                return entity;
            }
        }

        public virtual async Task<Folder> Create(Folder folder)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                try
                {
                    if (folder.Title == "root")
                        throw new Exception("Fig tebe, a ne root folder");

                    var check = await context.Folders.Where(x => x.Id == folder.ParentFolder.Id).FirstOrDefaultAsync();
                    if (check != null)
                        folder.ParentFolder = check;

                    EntityEntry<Folder> createdResult = await context.Set<Folder>().AddAsync(folder);
                    await context.SaveChangesAsync();
                    return createdResult.Entity;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Ошибка добавления");
                    return null;
                }
            }
        }

        public async Task<Folder> CreateRootFolder(Folder folder)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                try
                {
                    EntityEntry<Folder> createdResult = await context.Set<Folder>().AddAsync(folder);
                    context.SaveChanges();

                    return createdResult.Entity;

                }
                catch(Exception ex)
                {
                    logger.Error(ex,"Ошибка созданиея");
                    return null;
                }
               
            }
        }

        public virtual async Task<bool> Delete(Guid guid)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                try
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
                catch(Exception ex)
                {
                    logger.Error(ex, "Ошибка удаления");
                    return false;
                }
               

            }
        }

        public virtual async Task<Folder> Update(Guid guid, Folder folder)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                try
                {
                    var original = await context.Folders.FirstOrDefaultAsync(e => e.Id == guid);

                    foreach (PropertyInfo propertyInfo in original.GetType().GetProperties())
                    {
                        if (propertyInfo.GetValue(folder, null) == null)
                            propertyInfo.SetValue(folder, propertyInfo.GetValue(original, null), null);
                    }
                    context.Entry(original).CurrentValues.SetValues(folder);
                    await context.SaveChangesAsync();

                    return folder;

                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Ошибка обновления");
                    return null;
                }

            }
        }

        public virtual async Task<IEnumerable<Folder>> GetAll()
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                try
                {
                    IEnumerable<Folder> entities = await Task.FromResult(context.Folders
                        .Include(f => f.Memes)
                        .ToList());
                    return entities;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Ошибка получения");
                    return null;
                }

            }
        }

        public async Task<ObservableCollection<Folder>> GetFoldersByTitle(string Title)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                try
                {
                    IEnumerable<Folder> entities = await Task.FromResult(context.Folders
                      .Include(x => x.Memes)
                      .ToList()
                      .Where(x => x.Title == Title));
                    return new ObservableCollection<Folder>(entities);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Ошибка получения");
                    return null;
                }
              
            }
        }


        #region Вспомогательные методы
        
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

        #endregion


        #region Конструкторы

        public FolderDataService()
        {
            _contextFactory = new MemeFolderDbContextFactory();
        }

        public FolderDataService(MemeFolderDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        #endregion
    }
}
