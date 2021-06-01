using MemeFolder.Domain.Models;
using MemeFolder.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MemeFolder.Services
{
    public class MemeDataService : IMemeDataService
    {
        protected readonly MemeFolderDbContextFactory _contextFactory;

        public virtual async Task<bool> Delete(Guid guid)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                Meme entity = await context.Memes.FirstOrDefaultAsync(e => e.Id == guid);
                context.Memes.Remove(entity);

                await context.SaveChangesAsync();

                return true;
            }
        }

        public virtual async Task<Meme> Get(Guid guid)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                Meme entity = await context.Memes.FirstOrDefaultAsync(e => e.Id == guid);
                return entity;
            }
        }

        public virtual async Task<Meme> Create(Meme meme)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                try
                {
                    var check = await context.Folders
                        .Where(x => x.Id == meme.Folder.Id)
                        .FirstOrDefaultAsync();
                    if (check != null)
                        meme.Folder = check;

                    EntityEntry<Meme> createdResult = await context.Set<Meme>().AddAsync(meme);
                    await context.SaveChangesAsync();

                    return createdResult.Entity;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public virtual async Task<Meme> Create(Meme entity, Guid containerId)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                try
                {
                    var check = await context.Folders
                        .Where(x => x.Id == containerId)
                        .FirstOrDefaultAsync();
                    if (check != null)
                        entity.Folder = check;

                    EntityEntry<Meme> createdResult = await context.Set<Meme>().AddAsync(entity);
                    await context.SaveChangesAsync();

                    return createdResult.Entity;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public virtual async Task<Meme> Update(Guid guid, Meme entity)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                try
                {
                    var original = await context.Memes.FirstOrDefaultAsync(e => e.Id == guid);

                    foreach (PropertyInfo propertyInfo in original.GetType().GetProperties())
                    {
                        if (propertyInfo.GetValue(entity, null) == null)
                            propertyInfo.SetValue(entity, propertyInfo.GetValue(original, null), null);
                    }
                    context.Entry(original).CurrentValues.SetValues(entity);
                    await context.SaveChangesAsync();

                    return entity;

                }
                catch (Exception ex)
                {
                    return null;
                }

            }


        }

        public virtual async Task<IEnumerable<Meme>> GetAll()
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                IEnumerable<Meme> entities = await context.Memes.ToListAsync();
                return entities;
            }
        }

       

        #region Конструкторы
        public MemeDataService()
        {
            _contextFactory = new MemeFolderDbContextFactory();
        }

        public MemeDataService(MemeFolderDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        #endregion
    }
}

