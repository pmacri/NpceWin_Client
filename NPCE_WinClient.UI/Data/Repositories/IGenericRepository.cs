using System.Collections.Generic;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Data.Repositories
{

    public interface IGenericRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        Task SaveAsync();
        bool HasChanges();
        void Add(T model);
        void Remove(T model);

    }
}