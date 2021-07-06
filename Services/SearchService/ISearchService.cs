using MemeFolder.Domain.Models.AbstractModels;
using System;

namespace MemeFolder.Services
{
    public interface ISearchService
    {
        void GetWhere(Func<FolderObject, bool> func);

        event SearchServiceEvent NewRequest;
    }
}
