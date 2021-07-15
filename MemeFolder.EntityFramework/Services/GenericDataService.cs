using MemeFolder.Domain.Models.AbstractModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace MemeFolder.EntityFramework.Services
{
    public class GenericDataService<T> : IGenericDataService<T> where T : DomainObject
    {
        protected readonly MemeFolderDbContextFactory _contextFactory;

        public GenericDataService()
        {
            _contextFactory = new MemeFolderDbContextFactory();
        }

        public GenericDataService(MemeFolderDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public virtual async Task<T> Create(T entity)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                EntityEntry<T> createdResult = await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();

                return createdResult.Entity;
            }
        }

        public virtual async Task<bool> Delete(Guid guid)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                T entity = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == guid);
                context.Set<T>().Remove(entity);

                await context.SaveChangesAsync();

                return true;
            }
        }

        public virtual async Task<T> Get(Guid guid)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                T entity = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == guid);
                return entity;
            }
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                IEnumerable<T> entities = await context.Set<T>().ToListAsync();
                return entities;
            }
        }

        public virtual async Task<T> Update(Guid guid, T entity)
        {
            using (MemeFolderDbContext context = _contextFactory.CreateDbContext(null))
            {
                try
                {
                    var original = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == guid);

                    foreach (PropertyInfo propertyInfo in original.GetType().GetProperties())
                    {
                        if (propertyInfo.GetValue(entity, null) == null)
                            propertyInfo.SetValue(entity, propertyInfo.GetValue(original, null), null);
                    }
                    context.Entry(original).CurrentValues.SetValues(entity);
                    await context.SaveChangesAsync();

                    //entity.Id = guid;
                    //T ent = context.Entry(entity);
                    //ent.State = EntityState.Modified;

                    //context.Entry(entity).State = EntityState.Modified;

                    //context.Set<T>().Update(entity);
                    //await context.SaveChangesAsync();

                    return entity;

                }
                catch (Exception ex)
                {
                    return null;
                }
              
            }
        }
    }
}
