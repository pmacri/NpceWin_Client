using NPCE_WinClient.DataAccess;
using NPCE_WinClient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Data.Repositories
{

    public class AnagraficaRepository : GenericRepository<Anagrafica,NpceDbContext>, IAnagraficaRepository
    {
        public AnagraficaRepository(NpceDbContext context): base(context)
        {
        }
      }
}
