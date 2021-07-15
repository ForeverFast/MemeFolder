using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MemeFolder.EntityFramework.Services
{
    public interface IGenericDataService<T>
    {
        Task<T> Get(Guid guid);
        Task<T> Create(T entity);
        Task<bool> Delete(Guid guid);
        Task<T> Update(Guid guid, T entity);      
        Task<IEnumerable<T>> GetAll();
    }
}
