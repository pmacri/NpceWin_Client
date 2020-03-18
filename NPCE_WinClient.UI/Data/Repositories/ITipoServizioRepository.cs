using NPCE_WinClient.DataAccess;
using NPCE_WinClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Data.Repositories
{
    public interface ITipoServizioRepository
    {
        TipoServizio GetByDescription(string description);
        IEnumerable<TipoServizio> GetByAll();
    }

    public class TipoServizioRepository : ITipoServizioRepository
    {
        private readonly NpceDbContext _context;

        public TipoServizioRepository(NpceDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TipoServizio> GetByAll()
        {
            return _context.TipoServizio.AsNoTracking().ToList();
        }

        public TipoServizio GetByDescription(string description)
        {
            return _context.TipoServizio.Where(ts => ts.Descrizione == description).Single();
        }
    }
}
