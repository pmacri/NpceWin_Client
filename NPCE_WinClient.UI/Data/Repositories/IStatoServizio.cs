using NPCE_WinClient.DataAccess;
using NPCE_WinClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Data.Repositories
{
    public interface IStatoServizioRepository
    {
        IEnumerable<StatoServizio> GetAll();
        StatoServizio GetByDescription(string description);
    }

    public class StatoServizioRepository : IStatoServizioRepository
    {
        private NpceDbContext _context;

        public StatoServizioRepository(NpceDbContext context)
        {
            _context = context;
        }
        public IEnumerable<StatoServizio> GetAll()
        {
            return _context.StatoServizio.AsNoTracking().ToList();
        }

        public StatoServizio GetByDescription(string description)
        {
            return _context.StatoServizio.Single(ss => ss.Descrizione == description);
        }
    }
}


