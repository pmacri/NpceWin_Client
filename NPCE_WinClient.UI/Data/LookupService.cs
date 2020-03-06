using NPCE_WinClient.DataAccess;
using NPCE_WinClient.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Data
{
    public class LookupDataService : IServiceLookupDataService, IMittenteLookupDataService
    {
        private Func<NpceDbContext> _contextCreator;

        public LookupDataService(Func<NpceDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<IEnumerable<LookupItem>> GetMittenteLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Mittente.AsNoTracking().Select(m =>
                new LookupItem
                {
                    Id = m.Id,
                    DisplayMember = m.Cognome + " " + m.Nome + " " + m.ComplementoNominativo
                }).ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetServiceLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Services.AsNoTracking().Select(s =>
                new LookupItem
                {
                    Id = s.Id,
                    DisplayMember = s.Id + " " + s.State
                }).ToListAsync();
            }
        }

    }
}