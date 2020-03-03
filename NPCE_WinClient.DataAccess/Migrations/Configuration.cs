namespace NPCE_WinClient.DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NPCE_WinClient.DataAccess.NpceDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(NPCE_WinClient.DataAccess.NpceDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            context.Services.AddOrUpdate(s => s.IdRichiesta,
                new Model.Service { IdRichiesta = Guid.NewGuid(), CreationTime = DateTime.Now, State = "Prepared" },
                new Model.Service { IdRichiesta = Guid.NewGuid(), CreationTime = DateTime.Now, State = "Prepared" },
                new Model.Service { IdRichiesta = Guid.NewGuid(), CreationTime = DateTime.Now, State = "Prepared" },
                new Model.Service { IdRichiesta = Guid.NewGuid(), CreationTime = DateTime.Now, State = "Prepared" },
                new Model.Service { IdRichiesta = Guid.NewGuid(), CreationTime = DateTime.Now, State = "Prepared" }
                );
        }
    }
}
