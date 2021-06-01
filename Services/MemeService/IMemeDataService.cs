using MemeFolder.Domain.Models;
using MemeFolder.EntityFramework.Services;
using System;
using System.Threading.Tasks;

namespace MemeFolder.Services
{
    public interface IMemeDataService : IGenericDataService<Meme>
    {
        Task<Meme> Create(Meme entity, Guid containerId);
    }
}
