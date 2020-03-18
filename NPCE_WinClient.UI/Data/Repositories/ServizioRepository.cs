using NPCE_WinClient.DataAccess;
using NPCE_WinClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Data.Repositories
{
    public class ServizioRepository: GenericRepository<Servizio, NpceDbContext>, IServizioRepository
    {
        public ServizioRepository(NpceDbContext context) : base(context)
        {
        }

        public IEnumerable<Anagrafica> GetAll()
        {
            return Context.Anagrafica.ToList();
        }
    }
}
