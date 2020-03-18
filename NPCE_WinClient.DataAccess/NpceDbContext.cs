using NPCE_WinClient.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.DataAccess
{
    public class NpceDbContext: DbContext
    {
        public NpceDbContext() : base("NpceClientDB")
        {

        }
        public DbSet<Anagrafica> Anagrafica { get; set; }

        public DbSet<Documento> Documento { get; set; }

        public DbSet<Ambiente> Ambiente { get; set; }

        public DbSet<TipoServizio> TipoServizio { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        
    }
}
