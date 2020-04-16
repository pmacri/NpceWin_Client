using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using NPCE_WinClient.DataAccess;
using NPCE_WinClient.Model;

namespace NPCE_WinClient.UI.Data.Repositories
{
    public class VisureRepository: GenericRepository<Visura, NpceDbContext>, IVisureRepository
    {
        public VisureRepository(NpceDbContext context): base(context)
        {

        }

        public async Task<IEnumerable<VisureCodiceDocumento>> GetAllCodiciDocumentoAsync()
        {
            return await Context.VisureCodiceDocumento.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<VisureFormatoDocumento>> GetAllFormatiDocumentoAsync()
        {
            return await Context.VisureFormatoDocumento.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<VisureTipoDocumento>> GetAllTipiDocumentoAsync()
        {
            return await Context.VisureTipoDocumento.AsNoTracking().ToListAsync();
        }

        public async Task<VisureFormatoDocumento> GetFormatoDocumentoByDescriptionAsync(string description)
        {
            return await Context.VisureFormatoDocumento.Where(td => td.Descrizione == description).FirstAsync();
        }

        public async Task<VisureTipoDocumento> GetTipoDocumentoByDescriptionAsync(string description)
        {
            return await Context.VisureTipoDocumento.Where(td => td.Descrizione == description).FirstAsync();
        }

    }
}
