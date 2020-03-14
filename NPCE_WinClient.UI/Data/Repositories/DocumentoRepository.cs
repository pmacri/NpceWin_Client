using NPCE_WinClient.DataAccess;
using NPCE_WinClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Data.Repositories
{
    public class DocumentoRepository: GenericRepository<Documento, NpceDbContext>, IDocumentoRepository
    {
        public DocumentoRepository(NpceDbContext context): base(context)
        {

        }
    }
}
