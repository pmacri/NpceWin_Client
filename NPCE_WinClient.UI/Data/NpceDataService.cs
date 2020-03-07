using NPCE_WinClient.DataAccess;
using NPCE_WinClient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Data
{
    public class NpceDataService : INpceDataService
    {
        private readonly Func<NpceDbContext> _contextCreator;
        public NpceDataService(Func<NpceDbContext> contextCreator)
        {
            this._contextCreator = contextCreator;
        }
        public async Task<List<Service>> GetAllAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Services.AsNoTracking().ToListAsync();
            }
        }

        public async Task<Anagrafica> GetAnagraficaByIdAsync(long id)
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Anagrafica.AsNoTracking().FirstAsync(m => m.Id==id);
            }
        }

        public async Task<Service> GetServiceByIdAsync(long id)
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Services.AsNoTracking().SingleOrDefaultAsync(s => s.Id== id);
            }
        }

        public async Task SaveAsync(Anagrafica anagrafica)
        {
            using (var ctx = _contextCreator())
            {
                ctx.Anagrafica.Add(anagrafica);
                ctx.Entry(anagrafica).State = EntityState.Modified;
                await ctx.SaveChangesAsync();
            }
        }
    }
}
