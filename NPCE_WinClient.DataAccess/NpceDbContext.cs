

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

        public DbSet<Visura> Visura { get; set; }
        public DbSet<VisureCodiceDocumento> VisureCodiceDocumento { get; set; }
        public DbSet<VisureFormatoDocumento> VisureFormatoDocumento { get; set; }
        public DbSet<VisureTipoDocumento> VisureTipoDocumento { get; set; }
        public DbSet<VisureTipoRecapito> VisureTipoRecapito { get; set; }
        public DbSet<Servizio> Servizio { get; set; }
        public DbSet<StatoServizio> StatoServizio { get; set; }
        public DbSet<Anagrafica> Anagrafica { get; set; }
        public DbSet<Documento> Documento { get; set; }
        public DbSet<Ambiente> Ambiente { get; set; }
        public DbSet<PaginaBollettino> PaginaBollettino { get; set; }
        public DbSet<Bollettino> Bollettino { get; set; }
        public DbSet<TipoServizio> TipoServizio { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Anagrafica>()
            //    .Ignore(a => a.IsMittente);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        
    }
}
