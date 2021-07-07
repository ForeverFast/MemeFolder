using MemeFolder.Domain.Models;
using MemeFolder.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NLog;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace MemeFolder.Services
{
    public class MemeTagDataService : IMemeTagDataService
    {
        protected readonly MemeFolderDbContextFactory _contextFactory;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public virtual async Task<bool> Delete(Guid guid)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                MemeTag entity = await context.MemeTags.FirstOrDefaultAsync(e => e.Id == guid);
                context.MemeTags.Remove(entity);

                await context.SaveChangesAsync();

                return true;
            }
        }

        public virtual async Task<MemeTag> Get(Guid guid)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                MemeTag entity = await context.MemeTags.FirstOrDefaultAsync(e => e.Id == guid);
                return entity;
            }
        }

        public virtual async Task<MemeTag> Create(MemeTag meme)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                try
                {
                    EntityEntry<MemeTag> createdResult = await context.MemeTags.AddAsync(meme);
                    await context.SaveChangesAsync();

                    return createdResult.Entity;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Ошибка создания");
                    return null;
                }
            }
        }

        public virtual async Task<MemeTag> Update(Guid guid, MemeTag entity)
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
                    logger.Error(ex, "Ошибка обновления");
                    return null;
                }

            }


        }

        public virtual async Task<IEnumerable<MemeTag>> GetAll()
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                IEnumerable<MemeTag> entities = await context.MemeTags.ToListAsync();
                return entities;
            }
        }



        #region Конструкторы
        public MemeTagDataService()
        {
            _contextFactory = new MemeFolderDbContextFactory();
        }

        public MemeTagDataService(MemeFolderDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        #endregion
    }

}
