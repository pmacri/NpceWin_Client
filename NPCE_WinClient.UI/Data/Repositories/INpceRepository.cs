using System.Collections.Generic;
using System.Threading.Tasks;
using NPCE_WinClient.Model;

namespace NPCE_WinClient.UI.Data.Repositories
{
    public interface INpceRepository
    {
       Task<List<Service>> GetAllAsync();
        Task<Service> GetServiceByIdAsync(long id);
        Task<Anagrafica> GetByIdAsync(long id);
        Task SaveAsync();
        bool HasChanges();

    }
}