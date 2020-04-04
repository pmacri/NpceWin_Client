using NPCE_WinClient.DataAccess;
using NPCE_WinClient.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Data.Repositories
{
    public class ServizioRepository: GenericRepository<Servizio, NpceDbContext>, IServizioRepository
    {
        public ServizioRepository(NpceDbContext context) : base(context)
        {
        }

        public IEnumerable<Anagrafica> GetAllAnagrafiche()
        {
            return Context.Anagrafica.ToList();
        }

        public IEnumerable<Documento> GetAllDocumenti()
        {
            return Context.Documento.ToList();
        }

        public IEnumerable<TipoServizio> GetAllTipiServizio()
        {
            return Context.TipoServizio.ToList();
        }

        public override async Task<Servizio> GetByIdAsync(int id)
        {
            var servizio = await Context.Servizio.Include(s => s.Anagrafiche).Include(s => s.Documenti).Include(s => s.TipoServizio).Include(s => s.StatoServizio)
                .Include(s => s.PagineBollettini).Include(s => s.PagineBollettini.Select(pb => pb.Bollettini)).SingleAsync(s => s.Id == id);
            return servizio;
        }

        public override async Task<IEnumerable<Servizio>> GetAllAsync()
        {
            return await Context.Servizio.Include(s => s.Anagrafiche).Include(s => s.Documenti).Include(s => s.TipoServizio).Include(s => s.StatoServizio).ToListAsync();
        }



        public async void UpdateTipoServizioAsync(int idServizio, int idTipoServizio)
        {
            var servizio = Context.Servizio.FirstOrDefault(s => s.Id == idServizio);

            if (servizio != null)
            {
                servizio.TipoServizioId = idTipoServizio;
                await Context.SaveChangesAsync();
            }
            
        }
        
    }
}
