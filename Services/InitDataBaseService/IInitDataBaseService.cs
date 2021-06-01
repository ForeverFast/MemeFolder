using System.Threading.Tasks;

namespace MemeFolder.Services
{
    public interface IInitDataBaseService
    {
        Task<bool> Init();
    }
}
