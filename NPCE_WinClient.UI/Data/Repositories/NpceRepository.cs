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
    public class NpceRepository : INpceRepository
    {
        private readonly NpceDbContext _context;
        public NpceRepository(NpceDbContext context)
        {
            _context = context;
        }

        public void Add(Anagrafica anagrafica)
        {
            _context.Anagrafica.Add(anagrafica);
        }

        public async Task<List<Service>> GetAllAsync()
        {
            return await _context.Services.ToListAsync();
        }

        public async Task<Anagrafica> GetByIdAsync(int id)
        {
            return await _context.Anagrafica.FirstAsync(m => m.Id == id);
        }

        public async Task<Service> GetServiceByIdAsync(int id)
        {

            return await _context.Services.AsNoTracking().SingleOrDefaultAsync(s => s.Id == id);
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public void Remove(Anagrafica anagrafica)
        {
            _context.Anagrafica.Remove(anagrafica);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
