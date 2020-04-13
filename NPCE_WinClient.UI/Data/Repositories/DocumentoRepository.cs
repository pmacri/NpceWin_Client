using NPCE_WinClient.DataAccess;
using NPCE_WinClient.Model;

namespace NPCE_WinClient.UI.Data.Repositories
{
    public class DocumentoRepository: GenericRepository<Documento, NpceDbContext>, IDocumentoRepository
    {
        public DocumentoRepository(NpceDbContext context): base(context)
        {

        }
    }
}
