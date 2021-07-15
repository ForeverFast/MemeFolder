using MemeFolder.Domain.Models;
using MemeFolder.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MemeFolder.Services
{
    public class MemeTagNodeDataService : IMemeTagNodeDataService
    {

        protected readonly MemeFolderDbContextFactory _contextFactory;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public virtual async Task<bool> Delete(Guid guid)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                MemeTagNode entity = await context.MemeTagNodes.FirstOrDefaultAsync(e => e.Id == guid);
                context.MemeTagNodes.Remove(entity);

                await context.SaveChangesAsync();

                return true;
            }
        }

        public virtual async Task<MemeTagNode> Get(Guid guid)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                MemeTagNode entity = await context.MemeTagNodes
                    .Include(mtn => mtn.Meme)
                    .Include(mtn => mtn.MemeTag)
                    .FirstOrDefaultAsync(e => e.Id == guid);
                return entity;
            }
        }

        public virtual async Task<MemeTagNode> Create(MemeTagNode memeTagNode)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                try
                {
                    Meme meme = await context.Memes.FirstOrDefaultAsync(m => m.Id == memeTagNode.Meme.Id);
                    if (meme != null)
                        memeTagNode.Meme = meme;
                    else
                        throw new ArgumentNullException("Meme can not be null");

                    MemeTag memeTag = await context.MemeTags.FirstOrDefaultAsync(mt => mt.Id == memeTagNode.MemeTag.Id);
                    if (memeTag != null)
                        memeTagNode.MemeTag = memeTag;
                    else
                        throw new ArgumentNullException("MemeTag can not be null");

                    EntityEntry<MemeTagNode> createdResult = await context.MemeTagNodes.AddAsync(memeTagNode);
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

        public virtual async Task<MemeTagNode> Update(Guid guid, MemeTagNode entity)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                try
                {
                    var original = await context.MemeTags.FirstOrDefaultAsync(e => e.Id == guid);

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

        public virtual async Task<IEnumerable<MemeTagNode>> GetAll()
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                try
                {
                    IEnumerable<MemeTagNode> entities = await Task.FromResult(context.MemeTagNodes
                        .Include(mtn => mtn.Meme)
                        .Include(mtn => mtn.MemeTag)
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



        #region Конструкторы
        public MemeTagNodeDataService()
        {
            _contextFactory = new MemeFolderDbContextFactory();
        }

        public MemeTagNodeDataService(MemeFolderDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        #endregion

    }
}
