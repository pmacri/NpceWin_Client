using NPCE_WinClient.DataAccess;
using NPCE_WinClient.Model;

namespace NPCE_WinClient.UI.Data.Repositories
{
    public class VisureRepository: GenericRepository<Visura, NpceDbContext>, IVisureRepository
    {
        public VisureRepository(NpceDbContext context): base(context)
        {

        }
    }
}
