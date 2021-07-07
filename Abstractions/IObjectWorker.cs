using MemeFolder.Domain.Models;

namespace MemeFolder.Abstractions
{
    public interface IObjectWorker
    {
        Folder GetModel();
        object GetWorkerCollection(ObjectType collectionType);
    }
}
