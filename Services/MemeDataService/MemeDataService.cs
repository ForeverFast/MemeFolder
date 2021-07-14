using MemeFolder.Domain.Models;
using MemeFolder.EntityFramework;
using MemeFolder.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MemeFolder.Services
{
    public class MemeDataService : IMemeDataService
    {
        protected readonly MemeFolderDbContextFactory _contextFactory;
        private static Logger logger = LogManager.GetCurrentClassLogger();
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
                Meme entity = await context.Memes
                    .Include(m => m.Folder)
                    .Include(m => m.Tags)
                        .ThenInclude(mtn => mtn.MemeTag)
                    .FirstOrDefaultAsync(e => e.Id == guid);
                return entity;
            }
        }

        public virtual async Task<Meme> Create(Meme meme)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                try
                {
                    if (string.IsNullOrEmpty(meme.ImagePath))
                        throw new Exception("No image path");

                    if (string.IsNullOrEmpty(meme.Title))
                        meme.Title = Path.GetFileNameWithoutExtension(meme.ImagePath);

                    if (meme.ImageData == null)
                        meme.ImageData = MemeExtentions.ConvertImageToByteArray(meme.ImagePath);

                    Folder parentFolderEntity = await context.Folders.FirstOrDefaultAsync(x => x.Id == meme.Folder.Id);
                    if (parentFolderEntity != null)
                        meme.Folder = parentFolderEntity;

                    EntityEntry<Meme> createdResult = await context.Memes.AddAsync(meme);
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
                    logger.Error(ex, "Ошибка обновления");
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

